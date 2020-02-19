using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTopMapCoordinates: MonoBehaviour
{

    public TableTopMapNavigation target;

    public  Vector3 WorldCoordinatesToMapLocalCoordiantes(Vector3 point)
    {
        Vector3 localCoordinate = target.gameObject.transform.InverseTransformPoint(point);
        return localCoordinate;
    }

    public  Vector3 MapLocalCoordinatesToWorldCoordinates(Vector3 point)
    {
        Vector3 WorldCoordinates = target.gameObject.transform.TransformPoint(point);
        return WorldCoordinates;
    }

    public Mapzen.MercatorMeters MapLocalCoordinateToMapLocalMercatorMeters(Vector3 localCoordinate)
    {

        float XmetersLocal = localCoordinate.x / target.UnitsPerMeter;
        float YmetersLocal = localCoordinate.z / target.UnitsPerMeter; // bare in mind z is y

        Mapzen.MercatorMeters LocalMercatorMeters = new Mapzen.MercatorMeters(XmetersLocal, YmetersLocal);
        return LocalMercatorMeters;

    }

    public Mapzen.MercatorMeters MapLocalCoordinateToMapWorldMercatorMeters(Vector3 localCoordinate)
    {

        Mapzen.MercatorMeters LocalMercatorMeters = MapLocalCoordinateToMapLocalMercatorMeters(localCoordinate);

        double XmetersWorld = LocalMercatorMeters.x + target.Origin.x;
        double YmetersWorld = LocalMercatorMeters.y + target.Origin.y;

        Mapzen.MercatorMeters WorldMercatorMeters = new Mapzen.MercatorMeters(XmetersWorld, YmetersWorld);
        return WorldMercatorMeters;

    }

    public Mapzen.LngLat MapLocalCoordinateToLtdLng(Vector3 localCoordinate)
    {
        Mapzen.MercatorMeters WorldMercatorMeters = MapLocalCoordinateToMapWorldMercatorMeters(localCoordinate);

        Mapzen.LngLat LtdLngcoordinate = Mapzen.Geo.Unproject(WorldMercatorMeters);

        return LtdLngcoordinate;

    }

    public Vector3 LtdLngToMapLocalCoordinates(Mapzen.LngLat LngLatCoordinate)
    {

        Mapzen.MercatorMeters mercmeters = Mapzen.Geo.Project(LngLatCoordinate);
        double XmetersLocal = mercmeters.x * target.UnitsPerMeter;
        double YmetersLocal = mercmeters.y * target.UnitsPerMeter;
        return new Vector3((float)XmetersLocal, 0f, (float)YmetersLocal);

    }


}
