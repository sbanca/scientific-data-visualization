using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TableTop
{

    [RequireComponent(typeof(BoxCollider))]
    public class Clickable : MonoBehaviour
    {
        private static bool locked = false;

        private string taskName;

        private void Start()
        {
   
            taskName = this.gameObject.transform.name;

        }

        void OnMouseDown()
        {

            Clicked();

        }

        private void OnTriggerEnter(Collider other)
        {

            Clicked();

        }

        public void Clicked() {



            Panel originPanel;

            Panel targetPanel;

            (originPanel, targetPanel) = OriginAndTarget();

            Exchange(originPanel, targetPanel, taskName);

        }

        public (Panel,Panel) OriginAndTarget() {


            Panel originPanel = transform.parent.gameObject.GetComponent<Panel>();

            Panel targetPanel;


            if (originPanel.panelTasks.Type == PanelType.TASKASSEMBLYPANNEL)
            {

                string targetPanelName = originPanel.panelTasks.GetTask(taskName).OriginName;

                targetPanel = GameObject.Find(targetPanelName).GetComponent<Panel>();

            }
            else
            {

                targetPanel = GameObject.Find("Task Construction").GetComponent<Panel>();

            }

            return (originPanel,targetPanel);
        }

        private static async void Exchange(Panel origin, Panel target, string taskName)
        {

            if (!locked) {

                locked = true;

                if (origin == target) return;

                TaskData taskData = origin.panelTasks.ExtractTask(taskName);

                target.panelTasks.AddTask(taskData);

                origin.Relayout();

                target.Relayout();

                await Task.Delay(TimeSpan.FromSeconds(1));

                locked = false;
            
            }

        }


    }
}