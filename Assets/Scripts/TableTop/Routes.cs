using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class Routes : Singleton<Routes>
    {

        private Coordinates MapCoordinates;


        public void CreateRoute(Response routeData) {


            if (MapCoordinates == null) getCoordinates();

            var points = FromLtdLngToLocalCoordinates(routeData.features[0].geometry.coordinatesRoute);

            CreateRouteSegment(points, routeData.features[0].properties.name);

        }

        private Vector3[] FromLtdLngToLocalCoordinates(double[][] points) {

            Vector3[] list = new Vector3[points.Length];

            for(int i = 0; i < points.Length; i++)
            {

                //A
                var coordinateA = new Mapzen.LngLat(points[i][0], points[i][1]);

                var pointA = MapCoordinates.LatLngToMapLocalCoordinates(coordinateA);

                list[i] = pointA;

            }

            return list;

        }

        private void CreateRouteSegment(Vector3[] points, string name ) {

            //creating go renderer object
            GameObject route = new GameObject();
            route.transform.parent =  Map.Instance.gameObject.transform;
            route.name = name;

            //creating line
            LineRenderer lineRenderer = route.AddComponent<LineRenderer>();
            lineRenderer.material = (Material)Resources.Load("Materials/line", typeof(Material));
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = points.Length;
            lineRenderer.useWorldSpace = false;
            lineRenderer.numCornerVertices = 2;
            lineRenderer.numCapVertices = 2;

            //drawing line 
            for (int i = 0; i < points.Length; i++) lineRenderer.SetPosition(i, points[i]); 


        }

        private void getCoordinates()
        {


            if (Coordinates.Instance == null)
            {
                MapCoordinates = gameObject.GetComponent<Coordinates>();
            }
            else
            {
                MapCoordinates = Coordinates.Instance;
            }



        }
    
    }

}