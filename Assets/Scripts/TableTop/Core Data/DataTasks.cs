using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace TableTop
{

    [Serializable]
    public class TasksDataGroups
    {
        public List<TasksData> List;

    }

    [Serializable]
    public class TasksData
    {
        public List<TaskData> List;       
        public UserID User;
        public string Title;
        public float[] Position;
        public float[] Scale;
        public float[] Rotation;
        public PanelType Type;      

        public async Task Update()
        {

            InitializeSelectionsAndMetrics();
            await UpdateRouteData();
            CreateUiElementList();
            UpdateRoutesMetrics();
            
        }

        public void InitializeSelectionsAndMetrics()
        {

            lock (List)
            {

                if (Type == PanelType.TASKASSEMBLYPANNEL)
                {
                    for (int i = 0; i < List.Count; i++)
                    {

                        for (int j = 0; j < this.List[i].Options.Count; j++)
                        {

                            if (j == this.List[i].SelectedOption) this.List[i].Options[j].Selected = true;

                            else this.List[i].Options[j].Selected = false;

                        }

                    }
                }
                else
                {

                    for (int i = 0; i < List.Count; i++)
                    {

                        this.List[i].TimeDifferenceInSeconds = 0;

                        this.List[i].OriginName = this.Title;

                    }
                }
            }
        }


        //routes creation
        public List<RouteData> SelectedRoutes;
        public List<RouteData> OptionalRoutes;

        private async Task UpdateRouteData()
        {
            
                if (Type == PanelType.TASKASSEMBLYPANNEL)
                {

                    SelectedRoutes = new List<RouteData>();
                    OptionalRoutes = new List<RouteData>();

                    for (int i = 1; i < List.Count; i++)
                    {

                        ////////////////////////////
                        //add a selected route start

                        OptionData startOption = List[i - 1].Options[List[i - 1].SelectedOption];

                        OptionData endOption = List[i].Options[List[i].SelectedOption];

                        SelectedRoutes.Add(new RouteData(startOption, endOption, RouteType.SELECTED));

                        await SelectedRoutes[i - 1].apiCall();

                        //add a selected route end 
                        ///////////////////////////


                        if (i == List.Count - 1)
                        {

                            //////////////////////////////
                            ////add an optional routes start
                            startOption = List[i - 1].Options[List[i - 1].SelectedOption];

                            int optionCount = 0;

                            for (int j = 0; j < List[i].Options.Count; j++)
                            {
                                OptionData option = List[i].Options[j];


                                if (!option.Selected)
                                { // if the option is not selected add it to the optional routes

                                    OptionalRoutes.Add(new RouteData(startOption, option, RouteType.OPTIONAL));

                                    List[i].Options[j].RouteSegment = startOption.Name + "_" + option.Name;

                                    await OptionalRoutes[optionCount].apiCall();

                                    optionCount += 1;

                                }

                            }

                            ///add an optional routes end 
                            /////////////////////////////

                        }
                    }

                }
            

        }


        //metrics
        public int StartTime;
        public int totalDuration;
        public int routeDuration;
        public int routeDistance;
        public int routeDelay;

        private void UpdateRoutesMetrics()
        {

            lock (List) {

                if (Type == PanelType.TASKASSEMBLYPANNEL)
                {

                    totalDuration = StartTime;

                    routeDuration = 0;

                    routeDelay = 0;

                    routeDistance = 0;

                    foreach (UiItem item in UiItemList)
                    {


                        switch (item.type)
                        {

                            case (UiItemType.ROUTE):

                                item.routeData.departure = totalDuration;

                                totalDuration += (int)item.routeData.duration;

                                routeDuration += (int)item.routeData.duration;

                                item.routeData.arrival = totalDuration;

                                routeDistance += (int)item.routeData.distance;

                                break;

                            case (UiItemType.ADDEDTASK):

                                //delay 
                                if (item.taskData.TimeLocked) item.taskData.TimeDifferenceInSeconds = totalDuration - item.taskData.TimeInSeconds;

                                if (item.taskData.TimeDifferenceInSeconds > 0) routeDelay += item.taskData.TimeDifferenceInSeconds;
                                //end delay

                                totalDuration += (int)item.taskData.Duration;

                                break;


                        }


                    }


                    foreach (UiItem item in UiItemList)
                    {


                        switch (item.type)
                        {



                            case (UiItemType.METRICS):

                                item.metricsData = new MetricsData(routeDistance, routeDuration, routeDelay);

                                break;

                        }

                    }

                }

            }
        
        }



        //pannel ui creation
        public List<UiItem> UiItemList;

        public void DisplayRoutes()
        {
            lock (List)
            {
                if (Type == PanelType.TASKASSEMBLYPANNEL)
                {

                    Routes.Instance.DeactivateAll();

                    foreach (RouteData rs in SelectedRoutes) Routes.Instance.Add(rs);

                    foreach (RouteData rs in OptionalRoutes) Routes.Instance.Add(rs);

                }
            }

        }

        public void CreateUiElementList()
        {

            lock (List)
            {
                UiItemList = new List<UiItem>();

                if (Type == PanelType.TASKASSEMBLYPANNEL)
                {

                    UiItemList.Add(new UiItem(UiItemType.METRICS, new MetricsData(0, 0, 0)));


                    for (int i = 0; i < List.Count; i++)
                    {

                        UiItemList.Add(new UiItem(UiItemType.ADDEDTASK, List[i]));

                        if (i < SelectedRoutes.Count)
                        {
                            UiItemList.Add(new UiItem(UiItemType.ROUTE, SelectedRoutes[i]));
                        }

                    }



                }
                else
                {

                    foreach (TaskData task in List) UiItemList.Add(new UiItem(UiItemType.TASK, task));

                }

                for (int i = 0; i < UiItemList.Count; i++) UiItemList[i].itemNumber = i;
            }
        }



        //get extract add remove tasks 
        public TaskData GetTask(string name)
        {

            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].Name == name) return List[i];

            }

            return null;

        }

        public TaskData ExtractTask(string name)
        {

            TaskData extractedTask = GetTask(name);

            if (extractedTask != null) RemoveTask(extractedTask);

            return extractedTask;
        }

        public void RemoveTask(TaskData extractedTask)
        {
            lock (List)
            {
                string name = extractedTask.Name;

                for (int i = 0; i < List.Count; i++)
                {

                    if (List[i].Name == name)
                    {

                        List.Remove(List[i]);

                        break;
                    }

                }
            }

        }

        public void AddTask(TaskData task)
        {

            lock (List)
            {
                List.Add(task); //add the task
            }

        }
    
    }

    [Serializable]
    public class TaskData
    {
        public string Name;
        public string Description;
        public bool Clickable;
        public List<OptionData> Options;
        public bool TimeLocked;
        public int TimeInSeconds;
        public int Duration; //duration in seconds
        public int SelectedOption;
        public string RouteSegment;

        public string OriginName;

        public int TimeDifferenceInSeconds;

        public OptionData returnSelectedOption()
        {
            OptionData selectedOption = null;

            for (int j = 0; j < this.Options.Count; j++)
            {

                if (j == this.SelectedOption)
                {
                    selectedOption = this.Options[j];
                    break;
                }
            }

            return selectedOption;

        }



    }

    [Serializable]
    public class OptionData
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

    public class MetricsData {

        public int totalDistance;
        public int totalDurationInSeconds;
        public int totalDelayInSeconds;

        public MetricsData(int distance, int duration, int delay) {

            this.totalDistance = distance;
            this.totalDurationInSeconds = duration;
            this.totalDelayInSeconds = delay;
        }

    }

    public class UiItem
    {

        public UiItemType type;
        public RouteData routeData;
        public TaskData taskData;
        public MetricsData metricsData;
        public int itemNumber;

        public UiItem(UiItemType type, object Data)
        {

            this.type = type;

            switch (type)
            {

                case (UiItemType.ROUTE):

                    this.routeData = (RouteData)Data;

                    break;

                case (UiItemType.TASK):

                    this.taskData = (TaskData)Data;

                    break;

                case (UiItemType.ADDEDTASK):

                    this.taskData = (TaskData)Data;

                    break;

                case (UiItemType.METRICS):

                    this.metricsData = (MetricsData)Data;

                    break;
            }

        }

    }



}