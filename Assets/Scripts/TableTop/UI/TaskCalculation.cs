
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

            }
        }

        public async void ConnectTaskSelectedOptions(PannelTask StartTask, PannelTask EndTask) {

            //get coordinates

            Mapzen.LngLat start = new Mapzen.LngLat(StartTask.Options[0].Lng, StartTask.Options[0].Lat);

            Mapzen.LngLat end = new Mapzen.LngLat(EndTask.Options[0].Lng, EndTask.Options[0].Lat);

            //query API 

            Response direction = await openRoutService.Direction(start,end);

            //TODO need to save this in some form of cache somewehre 

            //sum up connection choiche

            totalDistance += direction.features[0].properties.summary.distance;

            totalDuration += direction.features[0].properties.summary.duration;

            //display the curve


        }

    }
}
