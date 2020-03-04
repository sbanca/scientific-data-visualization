
using OVRSimpleJSON;
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

            for(int i = 1; i < tasklist.List.Count; i++ ) {

                ConnectTaskSelectedOptions(tasklist.List[i-1], tasklist.List[i]);

                if (i == tasklist.List.Count-1)
                    
                    ConnectAllLastTaskOptions(tasklist.List[i - 1], tasklist.List[i]);
            }

            //todo display route metrics of duration and length 

            //todo develop metrics of punctuality
        }

        public async void ConnectTaskSelectedOptions(PannelTask StartTask, PannelTask EndTask) {


            //get start coordinates

            OptionItem startOption = StartTask.Options[StartTask.SelectedOption];

            Mapzen.LngLat start = new Mapzen.LngLat(startOption.Lng, startOption.Lat);


            //get end coordinates

            OptionItem endOption = EndTask.Options[EndTask.SelectedOption];

            Mapzen.LngLat end = new Mapzen.LngLat(endOption.Lng, endOption.Lat);


            //query API 

            Response direction = await openRoutService.Direction(start,end);

            //add name of route 

            direction.features[0].properties.name = StartTask.Name + "_" + EndTask.Name + "_SELECTED";


            //sum up connection choiche

            totalDistance += direction.features[0].properties.summary.distance;

            totalDuration += direction.features[0].properties.summary.duration;


            //create route
            Routes.Instance.CreateRoute(direction, RouteType.SELECTED);           

        }

        public async void ConnectAllLastTaskOptions(PannelTask StartTask, PannelTask EndTask)
        {

            //get start coordinates

            OptionItem startOption = StartTask.Options[StartTask.SelectedOption];

            Mapzen.LngLat start = new Mapzen.LngLat(startOption.Lng, startOption.Lat);



            foreach (OptionItem option in EndTask.Options) {

                
                //get end coordinates

                Mapzen.LngLat end = new Mapzen.LngLat(option.Lng, option.Lat);


                //query API 

                Response direction = await openRoutService.Direction(start, end);


                //add name of route 

                direction.features[0].properties.name = startOption.Name + "_" + option.Name + "_OPTIONAL";


                //this is not very clean get rid in refactoring
                //create route if option is not selected otherewise if it exist destroy 

                if (!option.Selected) Routes.Instance.CreateRoute(direction, RouteType.OPTIONAL);
                else {
                    var route = GameObject.Find(direction.features[0].properties.name);
                    if (route != null) Destroy(route);
                }
               
            }



        }

    }
}
