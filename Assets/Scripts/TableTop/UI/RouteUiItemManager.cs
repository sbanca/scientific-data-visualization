
using UnityEngine;

namespace TableTop
{
    public class RouteUiItemManager : MonoBehaviour
    {

        private RouteData _routeData;
        public RouteData routeData
        {

            get { return _routeData; }
            set
            {
                _routeData = value;

                this.gameObject.name = _routeData.name;

                setValue("DurationValue", _routeData.duration.ToString());

                setValue("DistanceValue", _routeData.distance.ToString());

            }

        }

        private int _routeItemNumber;

        public int routeItemNumber
        {

            get { return _routeItemNumber; }
            set
            {
                _routeItemNumber = value;
                setPos();
            }

        }

        public float startingValue = 0.258f;

        public float height = 0.05f;


        //static constructor
        public static RouteUiItemManager CreateComponent(GameObject where, RouteData routeData, int routeItemNumber)
        {

            RouteUiItemManager routeUiItemManagerObject = where.AddComponent<RouteUiItemManager>();

            routeUiItemManagerObject.routeData = routeData;

            routeUiItemManagerObject.routeItemNumber = routeItemNumber;


            return routeUiItemManagerObject;

        }


        //other methods
        private void setValue(string ValueName, string value)
        {
            TextMesh valueToSet = getTextMeshChildByName(ValueName);

            valueToSet.text = value;          

        }

        private TextMesh getTextMeshChildByName(string name)
        {

            TextMesh[] TextMeshes = this.gameObject.GetComponentsInChildren<TextMesh>();

            TextMesh textMesh = null;

            foreach (TextMesh tm in TextMeshes)
            {

                if (tm.name == name)
                {

                    textMesh = tm;

                    break;
                }

            }

            return textMesh;

        }

        private void setPos()
        {
            float newValue = startingValue - (_routeItemNumber * height);

            this.gameObject.transform.localPosition = new Vector3(0f, 0f, newValue);

        }


    }
}

