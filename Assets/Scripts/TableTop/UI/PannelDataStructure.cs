using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        public UserID User;
        public string Title;
        public float[] Position;      
        public float[] Scale;
        public float[] Rotation;
        public PanelType Type;

        public void InitializeSelections() {


            if (Type == PanelType.TASKASSEMBLYPANNEL)
            {
                for (int i = 0; i < List.Count; i++)
                {

                    for (int j=0; j < this.List[i].Options.Count; j++) {

                        if (j == this.List[i].SelectedOption) this.List[i].Options[j].Selected = true;

                        else this.List[i].Options[j].Selected = false;

                        
                    }

                }
            }


        }

    }

    [Serializable]
    public class PannelTask
    {
        public string Name;
        public string Description;
        public List<OptionItem> Options;
        public bool TimeLocked; 
        public DateTime Time;
        public DateTimeOffset Duration;
        public int SelectedOption;
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

    }

    [Serializable]
    public enum SpatialAnchorType
    {

        PRINTSHOP = 0,
        HOTEL = 1,
        RESTAURANT = 2,
        ELECTRONICSHOP = 3,
        WORKMEETING=4

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
    public class OptionClicked : UnityEvent<string> { }

    // Class declaration
    [System.Serializable]
    public class TaskOptionClicked : UnityEvent<string,string> { }



}