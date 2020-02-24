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
        public List<SpatialAnchors> Options;
        public bool TimeLocked; 
        public DateTime Time;
        public DateTimeOffset Duration;

    }

    [Serializable]
    public class SpatialAnchors
    {
        public string Name;
        public string Description;
        public Mapzen.LngLat LngLat;
        public Vector3 LocalPos;
        public SpatialAnchorsTypes Type;

    }

    [Serializable]
    public enum SpatialAnchorsTypes
    {

        PRINTSHOP = 0,
        HOTEL = 1,
        RESTAURANT = 2,
        ELECTRONICSHOP = 3


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