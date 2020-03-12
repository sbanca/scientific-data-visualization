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

        public GameObject TaskUiItemPrefab;

        public GameObject RouteUiItemPrefab;

        public List<GameObject> PannelUiItems;

        private TextMesh Title;

 
        public void Generate()
        {   
            SetTitle();
            Relayout();
            
        }

        public async void Relayout()
        {

            //clean the pannel

            DeletePannelsItems();


            //update route calculation and options update

            await pannelTasks.Update();


            //createUiItems

            PannelUiItems = new List<GameObject>();

            foreach (UiItem item in pannelTasks.UiItemList) InstantiateUIitem(item);


            if (pannelTasks.Type == PanelType.TASKASSEMBLYPANNEL && Application.isPlaying)
            {
  
                // trigger options for last pannel item 
                if (pannelTasks.List.Count > 0)
                {
                    PannelUiItems[PannelUiItems.Count - 1].GetComponent<TaskUiItemManager>().taskOptionClicked.AddListener(SelectedTaskOption);
                    PannelUiItems[PannelUiItems.Count - 1].GetComponent<TaskUiItemManager>().TriggerOptions();
                }


                //trigger display of routes
                pannelTasks.DisplayRoutes();


            }

      
        }

        public void InstantiateUIitem(UiItem item )
        {
            switch (item.type) {

                case (UiItemType.TASK):

                    PannelUiItems.Add(InstantiateTaskUIitem(item.taskData,item.itemNumber));

                    break;

                case (UiItemType.ROUTE):

                    PannelUiItems.Add(InstantiateRouteUIitem(item.routeData,item.itemNumber));

                    break;
            }
        }

        public GameObject InstantiateTaskUIitem(PannelTask task,int i) {


            //instantiate pannel task prefab

            GameObject NewPannelTask = Instantiate(TaskUiItemPrefab);          


            //set same position and rotation as the parent pannel

            NewPannelTask.transform.position = this.gameObject.transform.position;

            NewPannelTask.transform.rotation = this.gameObject.transform.rotation;

            NewPannelTask.transform.parent = this.gameObject.transform;


            //get pannel item manager and update the item details 

            TaskUiItemManager Manager = TaskUiItemManager.CreateComponent(NewPannelTask, task,i);


            return NewPannelTask;

        }

        public GameObject InstantiateRouteUIitem(RouteData routeData,int i)
        {

            //instantiate pannel task prefab

            GameObject RouteUIItem = Instantiate(RouteUiItemPrefab);


            //set same position and rotation as the parent pannel

            RouteUIItem.transform.position = this.gameObject.transform.position;

            RouteUIItem.transform.rotation = this.gameObject.transform.rotation;

            RouteUIItem.transform.parent = this.gameObject.transform;


            //get pannel item manager and update the item details 

            RouteUiItemManager Manager = RouteUiItemManager.CreateComponent(RouteUIItem, routeData, i);


            return RouteUIItem;

        }


        public void SetTitle() {

            Title = gameObject.GetComponentInChildren<TextMesh>();
            Title.text = pannelTasks.Title;
        }

        public void DeletePannelsItems() {

            if (PannelUiItems == null) return;

            foreach (GameObject g in PannelUiItems) {


#if UNITY_EDITOR


                DestroyImmediate(g);

#else

            Destroy(g);
#endif

            }

            PannelUiItems = null;

        }

        public void RemoveTask(PannelTask extractedTask) {

            string name = extractedTask.Name;

            for (int i =0; i< pannelTasks.List.Count; i++) {

                if (pannelTasks.List[i].Name == name) {

                    pannelTasks.List.Remove(pannelTasks.List[i]);

                    break;
                }

            }

        }

        private void SelectedTaskOption(string optiontask, string optionName) {

            for (int j = 0; j < pannelTasks.List.Count; j++)
            {

                if (pannelTasks.List[j].Name == optiontask) {

                    PannelTask pannelTask = pannelTasks.List[j];

                    for (int i = 0; i < pannelTask.Options.Count; i++)
                    {

                        OptionItem o = pannelTask.Options[i];

                        if (o.Name == optionName)
                        {
                            o.Selected = true;
                            pannelTasks.List[j].SelectedOption = i;
                        }
                        else o.Selected = false;

                    }
                }
            }

            Relayout();
        }

        public void AddTask(PannelTask task) {


            pannelTasks.List.Add(task); //add the task


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

            if (extractedTask != null) RemoveTask(extractedTask);

            return extractedTask;
        }


    }
}