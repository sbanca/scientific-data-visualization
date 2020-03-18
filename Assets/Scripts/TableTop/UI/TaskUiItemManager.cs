
using UnityEngine;

namespace TableTop
{
    public class TaskUiItemManager : UiItemManager
    {

        private TaskData _panelTask;
        public TaskData taskData
        {

            get { return _panelTask; }
            set
            {
                _panelTask = value;

                setValue("Title", _panelTask.Name);

                if (_panelTask.TimeLocked)
                {
                    string time = transformSecondsToTime(_panelTask.TimeInSeconds);

                    if (_panelTask.TimeDifferenceInSeconds > 0) time += "  ( dealy  " + transformSecondsToTime(_panelTask.TimeDifferenceInSeconds) + " )";

                    else if ((_panelTask.TimeDifferenceInSeconds < 0)) time += "  ( early  " + transformSecondsToTime(_panelTask.TimeDifferenceInSeconds) + " )";

                    setValue("Time", time);

                }
                else setValue("Time", "");
                
            }

        }

        public Options options;

        public TaskOptionClicked taskOptionClicked;


        //static constructor

        public static TaskUiItemManager CreateComponent(GameObject where, TaskData taskData, int routeItemNumber)
        {

            where.name = taskData.Name;

            TaskUiItemManager taskUiItemManagerObject = where.AddComponent<TaskUiItemManager>();

            if (taskData.Clickable) where.AddComponent<Clickable>();

            taskUiItemManagerObject.itemNumber = routeItemNumber;

            taskUiItemManagerObject.taskData = taskData;

            taskUiItemManagerObject.taskOptionClicked = new TaskOptionClicked();


            return taskUiItemManagerObject;

        }

        public void TriggerOptions() {

            if (options == null) getOptions();

            if (_panelTask.Options.Count == 1) return;

            options.OptionList = _panelTask.Options.ToArray();

            options.Generate();

            options.optionClicked.AddListener(SelectOption);

        }

        public void SelectOption(string name) {

            taskOptionClicked.Invoke(taskData.Name, name);
        
        }
        
        private void getOptions() {

            options = gameObject.GetComponent<Options>();

        }


    }
}

 