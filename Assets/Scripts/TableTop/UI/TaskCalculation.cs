
using OVRSimpleJSON;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace TableTop
{
    public class TaskCalculation : Singleton<TaskCalculation>
    {
        
        private float totalDistance;

        private float totalDuration;


        public void CalculateTask(PannelTasks tasklist) {

            Routes.Instance.DeactivateAll();

            for (int i = 1; i < tasklist.List.Count; i++ ) {

                SelectedRoutes(tasklist.List[i-1], tasklist.List[i]);

                if (i == tasklist.List.Count-1)
                    
                    OptionsRoutes(tasklist.List[i - 1], tasklist.List[i]);
            }

            //todo display route metrics of duration and length 

            //todo develop metrics of punctuality
        }

        public void SelectedRoutes(PannelTask StartTask, PannelTask EndTask) {


            OptionItem startOption = StartTask.Options[StartTask.SelectedOption];

            OptionItem endOption = EndTask.Options[EndTask.SelectedOption];

            Routes.Instance.Add(startOption, endOption, RouteType.SELECTED);

        }

        public void OptionsRoutes(PannelTask StartTask, PannelTask EndTask)
        {

            //get start coordinates

            OptionItem startOption = StartTask.Options[StartTask.SelectedOption];


            foreach (OptionItem option in EndTask.Options) {

                if(!option.Selected) Routes.Instance.Add(startOption, option, RouteType.OPTIONAL);

            }

        }

    }


    

}

