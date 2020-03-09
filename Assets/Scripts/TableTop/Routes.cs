﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TableTop
{
    public class Routes : Singleton<Routes>
    {
        private GameObject SelectedRouteParentContainer;

        private GameObject OptionalRouteParentContainer;

        private Coordinates MapCoordinates;

        private Map map;

        private bool[] largeBoolArray;

        private RouteCache routeCache = new RouteCache(100);

        public void Start()
        {

            

#if UNITY_EDITOR

            Scene scene = SceneManager.GetActiveScene();

            if (scene.name == "RouteTest") TestRoute();

#endif


            CreateParentsRouteContainer();


        }

        private void CreateParentsRouteContainer() {

            CreateSelectedParentRouteContainer();
            CreateOptionalParentRouteContainer();
        }

        private void CreateSelectedParentRouteContainer()
        {

            //check if it already Exists in the scene 

            SelectedRouteParentContainer = GameObject.Find("Selected Routes Container");

            if (SelectedRouteParentContainer != null) return;



            SelectedRouteParentContainer = new GameObject();

            //add parent 

            if (map == null) getMapInstance();

            SelectedRouteParentContainer.transform.parent = map.transform;


            //add name 

            SelectedRouteParentContainer.name = "Selected Routes Container";

        }

        private void CreateOptionalParentRouteContainer()
        {


            //check if it already Exists in the scene 

            OptionalRouteParentContainer = GameObject.Find("Optional Routes Container");

            if (OptionalRouteParentContainer != null) return;



            OptionalRouteParentContainer = new GameObject();

            //add parent 

            if (map == null) getMapInstance();

            OptionalRouteParentContainer.transform.parent = map.transform;


            //add name 

            OptionalRouteParentContainer.name = "Optional Routes Container";

        }
        
        public void TestRoute() {


            Vector3[] points = { new Vector3(1.281359f,0f,1.161354f),
                                new Vector3(1.26623f,0f,1.13849f),
                                new Vector3(1.257314f,0f,1.040955f),
                                new Vector3(1.262924f,0f,0.959941f),
                                new Vector3(1.270939f,0f,0.8932008f),
                                new Vector3(1.432842f,0f,0.8888396f),
                                new Vector3(1.432241f,0f,0.8218357f),
                                new Vector3(1.439555f,0f,0.7491496f),
                                new Vector3(1.484539f,0f,0.7635546f),
                                new Vector3(1.484539f, 0f, 0.7635546f),
                                new Vector3(1.551865f,0f,0.7852283f),
                                new Vector3(1.574107f,0f,0.7944793f),
                                new Vector3(1.631514f,0f,0.844831f),
                                new Vector3(1.693931f,0f,0.8996766f),
                                new Vector3(1.710963f, 0f, 0.9146104f),
                                new Vector3(1.778489f,0f,0.9742141f),
                                new Vector3(1.820668f,0f,1.012937f),
                                new Vector3(1.852027f,0f,1.041748f),
                                new Vector3(1.89631f,0f,0.9913949f)};

            CreateRouteMesh(points, "testRoute", gameObject,RouteType.SELECTED);

        }

        public void CreateRoute(Response routeData, RouteType type) {

            var points = FromLtdLngToLocalCoordinates(routeData.features[0].geometry.coordinatesRoute);

            if (map == null) getMapInstance();

            if (SelectedRouteParentContainer == null || OptionalRouteParentContainer ==null) CreateParentsRouteContainer();

            if (type == RouteType.SELECTED)  CreateRouteMesh(points, routeData.features[0].properties.name, SelectedRouteParentContainer, type);

            else if (type == RouteType.OPTIONAL) CreateRouteMesh(points, routeData.features[0].properties.name, OptionalRouteParentContainer, type);

        }

        private Vector3[] FromLtdLngToLocalCoordinates(double[][] points) {


            if (MapCoordinates == null) getCoordinateInstance();


            Vector3[] list = new Vector3[points.Length];

            for (int i = 0; i < points.Length; i++)
            {

                //A
                var coordinateA = new Mapzen.LngLat(points[i][0], points[i][1]);

                var pointA = MapCoordinates.LatLngToMapCurrentWorldCoordinates(coordinateA);

                list[i] = pointA;

            }

            return list;

        }

        private void CreateRouteMesh(Vector3[] points, string name, GameObject parent, RouteType type)
        {
            //check if route exists already 
            GameObject route = GameObject.Find(name);
            if (route != null) {
                //return;
                Destroy(route);
            }
                
        
            //creating go mesh object
            route = new GameObject();
            route.transform.parent = parent.transform;
            route.name = name;

            //creating Mesh and Renderer
            MeshFilter mesh = route.AddComponent<MeshFilter>();
            MeshRenderer renderer = route.AddComponent<MeshRenderer>();

            //this is optional could be removed
            Vector3[] reducedPoints = reducePointsbyDistance(points);
         
            //create mesh
            mesh.mesh.vertices = createVerticesFromPointRoute(reducedPoints, type);
            mesh.mesh.triangles = createtrianglesFromPointRoute(reducedPoints);
 
            //material
            setMaterialAndMaterialBoundaries(renderer,type);
        }

        public void setMaterialAndMaterialBoundaries(MeshRenderer renderer,RouteType type){

            name = type == RouteType.SELECTED ? "selected_route" : "optional_route";

            //createAsphereforVertex(mesh.mesh.vertices);
            renderer.material = Resources.Load("Materials/"+name, typeof(Material)) as Material;

            //Set Material Boundaries
            var cornersBounds = Map.Instance.useSlippyMap ? Boundaries.Instance.TableBounds : Boundaries.Instance.MapBounds;
            var corners = new Vector4(cornersBounds.min.x, cornersBounds.min.z, cornersBounds.size.x, cornersBounds.size.z);
            renderer.material.SetVector("_Corners", corners);

        }

        public float getBuildingsElevation(Vector3 LocalPosition)
        {

            float elevation = 0f;

            var buildings = GameObject.Find("Buildings");

            var childnumber = buildings.transform.childCount;

            for (int x = 0; x < childnumber; x++)
            {

                Transform child = buildings.transform.GetChild(x);

                MeshRenderer r = child.GetComponent<MeshRenderer>();

                if (r.bounds.Contains(LocalPosition))
                {

                    MeshCollider collider = child.gameObject.AddComponent<MeshCollider>();

                    RaycastHit hit = new RaycastHit();

                    Vector3 startingPoint = LocalPosition + (Vector3.up * 2); //move up the point 


                    //this is to cast rays around to make sure surrunding buidlings or small sapce do not obscure the object 

                    var ray = new Ray(startingPoint, Vector3.down);

                    if (collider.Raycast(ray, out hit, 200f))
                    {

                        elevation = hit.point.y;

                        UnityEngine.Object.Destroy(collider);

                        break;
                    }

                    UnityEngine.Object.Destroy(collider);
                }
            }

            return elevation;

        }

        public Vector3[] reducePointsbyDistance(Vector3[] points) {

            float threshold = 0.02f;

            List<Vector3> listPoints = new List<Vector3>();

            for (int i = 1; i < points.Length - 1; i++)
            {

                Vector3 pointPrevious = points[i - 1];//A
                Vector3 point = points[i];//B
                
                Vector3 distance = point - pointPrevious;

                if (distance.magnitude > threshold) listPoints.Add(point);



            }

            return listPoints.ToArray();

        }

        private void createAsphereforVertex(Vector3[] vertices) {

            for (int i = 0; i<vertices.Length;i++)
            {
                
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = vertices[i];
                var scale = new Vector3(0.005f, 0.005f, 0.005f);
                sphere.transform.localScale = scale;

                Renderer r = sphere.GetComponent<MeshRenderer>();

                var cas = i % 4;

                switch (cas)
                {
                    case (0):
                        r.material.color = Color.black; 
                        break;
                    case (2):
                        r.material.color = Color.red;
                        break;
                    case (3):
                        r.material.color = Color.yellow;
                        break;
                    case (4):
                        r.material.color = Color.red;
                        break;

                }
            }

        }

        private Vector3[] createVerticesFromPointRoute(Vector3[] points,RouteType type) {

            var Width = type == RouteType.SELECTED? 0.02f : 0.01f;
            var Height = type == RouteType.SELECTED ? 0.06f : 0.05f;

            Vector3[] vertices = new Vector3[points.Length * 4];

            //First point 

            var point = points[0];//A
            var pointNext = points[1];//B

            var direction = pointNext - point;
            var ortogonalToDirection = Vector3.Cross(direction, Vector3.up).normalized;
            var oldOrtogonalToDirection = ortogonalToDirection;

            vertices[0] = point + (ortogonalToDirection * Width );
            vertices[1] = point - (ortogonalToDirection * Width );
            vertices[2] = vertices[1] + Vector3.up * Height;
            vertices[3] = vertices[0] + Vector3.up * Height;           

            Vector3 pointPrevious;
            int i;

            //for all point when i>0 and i<lenght-1
            for (i = 1; i < points.Length - 1; i++)
            {

                pointPrevious = points[i-1];//A
                point = points[i];//B
                pointNext = points[i + 1];//C

                var directionPrevious = point - pointPrevious;
                var directionNext = pointNext - point;

                var ortogonalToDirectionPrevious = Vector3.Cross(directionPrevious, Vector3.up).normalized;                
                var ortogonalToDirectionNext = Vector3.Cross(directionNext, Vector3.up).normalized;

                ortogonalToDirection = (ortogonalToDirectionPrevious + ortogonalToDirectionNext) / 2;

                var angle = Vector3.Angle(ortogonalToDirection, ortogonalToDirectionPrevious) * Mathf.Deg2Rad;
                var secant = Mathf.Abs(1 / Mathf.Cos(angle));

                //
                //var elevationPoint1 = point + (ortogonalToDirection * Width * secant * 3);
                //var elevationPoint2 = point - (ortogonalToDirection * Width * secant * 3);

                //var elevation1 = getBuildingsElevation(elevationPoint1);
                //var elevation2 = getBuildingsElevation(elevationPoint1);

                //var elevation = elevation1 > elevation2 ? elevation1 : elevation2;

                //var h = Height > elevation ? Height : elevation;
                //

                //add vertices 
                vertices[i * 4] = point +  (ortogonalToDirection * Width * secant);
                vertices[i * 4 + 1] = point - (ortogonalToDirection * Width * secant);
                vertices[i * 4 + 2] = vertices[i * 4 + 1] + Vector3.up * Height;
                vertices[i * 4 + 3] = vertices[i * 4] + Vector3.up * Height;

            }

            //Last point 

            i = points.Length - 1;
            pointPrevious = points[i - 1];//A
            point = points[i];//B

            direction = point - pointPrevious;
            ortogonalToDirection = Vector3.Cross(direction, Vector3.up).normalized;

            vertices[i * 4] = point + (ortogonalToDirection * Width);
            vertices[i * 4 + 1] = point - (ortogonalToDirection * Width);
            vertices[i * 4 + 2] = vertices[i * 4 + 1] + Vector3.up * Height;
            vertices[i * 4 + 3] = vertices[i * 4] + Vector3.up * Height;

            return vertices;

        }

        private int[] createtrianglesFromPointRoute(Vector3[] points)
        {
           
            int totalVertices = points.Length * 4;//for every point 8 vaertices
            int totalFaces = (points.Length-1) * 8;//for every 2 point 8 faces 

            int[] triangles = new int[totalFaces*3];
            int[] face1 = {0,5,6};
            int[] face2 = {5,0,1};
            int[] face3 = {1,6,5};
            int[] face4 = {6,1,2};
            int[] face5 = {2,7,6};
            int[] face6 = {7,2,3};
            int[] face7 = {3,4,7};
            int[] face8 = {4,3,0};

            for (int i = 0; i < totalFaces*3; i=i+24)
            {
                var increment = (i / 24)*4;

                //first face
                triangles[i] = face1[0] + increment;
                triangles[i + 1] = face1[1] + increment;
                triangles[i + 2] = face1[2] + increment;

                //2 face
                triangles[i + 3] = face2[0] + increment;
                triangles[i + 4] = face2[1] + increment;
                triangles[i + 5] = face2[2] + increment;

                //3 face
                triangles[i + 6] = face3[0] + increment;
                triangles[i + 7] = face3[1] + increment;
                triangles[i + 8] = face3[2] + increment;

                //3 face
                triangles[i + 9] = face4[0] + increment;
                triangles[i + 10] = face4[1] + increment;
                triangles[i + 11] = face4[2] + increment;

                //5 face
                triangles[i + 12] = face5[0] + increment;
                triangles[i + 13] = face5[1] + increment;
                triangles[i + 14] = face5[2] + increment;

                //6 face
                triangles[i + 15] = face6[0] + increment;
                triangles[i + 16] = face6[1] + increment;
                triangles[i + 17] = face6[2] + increment;

                //7 face
                triangles[i + 18] = face7[0] + increment;
                triangles[i + 19] = face7[1] + increment;
                triangles[i + 20] = face7[2] + increment;

                //8 face
                triangles[i + 21] = face8[0] + increment;
                triangles[i + 22] = face8[1] + increment;
                triangles[i + 23] = face8[2] + increment;


            }

            return triangles;
        }

        public void DeleteRoutes()
        {

            //delete parent routes

            if (SelectedRouteParentContainer == null) return;

            var numberOfChildrens = SelectedRouteParentContainer.transform.childCount;

            Transform child;

            for (int x = 0; x < numberOfChildrens; x++)
            {

                child = SelectedRouteParentContainer.transform.GetChild(x);

                Destroy(child.gameObject);
            }


            //delete optional routes

            if (OptionalRouteParentContainer == null) return;

            numberOfChildrens = OptionalRouteParentContainer.transform.childCount;



            for (int x = 0; x < numberOfChildrens; x++)
            {

                child = OptionalRouteParentContainer.transform.GetChild(x);

                Destroy(child.gameObject);
            }

        }

        public void DeleteSelectedRoutesContainingTaskName(string TaskName) {

            if (SelectedRouteParentContainer == null) return;

            var numberOfChildrens = SelectedRouteParentContainer.transform.childCount;

            Transform child;

            for (int x = 0; x < numberOfChildrens; x++) {

                child = SelectedRouteParentContainer.transform.GetChild(x);

                if (child.name.Contains(TaskName)) Destroy(child.gameObject);
            }

        }

        public void DeleteOptionalRoutes()
        {

            if (OptionalRouteParentContainer == null) return;

            var numberOfChildrens = OptionalRouteParentContainer.transform.childCount;

            Transform child;

            for (int x = 0; x < numberOfChildrens; x++)
            {

                child = OptionalRouteParentContainer.transform.GetChild(x);

                Destroy(child.gameObject);
            }

        }

        //get instances 
        private void getCoordinateInstance()
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

        private void getMapInstance()
        {

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