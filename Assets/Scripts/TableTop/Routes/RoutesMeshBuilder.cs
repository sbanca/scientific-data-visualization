using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TableTop
{
    public class RoutesMeshBuilder: Singleton<RoutesMeshBuilder>
    {
     
        private Coordinates MapCoordinates;

        public Material material_selected;

        public Material material_optional;


        public void CreateRouteMesh(RouteManager route)
        {
            Vector3[] points = FromLtdLngToWorldCoordinates(route.routeData.coordinatesRoute);

            //creating Mesh and Renderer
            MeshFilter mesh = route.gameObject.AddComponent<MeshFilter>();
            MeshRenderer renderer = route.gameObject.AddComponent<MeshRenderer>();

            //this is optional could be removed
            //Vector3[] reducedPoints = reducePointsbyDistance(points);
            Vector3[] reducedPoints = points;

            //save both vertices 
            route.vertices_optional = createVerticesFromPointRoute(reducedPoints, RouteType.OPTIONAL);
            route.vertices_selected = createVerticesFromPointRoute(reducedPoints, RouteType.SELECTED);

            //create mesh
            mesh.mesh.vertices = createVerticesFromPointRoute(reducedPoints, route.type);
            mesh.mesh.triangles = createtrianglesFromPointRoute(reducedPoints);

            //material
            SetMaterialAndMaterialBoundaries(renderer, route.type);

        }

        public void SetMaterialAndMaterialBoundaries(MeshRenderer renderer, RouteType type)
        {

            if (material_selected == null || material_optional == null) LoadMaterials();


            switch (type)
            {

                case (RouteType.OPTIONAL):
                    renderer.material = material_optional;
                    break;

                case (RouteType.SELECTED):
                    renderer.material = material_selected;
                    break;

            }

        }

        public void LoadMaterials()
        {

            //createAsphereforVertex(mesh.mesh.vertices);
            material_selected = Resources.Load("Materials/selected_route", typeof(Material)) as Material;
            material_optional = Resources.Load("Materials/optional_route", typeof(Material)) as Material;

            UpdateMaterialsBoundaries();
        }

        public void UpdateMaterialsBoundaries() {

            //get Boundaries
            var cornersBounds = Map.Instance.useSlippyMap ? Boundaries.Instance.TableBounds : Boundaries.Instance.MapBounds;
            var corners = new Vector4(cornersBounds.min.x, cornersBounds.min.z, cornersBounds.size.x, cornersBounds.size.z);

            //update corners with boundaries
            material_selected.SetVector("_Corners", corners);
            material_optional.SetVector("_Corners", corners);

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

        public Vector3[] reducePointsbyDistance(Vector3[] points)
        {

            float threshold = 0.01f;

            List<Vector3> listPoints = new List<Vector3>();

            for (int i = 1; i < points.Length - 1; i++)
            {

                Vector3 pointPrevious = points[i - 1];//A
                Vector3 point = points[i];//B

                Vector3 distance = point - pointPrevious;

                if (distance.magnitude > threshold) listPoints.Add(point);


            }

            listPoints.Add(points[points.Length - 1]);

            return listPoints.ToArray();

        }

        private Vector3[] createVerticesFromPointRoute(Vector3[] points, RouteType type)
        {

            var Width = type == RouteType.SELECTED ? 0.02f : 0.01f;
            var Height = type == RouteType.SELECTED ? 0.06f : 0.05f;

            Vector3[] vertices = new Vector3[points.Length * 4];

            //First point 

            var point = points[0];//A
            var pointNext = points[1];//B

            var direction = pointNext - point;
            var ortogonalToDirection = Vector3.Cross(direction, Vector3.up).normalized;
            var oldOrtogonalToDirection = ortogonalToDirection;

            vertices[0] = point + (ortogonalToDirection * Width);
            vertices[1] = point - (ortogonalToDirection * Width);
            vertices[2] = vertices[1] + Vector3.up * Height;
            vertices[3] = vertices[0] + Vector3.up * Height;

            Vector3 pointPrevious;
            int i;

            //for all point when i>0 and i<lenght-1
            for (i = 1; i < points.Length - 1; i++)
            {

                pointPrevious = points[i - 1];//A
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
                vertices[i * 4] = point + (ortogonalToDirection * Width * secant);
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
            int totalFaces = (points.Length - 1) * 8;//for every 2 point 8 faces 

            int[] triangles = new int[totalFaces * 3];
            int[] face1 = { 0, 5, 6 };
            int[] face2 = { 5, 0, 1 };
            int[] face3 = { 1, 6, 5 };
            int[] face4 = { 6, 1, 2 };
            int[] face5 = { 2, 7, 6 };
            int[] face6 = { 7, 2, 3 };
            int[] face7 = { 3, 4, 7 };
            int[] face8 = { 4, 3, 0 };

            for (int i = 0; i < totalFaces * 3; i = i + 24)
            {
                var increment = (i / 24) * 4;

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

        private Vector3[] FromLtdLngToWorldCoordinates(double[][] points)
        {

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


    }



}