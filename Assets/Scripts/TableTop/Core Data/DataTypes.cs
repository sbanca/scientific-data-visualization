using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace TableTop
{

    public enum UiItemType
    {

        TASK = 0,
        ROUTE = 1,
        METRICS =2,
        ADDEDTASK=3
    }


    [Serializable]
    public enum SpatialAnchorType
    {

        PRINTSHOP = 0,
        HOTEL = 1,
        RESTAURANT = 2,
        ELECTRONICSHOP = 3,
        WORKMEETING = 4,
        APPLESTORE = 5,
        AIRPORT = 6

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

    [Serializable]
    public enum RulerCoordinateType
    {

        LETTERS = 0,
        NUMBERS = 1

    }


}