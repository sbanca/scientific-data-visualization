//script to connect to the open route api https://openrouteservice.org/

using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TableTop;
using OVRSimpleJSON;

public class OpenRouteService : Singleton<OpenRouteService>
{

    public string api_key = "5b3ce3597851110001cf624818b8cf7d927f414aadee32142ca60d63";
    
    public Uri baseAddress = new Uri("https://api.openrouteservice.org");

    private Coordinates MapCoordinates;

    private OpenRouteDirectionCache directionCache = new OpenRouteDirectionCache(100);

    public async Task<Response> Pois(Mapzen.LngLat coordinates)
    {
        if (MapCoordinates == null) GetCoordinatesInstance();

        var address = new Uri(baseAddress.OriginalString + "/geocode/reverse?api_key=" + api_key + "&point.lon=" + coordinates.longitude + "&point.lat=" + coordinates.latitude);

        using (var httpClient = new HttpClient { BaseAddress = address })
        {

            using (var response = await httpClient.GetAsync(address))
            {
                string responseData = await response.Content.ReadAsStringAsync();

                Response responseDataparsed = JsonUtility.FromJson<Response>(responseData);

                return responseDataparsed;

            }

        }
    }

    public async Task<GeoFeature> SinglePois(Mapzen.LngLat coordinates)
    {

        if (MapCoordinates == null) GetCoordinatesInstance();

        Response Response = await Pois(coordinates);

        Vector2 pointA = MapCoordinates.LatLngToMapLocalCoordinates(coordinates);


        for (int x = 0; x < Response.features.Count; x++)
        {

            GeoFeature f = Response.features[x];

            Mapzen.LngLat LngLatCoord = new Mapzen.LngLat(f.geometry.coordinates[0], f.geometry.coordinates[1]);

            Vector2 pointB = MapCoordinates.LatLngToMapLocalCoordinates(LngLatCoord);

            Response.features[x].Distance = Vector2.Distance(pointA, pointB);

        }

        List<GeoFeature> SortedList = Response.features.OrderBy(o => o.Distance).ToList();

        return SortedList[0];
    }

    public async Task<Response> Direction(Mapzen.LngLat Start, Mapzen.LngLat End) {

        if (MapCoordinates == null) GetCoordinatesInstance();

        //if (directionCache == null) GetDirectionCacheInstance();

        var address = new Uri(baseAddress.OriginalString + "/v2/directions/driving-car?api_key=" + api_key +
                                "&start=" + Start.longitude + "," + Start.latitude +
                                "&end=" + End.longitude     + "," + End.latitude );


        Response responseDataparsed = directionCache.Get(address);

        if (responseDataparsed != null)
        {
            return responseDataparsed;
        }
        else {

            using (var httpClient = new HttpClient { BaseAddress = address })
            {

                using (var response = await httpClient.GetAsync(address))
                {

                    string responseData = await response.Content.ReadAsStringAsync();

                    responseDataparsed = JsonUtility.FromJson<Response>(responseData);


                    // workaourd with OVRSimpleJSON

                    JSONNode data = JSON.Parse(responseData);

                    responseDataparsed.features[0].geometry.coordinatesRoute = data["features"][0]["geometry"]["coordinates"].ToVector2List();

                    responseDataparsed.features[0].properties = new Properties();

                    responseDataparsed.features[0].properties.summary = new Summary();

                    responseDataparsed.features[0].properties.summary.distance = data["features"][0]["properties"]["summary"]["distance"].AsFloat;

                    responseDataparsed.features[0].properties.summary.duration = data["features"][0]["properties"]["summary"]["duration"].AsFloat;


                    //save in cache 

                    directionCache.Add(address, responseDataparsed);


                    //return

                    return responseDataparsed;

                }
            }

        }
    }

    private void GetCoordinatesInstance(){

        MapCoordinates = Coordinates.Instance;     

    }
}


