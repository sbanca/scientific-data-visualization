using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class PannelItem : MonoBehaviour
    {

        private PannelTask _pannelTask;
        public PannelTask pannelTask
        {

            get { return _pannelTask; }
            set
            {
                _pannelTask = value;
                setTitle();
            }

        }

        private int _panelItemNumber;

        public int panelItemNumber
        {

            get { return _panelItemNumber; }
            set
            {
                _panelItemNumber = value;
                setPos();
            }

        }


        public float startingValue = 0.218f;

        public float height = 0.1f  ;

        private TextMesh title;
        private void setTitle()
        {

            title = this.gameObject.GetComponentInChildren<TextMesh>();

            title.text = _pannelTask.Name;

            gameObject.name = _pannelTask.Name;

        }

        private void setPos()
        {
            float newValue = startingValue - (_panelItemNumber * height );

            gameObject.transform.position = new Vector3(0f, 0f, newValue);

        }

    }
}

 