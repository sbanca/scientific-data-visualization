using System.Threading.Tasks;
using UnityEngine;


namespace TableTop {
    
    public class RouteManager : MonoBehaviour
    {

        //Route constructor
        
        public RouteData routeData;

        public static RouteManager CreateComponent(GameObject where, RouteData routeData)
        {
            RouteManager route = where.AddComponent<RouteManager>();

            route.routeData = routeData;
            route.type = routeData.type;
            where.name = routeData.name;

            //set parent container 
            if (route.RouteParentContainer == null) route.CreateParentRouteContainer();

            route.gameObject.transform.parent = route.RouteParentContainer.transform;

            return route;

        }

        //route type 
        private RouteType _type;

        public RouteType type {

            get { return _type; }
            set {

                if (_type == value) return;

                _type = value;

                if (renderer != null)  ChangeMaterialBasedOnType();
                if (meshFilter != null) ChangeVerticesBasedOnType();

            }
        
        }


        //mesh

        private MeshRenderer renderer;

        private MeshFilter meshFilter;

        public Vector3[] vertices_selected;

        public Vector3[] vertices_optional;

        public void CreateMesh() {

            RoutesMeshBuilder.Instance.CreateRouteMesh(this); //this create a mesh filter and mesh renderer attached to the current gameobject

            renderer =  gameObject.GetComponent<MeshRenderer>();

            meshFilter = gameObject.GetComponent<MeshFilter>();


        }

        private void ChangeMaterialBasedOnType() {

            switch (_type) {

                case (RouteType.OPTIONAL):
                    renderer.material = RoutesMeshBuilder.Instance.material_optional;
                    break;

                case (RouteType.SELECTED):
                    renderer.material = RoutesMeshBuilder.Instance.material_selected;
                    break;

            }
           
        }

        private void ChangeVerticesBasedOnType()
        {
           
            switch (_type)
            {

                case (RouteType.OPTIONAL):
                    meshFilter.mesh.vertices = vertices_optional;
                    break;

                case (RouteType.SELECTED):
                    meshFilter.mesh.vertices = vertices_selected;
                    break;

            }

        }


        //parent 

        private Map map;

        private GameObject RouteParentContainer;

        private void CreateParentRouteContainer()
        {

            //check if it already Exists in the scene 

            RouteParentContainer = GameObject.Find("Routes");

            if (RouteParentContainer != null) return;



            RouteParentContainer = new GameObject();

            //add parent 

            if (map == null) getMapInstance();

            RouteParentContainer.transform.parent = map.transform;


            //add name 

            RouteParentContainer.name = "Routes";

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