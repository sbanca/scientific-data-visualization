//script to connect to the open route api https://openrouteservice.org/

using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TableTop;

public class OpenRouteService : Singleton<OpenRouteService>
{

    public string api_key = "5b3ce3597851110001cf624818b8cf7d927f414aadee32142ca60d63";
    
    public Uri baseAddress = new Uri("https://api.openrouteservice.org");

    private Coordinates MapCoordinates;

    public async Task<PoisResponse> Pois(Mapzen.LngLat coordinates)
    {
        if (MapCoordinates == null) GetCoordinatesInstance();

        var address = new Uri(baseAddress.OriginalString + "/geocode/reverse?api_key=" + api_key + "&point.lon=" + coordinates.longitude + "&point.lat=" + coordinates.latitude);

        using (var httpClient = new HttpClient { BaseAddress = address })
        {

            using (var response = await httpClient.GetAsync(address))
            {
                string responseData = await response.Content.ReadAsStringAsync();

                PoisResponse responseDataparsed = JsonUtility.FromJson<PoisResponse>(responseData);

                return responseDataparsed;

            }

        }
    }

    public async Task<GeoFeature> SinglePois(Mapzen.LngLat coordinates)
    {

        if (MapCoordinates == null) GetCoordinatesInstance();

        PoisResponse Response = await Pois(coordinates);

        Vector2 pointA = MapCoordinates.LtdLngToMapLocalCoordinates(coordinates);


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

    private void GetCoordinatesInstance(){

        MapCoordinates = Coordinates.Instance; 

    }
}




