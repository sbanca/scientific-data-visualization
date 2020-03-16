
using UnityEngine;

namespace TableTop
{
    public class MetricsUiItemManager : UiItemManager
    {

        private MetricsData _metricsData;
        public MetricsData metricsData
        {

            get { return _metricsData; }
            set
            {
                _metricsData = value;

                setValue("DurationValue", transformSecondsToTime((int)_metricsData.totalDurationInSeconds));

                setValue("DistanceValue", _metricsData.totalDistance.ToString());

                setValue("DelayValue", transformSecondsToTime((int)_metricsData.totalDelayInSeconds));

            }

        }



        //static constructor
        public static MetricsUiItemManager CreateComponent(GameObject where, MetricsData metricsData, int routeItemNumber)
        {

            MetricsUiItemManager routeUiItemManagerObject = where.AddComponent<MetricsUiItemManager>();

            routeUiItemManagerObject.metricsData = metricsData;

            routeUiItemManagerObject.itemNumber = routeItemNumber;


            return routeUiItemManagerObject;

        }


    }
}

