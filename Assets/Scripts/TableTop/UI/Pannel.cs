using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class Pannel : MonoBehaviour
    {
        
        public Vector3 Pos;

        public Vector2 Size;

        public float thickenss;

        public delegate void OnClick(string name);

        public PannelTasks pannelTasks;

        public GameObject PannelItemPrefab;
        
        public GameObject[] PannelItems;

        private TextMesh Title;

        public void Generate()
        {   
            SetTitle();
            Relayout();
            
        }

        public void Relayout()
        {

            DeletePannelsItems();

            GameObject NewPannelItem;

            PannelItem Manager;

            PannelItems = new GameObject[pannelTasks.List.Count];

            for (int i =0; i< pannelTasks.List.Count; i++) {

                NewPannelItem = Instantiate(PannelItemPrefab);


                //set same position and rotation as the parent pannel

                NewPannelItem.transform.position = this.gameObject.transform.position;

                NewPannelItem.transform.rotation = this.gameObject.transform.rotation;

                NewPannelItem.transform.parent = this.gameObject.transform;


                //get pannel item manager and update the item details 

                Manager = NewPannelItem.GetComponent<PannelItem>();

                Manager.panelItemNumber =i;

                Manager.pannelTask = pannelTasks.List[i];          

                PannelItems[i] = NewPannelItem;

            }

            if (pannelTasks.Type == PanelType.TASKASSEMBLYPANNEL)
            {

                //if there is more than one item trigger options for last task item 
                if (pannelTasks.List.Count > 0) 
                {
                    PannelItems[pannelTasks.List.Count - 1].GetComponent<PannelItem>().TriggerOptions();
                }

                //trigger route calculations
                TaskCalculation.Instance.CalculateTask(pannelTasks);

            }
        
        }

        public void SetTitle() {

            Title = gameObject.GetComponentInChildren<TextMesh>();
            Title.text = pannelTasks.Title;
        }

        public void DeletePannelsItems() {

            if (PannelItems == null) return;

            foreach (GameObject g in PannelItems) {


#if UNITY_EDITOR


                DestroyImmediate(g);

#else

            Destroy(g);
#endif

            }

            PannelItems = null;

        }

        public void RemoveTask(string name) {

            for (int i =0; i< pannelTasks.List.Count; i++) {

                if (pannelTasks.List[i].Name == name) {

                    pannelTasks.List.Remove(pannelTasks.List[i]);

                    break;
                }

            }


            if (pannelTasks.Type == PanelType.TASKASSEMBLYPANNEL)
            {
                Routes.Instance.DeleteRoutesContainingTaskName(name);
            }
        }

        public void AddTask(PannelTask task) {

            pannelTasks.List.Add(task);
        }

        public PannelTask GetTask(string name) {
            
            for (int i = 0; i < pannelTasks.List.Count; i++)
            {
                if (pannelTasks.List[i].Name == name) return pannelTasks.List[i];

            }

            return null;

        }

        public PannelTask ExtractTask(string name) {

            PannelTask extractedTask = GetTask(name);

            if (extractedTask != null) RemoveTask(name);

            return extractedTask;
        }
    

    }
}