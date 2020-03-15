using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class Panel : MonoBehaviour
    {
        
        public Vector3 Pos;

        public Vector2 Size;

        public float thickenss;

        public delegate void OnClick(string name);

        public TasksData panelTasks;

        public GameObject TaskUiItemPrefab;

        public GameObject RouteUiItemPrefab;

        public GameObject MetricsUiItemPrefab;

        public List<GameObject> PanelUiItems;

        private TextMesh Title;

 
        public void Generate()
        {   
            SetTitle();
            Relayout();
            
        }

        public async void Relayout()
        {

            //clean the panel

            DeletePanelsItems();


            //update route calculation and options update

            await panelTasks.Update();


            //createUiItems

            PanelUiItems = new List<GameObject>();

            foreach (UiItem item in panelTasks.UiItemList) InstantiateUIitem(item);


            if (panelTasks.Type == PanelType.TASKASSEMBLYPANNEL && Application.isPlaying)
            {
  
                // trigger options for last panel item 
                if (panelTasks.List.Count > 0)
                {
                    PanelUiItems[PanelUiItems.Count - 1].GetComponent<TaskUiItemManager>().taskOptionClicked.AddListener(SelectedTaskOption);
                    PanelUiItems[PanelUiItems.Count - 1].GetComponent<TaskUiItemManager>().TriggerOptions();
                }


                //trigger display of routes
                panelTasks.DisplayRoutes();


            }

      
        }

        public void InstantiateUIitem(UiItem item )
        {
            int indexaddition = panelTasks.Type == PanelType.TASKASSEMBLYPANNEL ? 1 : 0; //this is an excamotage for the layout;

            switch (item.type) {

                case (UiItemType.TASK):

                    PanelUiItems.Add(InstantiateTaskUIitem(item.taskData,item.itemNumber + indexaddition)); 

                    break;

                case (UiItemType.ROUTE):

                    PanelUiItems.Add(InstantiateRouteUIitem(item.routeData,item.itemNumber + indexaddition));

                    break;

                case (UiItemType.METRICS):

                    PanelUiItems.Add(InstantiateResultsUIitem(item.metricsData, item.itemNumber));

                    break;
            }
        }

        public GameObject InstantiateTaskUIitem(TaskData task,int i) {


            //instantiate panel task prefab

            GameObject NewPanelTask = Instantiate(TaskUiItemPrefab);          


            //set same position and rotation as the parent panel

            NewPanelTask.transform.position = this.gameObject.transform.position;

            NewPanelTask.transform.rotation = this.gameObject.transform.rotation;

            NewPanelTask.transform.parent = this.gameObject.transform;


            //get panel item manager and update the item details 

            TaskUiItemManager Manager = TaskUiItemManager.CreateComponent(NewPanelTask, task,i);


            return NewPanelTask;

        }

        public GameObject InstantiateRouteUIitem(RouteData routeData,int i)
        {

            //instantiate panel task prefab

            GameObject RouteUIItem = Instantiate(RouteUiItemPrefab);


            //set same position and rotation as the parent panel

            RouteUIItem.transform.position = this.gameObject.transform.position;

            RouteUIItem.transform.rotation = this.gameObject.transform.rotation;

            RouteUIItem.transform.parent = this.gameObject.transform;


            //get panel item manager and update the item details 

            RouteUiItemManager Manager = RouteUiItemManager.CreateComponent(RouteUIItem, routeData, i);


            return RouteUIItem;

        }

        public GameObject InstantiateResultsUIitem(MetricsData metricsData, int i)
        {

            //instantiate panel task prefab

            GameObject MetricsUIItem = Instantiate(MetricsUiItemPrefab);


            //set same position and rotation as the parent panel

            MetricsUIItem.transform.position = this.gameObject.transform.position;

            MetricsUIItem.transform.rotation = this.gameObject.transform.rotation;

            MetricsUIItem.transform.parent = this.gameObject.transform;


            //get panel item manager and update the item details 

            MetricsUiItemManager Manager = MetricsUiItemManager.CreateComponent(MetricsUIItem, metricsData, i);


            return MetricsUIItem;

        }

        public void SetTitle() {

            Title = gameObject.GetComponentInChildren<TextMesh>();
            Title.text = panelTasks.Title;
        }

        public void DeletePanelsItems() {

            if (PanelUiItems == null) return;

            foreach (GameObject g in PanelUiItems) {


#if UNITY_EDITOR


                DestroyImmediate(g);

#else

            Destroy(g);
#endif

            }

            PanelUiItems = null;

        }

        public void RemoveTask(TaskData extractedTask) {

            string name = extractedTask.Name;

            for (int i =0; i< panelTasks.List.Count; i++) {

                if (panelTasks.List[i].Name == name) {

                    panelTasks.List.Remove(panelTasks.List[i]);

                    break;
                }

            }

        }

        private void SelectedTaskOption(string optiontask, string optionName) {

            for (int j = 0; j < panelTasks.List.Count; j++)
            {

                if (panelTasks.List[j].Name == optiontask) {

                    TaskData taskData = panelTasks.List[j];

                    for (int i = 0; i < taskData.Options.Count; i++)
                    {

                        OptionData o = taskData.Options[i];

                        if (o.Name == optionName)
                        {
                            o.Selected = true;
                            panelTasks.List[j].SelectedOption = i;
                        }
                        else o.Selected = false;

                    }
                }
            }

            Relayout();
        }

        public void AddTask(TaskData task) {


            panelTasks.List.Add(task); //add the task


        }

        public TaskData GetTask(string name) {
            
            for (int i = 0; i < panelTasks.List.Count; i++)
            {
                if (panelTasks.List[i].Name == name) return panelTasks.List[i];

            }

            return null;

        }

        public TaskData ExtractTask(string name) {

            TaskData extractedTask = GetTask(name);

            if (extractedTask != null) RemoveTask(extractedTask);

            return extractedTask;
        }


    }
}