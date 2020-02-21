using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableTop
{

    public class Interaction : Singleton<Interaction>
    {
        private Coordinates MapCoordinates;

        private OpenRouteService MapOpenRouteService;

        private Vector4 TableTopSize;

        private Annotations MapAnnotations;

        private Camera MainCam;

        private BoxCollider MapCollider;
        private void Start()
        {

            MapCoordinates = Coordinates.Instance;

            MapOpenRouteService = OpenRouteService.Instance;

            MapCollider = Map.Instance.gameObject.GetComponent<BoxCollider>();
            
            TableTopSize = Boundaries.Instance.slippyMapSize;

            MainCam = Camera.main;

        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                var point = GetClickedPoint();

                if (point == null) return;

                Vector3 p = (Vector3)point;

                GetPointInfo(p);
                SpanAnnotation(p);

            }
        }

        private RaycastHit hit;

        private Ray ray;

        private Nullable<Vector3> GetClickedPoint()
        {
            ray = MainCam.ScreenPointToRay(Input.mousePosition);          

            if (MapCollider.Raycast(ray, out hit, 200f))
            {

                if (hit.point.x < TableTopSize.x || hit.point.x > TableTopSize.z || hit.point.z < TableTopSize.y || hit.point.z > TableTopSize.w)
                {
                    
                    return null;
                }

                return hit.point;

            }

            return null;
        }

        private async void GetPointInfo(Vector3 point)
        {
            Vector3 localCoordinate = MapCoordinates.WorldCoordinatesToMapLocalCoordiantes(point);
            Mapzen.MercatorMeters LocalMercMeters = MapCoordinates.MapLocalCoordinateToMapLocalMercatorMeters(localCoordinate);
            Mapzen.MercatorMeters WorldMercMeters = MapCoordinates.MapLocalCoordinateToMapWorldMercatorMeters(localCoordinate);
            Mapzen.LngLat pointLngLtd = MapCoordinates.MapLocalCoordinateToLtdLng(localCoordinate);
            GeoFeature closestfeature = await MapOpenRouteService.SinglePois(pointLngLtd);

            Debug.Log("GET POINT INFO \n" + 
                      closestfeature.properties.label + " Lng: " + pointLngLtd.longitude + " Ltd: " + pointLngLtd.latitude+
                      "\n Unity World Coordinate: " + point +
                      "\n Local Coordinate: " + localCoordinate +
                      "\n Marcators Meter from map origin X:" + LocalMercMeters.x + " Y:" + LocalMercMeters.y +
                      "\n Marcators Meters" + WorldMercMeters.x + " Y:" + WorldMercMeters.y );
     
        }

        private void SpanAnnotation(Vector3 point) {

            if (MapAnnotations==null) GetAnnotationInstance();

            MapAnnotations.SpawnAnnotation(point);
        }

        private void GetAnnotationInstance() {
            
            MapAnnotations = Annotations.Instance;

        }
    }
}




