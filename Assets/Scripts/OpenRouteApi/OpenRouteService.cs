//script to connect to the open route api https://openrouteservice.org/

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.Linq;

public class OpenRouteService : Singleton<OpenRouteService>
{

    public string api_key = "5b3ce3597851110001cf624818b8cf7d927f414aadee32142ca60d63";
    public Uri baseAddress = new Uri("https://api.openrouteservice.org");

    public async Task<PoisResponse> Pois(Mapzen.LngLat coordinates)
    {

        var address = new Uri(baseAddress.OriginalString + "/geocode/reverse?api_key=" + api_key + "&point.lon=" + coordinates.longitude + "&point.lat=" + coordinates.latitude);

        Debug.Log(address);

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



}
