using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace TableTop
{

    [Serializable]
    public class PannelsList
    {
        public List<PannelTasks> List;

    }

    [Serializable]
    public class PannelTasks
    {
        public List<PannelTask> List;
        public List<RouteData> SelectedRoutes;
        public List<RouteData> OptionalRoutes;
        public UserID User;
        public string Title;
        public float[] Position;
        public float[] Scale;
        public float[] Rotation;
        public PanelType Type;

        public int startTime;
        public int totalDuration;
        public int totalDistance;

        public async Task Update()
        {

            InitializeSelections();
            await UpdateRouteData();
            UpdateRouteDuration();
            UpdateRouteDistance();

        }
        
        public void InitializeSelections() {


            if (Type == PanelType.TASKASSEMBLYPANNEL)
            {
                for (int i = 0; i < List.Count; i++)
                {

                    for (int j = 0; j < this.List[i].Options.Count; j++) {

                        if (j == this.List[i].SelectedOption) this.List[i].Options[j].Selected = true;

                        else this.List[i].Options[j].Selected = false;


                    }

                }
            }

        }

        private async Task UpdateRouteData() {

            if (Type == PanelType.TASKASSEMBLYPANNEL)
            {
               
                SelectedRoutes = new List<RouteData>();
                OptionalRoutes = new List<RouteData>();

                for (int i = 1; i < List.Count; i++)
                {

                    ////////////////////////////
                    //add a selected route start

                    OptionItem startOption = List[i - 1].Options[List[i - 1].SelectedOption];

                    OptionItem endOption = List[i].Options[List[i].SelectedOption];

                    SelectedRoutes.Add(new RouteData(startOption, endOption, RouteType.SELECTED));

                    await SelectedRoutes[i-1].apiCall();

                    //add a selected route end 
                    ///////////////////////////


                    if (i == List.Count - 1)

                        //////////////////////////////
                        ////add an optional routes start
                        startOption = List[i - 1].Options[List[i - 1].SelectedOption];

                        int optionCount = 0;

                        for (int j = 0; j < List[i].Options.Count; j++)
                        {
                            OptionItem option = List[i].Options[j];
                            
                            
                            if (!option.Selected) { // if the option is not selected add it to the optional routes

                                OptionalRoutes.Add(new RouteData(startOption, option, RouteType.OPTIONAL));

                                List[i].Options[j].RouteSegment = startOption.Name + "_" + option.Name;

                                await OptionalRoutes[optionCount].apiCall();

                                optionCount +=1;

                            }

                        }

                        ///add an optional routes end 
                        /////////////////////////////
                }

            }


        }

        private void UpdateRouteDuration()
        {

            if (Type == PanelType.TASKASSEMBLYPANNEL)
            {

                totalDuration = startTime;

                foreach (PannelTask pt in List) totalDuration += pt.Duration;

                foreach (RouteData rd in SelectedRoutes) totalDuration += (int)rd.duration;

            }
        }

        private void UpdateRouteDistance()
        {

            if (Type == PanelType.TASKASSEMBLYPANNEL)
            {

                totalDistance = 0;

                foreach (RouteData rd in SelectedRoutes) totalDuration += (int)rd.distance;

            }
        }


    }


    [Serializable]
    public class PannelTask
    {
        public string Name;
        public string Description;
        public bool Draggable;
        public List<OptionItem> Options;
        public bool TimeLocked; 
        public DateTime Time;
        public int Duration; //duration in seconds
        public int SelectedOption;
        public string RouteSegment;
        
        public OptionItem returnSelectedOption()
        {
            OptionItem selectedOption=null;

            for (int j = 0; j < this.Options.Count; j++)
            {

                if (j == this.SelectedOption) { 
                    selectedOption = this.Options[j];
                    break;
                }
            }

            return selectedOption;

        }

     

    }


    [Serializable]
    public class OptionItem
    {
        public string Name;
        public string Description;
        public double Lng;
        public double Lat;
        public Vector3 LocalPos;
        public SpatialAnchorType Type;
        public int number;
        public bool Selected;
        public string RouteSegment;

    }


    [Serializable]
    public enum SpatialAnchorType
    {

        PRINTSHOP = 0,
        HOTEL = 1,
        RESTAURANT = 2,
        ELECTRONICSHOP = 3,
        WORKMEETING=4,
        APPLESTORE=5,
        AIRPORT=6

    }

    [Serializable]
    public enum UserID
    {

        FIRST = 0,
        SECOND = 1
    }

    [Serializable]
    public enum PanelType
    {

        USERPANEL = 0,
        TASKASSEMBLYPANNEL = 1,
        INFO = 3
    }

    [Serializable]
    public enum RouteType
    {

        SELECTED = 0,
        OPTIONAL = 1

    }

    // Class declaration
    [System.Serializable]
    public class OptionClicked : UnityEngine.Events.UnityEvent<string> { }

    // Class declaration
    [System.Serializable]
    public class TaskOptionClicked : UnityEngine.Events.UnityEvent<string,string> { }



}