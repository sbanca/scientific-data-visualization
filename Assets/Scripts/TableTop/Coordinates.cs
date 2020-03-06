using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class Coordinates : Singleton<Coordinates>
    {
        private Map map;

        public Vector3 WorldCoordinatesToMapLocalCoordiantes(Vector3 point)
        {
            Vector3 localCoordinate = Map.Instance.gameObject.transform.InverseTransformPoint(point);
            return localCoordinate;
        }

        public Vector3 MapLocalCoordinatesToWorldCoordinates(Vector3 point)
        {
            Vector3 WorldCoordinates = Map.Instance.gameObject.transform.TransformPoint(point);
            return WorldCoordinates;
        }

        public Mapzen.MercatorMeters MapLocalCoordinateToMapLocalMercatorMeters(Vector3 localCoordinate)
        {

            float XmetersLocal = localCoordinate.x / Map.Instance.UnitsPerMeter;
            float YmetersLocal = localCoordinate.z / Map.Instance.UnitsPerMeter; // bare in mind z is y

            Mapzen.MercatorMeters LocalMercatorMeters = new Mapzen.MercatorMeters(XmetersLocal, YmetersLocal);
            return LocalMercatorMeters;

        }

        public Mapzen.MercatorMeters MapLocalCoordinateToMapWorldMercatorMeters(Vector3 localCoordinate)
        {

            var Origin = getMapOrigin();

            Mapzen.MercatorMeters LocalMercatorMeters = MapLocalCoordinateToMapLocalMercatorMeters(localCoordinate);

            double XmetersWorld = LocalMercatorMeters.x + Origin.x;
            double YmetersWorld = LocalMercatorMeters.y + Origin.y;

            Mapzen.MercatorMeters WorldMercatorMeters = new Mapzen.MercatorMeters(XmetersWorld, YmetersWorld);
            return WorldMercatorMeters;

        }

        public Mapzen.LngLat MapLocalCoordinateToLtdLng(Vector3 localCoordinate)
        {
            Mapzen.MercatorMeters WorldMercatorMeters = MapLocalCoordinateToMapWorldMercatorMeters(localCoordinate);

            Mapzen.LngLat LtdLngcoordinate = Mapzen.Geo.Unproject(WorldMercatorMeters);

            return LtdLngcoordinate;

        }

        public Vector3 LatLngToMapLocalCoordinates(Mapzen.LngLat LngLatCoordinate)
        {
            if (map == null) getMapInstance();

            var Origin = getMapOrigin();

            Mapzen.MercatorMeters mercmeters = Mapzen.Geo.Project(LngLatCoordinate);

            double XmetersLocal = ( mercmeters.x - Origin.x ) * map.UnitsPerMeter ;

            double YmetersLocal = ( mercmeters.y - Origin.y ) * map.UnitsPerMeter ; 

            return new Vector3((float)XmetersLocal, 0f, (float)YmetersLocal);

        }

        public Vector3 LatLngToMapCurrentWorldCoordinates(Mapzen.LngLat LngLatCoordinate)
        {

            Vector3 MapCurrentWorldPosition = getMapCurrentWorldPosition();

            return  LatLngToMapLocalCoordinates(LngLatCoordinate) + MapCurrentWorldPosition;

        }

        private Vector2 getMapOrigin()
        {

            if (map == null) getMapInstance();

            return map.Origin;

        }

        private Vector3 getMapCurrentWorldPosition()
        {

            if (map == null) getMapInstance();

            return map.gameObject.transform.position; 

        }

        private void getMapInstance() {

            if (Map.Instance == null)
            {
                map = gameObject.GetComponent<Map>();
            }
            else
            {
                map = Map.Instance;
            }

        }
    }

}

