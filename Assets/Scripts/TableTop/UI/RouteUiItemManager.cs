
using UnityEngine;

namespace TableTop
{
    public class RouteUiItemManager : UiItemManager
    {

        private RouteData _routeData;
        public RouteData routeData
        {

            get { return _routeData; }
            set
            {
                _routeData = value;

                this.gameObject.name = _routeData.name;
             

                setValue("DepartureValue", transformSecondsToTime((int)_routeData.departure));//route departure time        
                
                setValue("DurationValue", transformSecondsToTime((int)_routeData.duration));//route duration 

                setValue("ArrivalValue", transformSecondsToTime((int)_routeData.arrival));//route Arrival  time 

                setValue("DistanceValue", _routeData.distance.ToString() + " m");//route distance

            }

        }


      
        //static constructor
        public static RouteUiItemManager CreateComponent(GameObject where, RouteData routeData, int routeItemNumber)
        {

            RouteUiItemManager routeUiItemManagerObject = where.AddComponent<RouteUiItemManager>();

            routeUiItemManagerObject.routeData = routeData;

            routeUiItemManagerObject.itemNumber = routeItemNumber;


            return routeUiItemManagerObject;

        }

   
    }
}

