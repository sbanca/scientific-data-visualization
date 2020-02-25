using System;
using System.Collections.Generic;
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
        public UserID User;
        public string Title;
        public float[] Position;      
        public float[] Scale;
        public float[] Rotation;
        public PanelType Type;

    }

    [Serializable]
    public class PannelTask
    {
        public string Name;
        public string Description;
        public List<SpatialAnchor> Options;
        public bool TimeLocked; 
        public DateTime Time;
        public DateTimeOffset Duration;

    }

    [Serializable]
    public class SpatialAnchor
    {
        public string Name;
        public string Description;
        public double Lng;
        public double Lat;
        public Vector3 LocalPos;
        public SpatialAnchorType Type;

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

}