using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TableTop
{
    // Class declaration
    [System.Serializable]
    public class OptionClicked : UnityEvent<string> { }
    

    public class Option : MonoBehaviour
    {

        private OptionItem _optionData;

        public OptionItem optionData {

            get { return _optionData; }

            set {

                _optionData = value;

                setTitle();

                setPos();

                intializeTick();
                    }
        }

        public GameObject selected;

        public TextMesh text;

        private float offset =  -0.058f;

        public OptionClicked optionClicked;

        private void intializeTick() {

            if (_optionData.Selected) Tick();
            else UnTick();

        }

        public void Tick() { selected.SetActive(true); }

        public void UnTick() { selected.SetActive(false);  }

        private void setTitle() {  text.text = Truncate(_optionData.Name,15) ; }

        private void setPos()
        {
            Vector3 pos = new Vector3(0f, 0f, offset * ( _optionData.number + 1) );

            this.gameObject.transform.Translate(pos, Space.Self);
        }

        public void OnMouseDown()
        {
            selected.SetActive(!selected.activeSelf);

            optionClicked.Invoke(name);

        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);

        }

    }
}