
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

        public float startingValue = 0.118f;

        public float height = 0.05f ;

        private TextMesh title;

        private TextMesh time;

        public Options options;

        public TaskOptionClicked taskOptionClicked;

        private void setTitle()
        {
            if (title == null) title = getTextMeshChildByName("Title");

            title.text = _pannelTask.Name;

            this.gameObject.name = _pannelTask.Name;

        }

        private TextMesh getTextMeshChildByName(string name) {
        
            TextMesh[] TextMeshes = this.gameObject.GetComponentsInChildren<TextMesh>();

            TextMesh textMesh = null;

            foreach (TextMesh tm in TextMeshes) {

                if (tm.name == name) { 

                    textMesh = tm;

                    break;
                }

            }

            return textMesh;
        
        }

        private void setPos()
        {
            float newValue = startingValue - (_panelItemNumber * height );

            this.gameObject.transform.localPosition = new Vector3(0f, 0f, newValue);

        }

        public void TriggerOptions() {

            if (options == null) getOptions();

            if (_pannelTask.Options.Count == 1) return;

            options.OptionList = _pannelTask.Options.ToArray();

            options.Generate();

            options.optionClicked.AddListener(SelectOption);

        }

        public void SelectOption(string name) {

            taskOptionClicked.Invoke(pannelTask.Name, name);
        
        }
        
        private void getOptions() {

            options = gameObject.GetComponent<Options>();

        }

        public void setTime(int timeInSeconds) {

            int minutes = timeInSeconds % 60;

            int hours = minutes % 60;

            if (time == null) time = getTextMeshChildByName("Time");

            time.text = hours +":"+ minutes;

        }

    }
}

 