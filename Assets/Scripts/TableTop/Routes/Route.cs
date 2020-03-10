using System.Threading.Tasks;
using UnityEngine;


namespace TableTop {
    
    public class Route : MonoBehaviour
    {


        //Route constructor

        public static Route CreateComponent(GameObject where, OptionItem startOption, OptionItem endOption, RouteType type)
        {
            Route route = where.AddComponent<Route>();

            route.startOption = startOption;
            route.endOption = endOption;
            route.type = type;

            //set name
            route.UpdateName();

            //set parent container 
            if (route.RouteParentContainer == null) route.CreateParentRouteContainer();

            route.gameObject.transform.parent = route.RouteParentContainer.transform;

            return route;

        }

        public async void Generate() {

            await apiCall();

            CreateMesh();

        }

        public string name;
        private void UpdateName()
        {

            name = _startOption.Name + "_" + _endOption.Name;

            gameObject.name = name;

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


        //start coordinates

        private OptionItem _startOption;

        private Mapzen.LngLat start;

        public OptionItem startOption
        {

            get { return _startOption; }
            set
            {

                _startOption = value;

                start = new Mapzen.LngLat(_startOption.Lng, _startOption.Lat);

            }

        }



        //end coordinates

        private OptionItem _endOption;

        private Mapzen.LngLat end;

        public OptionItem endOption
        {

            get { return _endOption; }
            set
            {

                _endOption = value;

                end = new Mapzen.LngLat(_endOption.Lng, _endOption.Lat);

            }

        }



        //direction 

        private OpenRouteService openRoutService;

        public double[][] coordinatesRoute;

        public async Task apiCall()
        {

            //get the API

            if (openRoutService == null) openRoutService = OpenRouteService.Instance;

            //query API 

            Response response = await openRoutService.Direction(start, end);

            coordinatesRoute = response.features[0].geometry.coordinatesRoute;

        }


        //create mesh

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