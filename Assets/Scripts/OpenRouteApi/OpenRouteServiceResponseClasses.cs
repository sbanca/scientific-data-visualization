using System;
using System.Collections.Generic;
using UnityEngine;
using OVRSimpleJSON;


[Serializable]
public class Response
{
    public string type;
    public List<GeoFeature> features;
}

[Serializable]
public class GeoFeature
{
    public string type;
    public Geometry geometry;
    public Properties properties;
    public float Distance;

}

[Serializable]
public class Geometry
{
    public string type;
    public double[] coordinates;
    public double[][] coordinatesRoute;

}


[Serializable]
public class Properties
{
    public string id;
    public string gid;
    public string layer;
    public string source;
    public string source_id;
    public string name;
    public string housenumber;
    public string street;
    public string postalcode;
    public string confidence;
    public string distance;
    public string accuracy;
    public string country;
    public string country_gid;
    public string country_a;
    public string region;
    public string region_gid;
    public string region_a;
    public string county;
    public string county_gid;
    public string county_a;
    public string locality;
    public string locality_gid;
    public string locality_a;
    public string borough;
    public string borough_gid;
    public string neighbourhood;
    public string neighbourhood_gid;
    public string continent;
    public string continent_gid;
    public string label;
    public Summary summary;
}

public class Summary
{

    public float distance;
    public float duration;

}

public static class JSONNodeUnityExt
{
  
    public static double[][] ToVector2List(this JSONNode aNode)
    {
        double[][] list = new double[aNode.Count][];

        for (int i = 0; i < aNode.Count; i++) {

            list[i] = new double[2];
            list[i][0] = aNode[i][0].AsDouble;
            list[i][1] = aNode[i][1].AsDouble;

        }

        return list;

    }
}