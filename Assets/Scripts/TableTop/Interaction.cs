using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableTop
{

    public class Interaction : Singleton<Interaction>
    {
        private Coordinates MapCoordinates;

        private OpenRouteService MapOpenRouteService;

        private Vector4 size;

        private void Start() {

                MapCoordinates = Coordinates.Instance;

                MapOpenRouteService = OpenRouteService.Instance;

                size = new Vector4(Boundaries.Instance.TableBounds.min.x, Boundaries.Instance.TableBounds.min.z, Boundaries.Instance.TableBounds.max.x, Boundaries.Instance.TableBounds.max.z);

        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Clicked();
            }
        }

        private async void Clicked()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.point.x < size.x || hit.point.x > size.z || hit.point.z < size.y || hit.point.z > size.w)
                {
                    return;
                }

                
                Vector3 localCoordinate = MapCoordinates.WorldCoordinatesToMapLocalCoordiantes(hit.point);
                Mapzen.MercatorMeters LocalMercMeters = MapCoordinates.MapLocalCoordinateToMapLocalMercatorMeters(localCoordinate);
                Mapzen.MercatorMeters WorldMercMeters = MapCoordinates.MapLocalCoordinateToMapWorldMercatorMeters(localCoordinate);
                Mapzen.LngLat pointLngLtd = MapCoordinates.MapLocalCoordinateToLtdLng(localCoordinate);
                PoisResponse response = await MapOpenRouteService.Pois(pointLngLtd);
                GeoFeature closestfeature = ClosestFeature(response, pointLngLtd);

                Debug.Log("Object: " + hit.collider.gameObject.name);
                Debug.Log("World Coordinate: " + hit.point);
                Debug.Log("Local Coordinate: " + localCoordinate);
                Debug.Log("Marcators Meter from map origin X:" + LocalMercMeters.x + " Y:" + LocalMercMeters.y);
                Debug.Log("Marcators Meters" + WorldMercMeters.x + " Y:" + WorldMercMeters.y);
                Debug.Log("Lng: " + pointLngLtd.longitude + " Ltd:" + pointLngLtd.latitude);
                Debug.Log(closestfeature.properties.label);

            }
        }

        public GeoFeature ClosestFeature(PoisResponse Response, Mapzen.LngLat coordinates)
        {



            Vector2 pointA = MapCoordinates.LtdLngToMapLocalCoordinates(coordinates);


            //for each feature get mercatorsmeters and compute distance 

            for (int x = 0; x < Response.features.Count; x++)
            {

                GeoFeature f = Response.features[x];

                Mapzen.LngLat LngLatCoord = new Mapzen.LngLat(f.geometry.coordinates[0], f.geometry.coordinates[1]);

                Vector2 pointB = MapCoordinates.LtdLngToMapLocalCoordinates(LngLatCoord);

                Response.features[x].Distance = Vector2.Distance(pointA, pointB);

            }

            List<GeoFeature> SortedList = Response.features.OrderBy(o => o.Distance).ToList();

            return SortedList[0];
        }

    }

}
