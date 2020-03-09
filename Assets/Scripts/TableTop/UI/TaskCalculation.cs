
using OVRSimpleJSON;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace TableTop
{
    public class TaskCalculation : Singleton<TaskCalculation>
    {
        OpenRouteService openRoutService;

        private float totalDistance;

        private float totalDuration;

        public void Start()
        {
            openRoutService = OpenRouteService.Instance;
        }

        public void CalculateTask(PannelTasks tasklist) {

            Routes.Instance.DeleteRoutes();

            for (int i = 1; i < tasklist.List.Count; i++ ) {

                ConnectTaskSelectedOptions(tasklist.List[i-1], tasklist.List[i]);

                if (i == tasklist.List.Count-1)
                    
                    ConnectAllLastTaskOptions(tasklist.List[i - 1], tasklist.List[i]);
            }

            //todo display route metrics of duration and length 

            //todo develop metrics of punctuality
        }

        public async Task ConnectTaskSelectedOptions(PannelTask StartTask, PannelTask EndTask) {


            OptionItem startOption = StartTask.Options[StartTask.SelectedOption];

            OptionItem endOption = EndTask.Options[EndTask.SelectedOption];


            RoutesBetweenOptions route = new RoutesBetweenOptions(startOption, endOption, RouteType.SELECTED);

            //query API 

            await Task.Run(()=>route.apiCall());

            //sum up connection choiche

            totalDistance += route.direction.features[0].properties.summary.distance;

            totalDuration += route.direction.features[0].properties.summary.duration;
            
            // 
            Routes.Instance.CreateRoute(route.direction, RouteType.OPTIONAL);

        }

        public async void ConnectAllLastTaskOptions(PannelTask StartTask, PannelTask EndTask)
        {

            //get start coordinates

            OptionItem startOption = StartTask.Options[StartTask.SelectedOption];


            foreach (OptionItem option in EndTask.Options) {


                //create route
                RoutesBetweenOptions route = new RoutesBetweenOptions(startOption, option, RouteType.OPTIONAL);

                //query API 
                await Task.Run(() => route.apiCall());


                //this is not very clean get rid in refactoring
                //create route if option is not selected otherewise if it exist destroy 

                if (!option.Selected) Routes.Instance.CreateRoute(route.direction, RouteType.OPTIONAL);
                else {
                    var routeObject = GameObject.Find(route.direction.features[0].properties.name);
                    if (routeObject != null) Destroy(routeObject);
                }
               
            }



        }

    }


    [Serializable]
    public class RoutesBetweenOptions
    {

        //constructor
        public RoutesBetweenOptions(OptionItem startOption, OptionItem endOption, RouteType type  )
        {
            this.startOption = startOption;
            this.endOption = endOption;
            this.type = type;

        }


        //name id
        public string name=null;

        private void CreateName() {

            name = _startOption.Name + "_" + _endOption.Name + "_SELECTED";

        }


        //route type 

        public RouteType type;


        //start coordinates

        private OptionItem _startOption;
        
        private Mapzen.LngLat start;
        
        public OptionItem startOption {

            get { return _startOption; }
            set {

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

        OpenRouteService openRoutService;

        public Response direction;
        public async void apiCall() {

            //get the API

            if(openRoutService==null) openRoutService = OpenRouteService.Instance;

            //query API 

            direction = await openRoutService.Direction(start, end);

            //add name of route 

            if (name == null) CreateName();

            direction.features[0].properties.name = name;

           

        }

    

    }

}

