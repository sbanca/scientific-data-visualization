﻿using System.Collections.Generic;
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

        private TaskCalculation task;


        public void Generate()
        {   
            SetTitle();
            Relayout();
            
        }

        public void Relayout()
        {

            //clean the pannel

            DeletePannelsItems();


            //make sure selection is initialize correctly

            pannelTasks.InitializeSelections();


            //intialize variables 

            GameObject NewPannelItem;

            PannelItem Manager;

            PannelItems = new GameObject[pannelTasks.List.Count];


            //iterate

            for (int i =0; i< pannelTasks.List.Count; i++) {


                NewPannelItem = Instantiate(PannelItemPrefab);

                if(pannelTasks.List[i].Draggable) NewPannelItem.AddComponent<Draggable>();


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
                    PannelItems[pannelTasks.List.Count - 1].GetComponent<PannelItem>().taskOptionClicked.AddListener(SelectedTaskOption);
                    PannelItems[pannelTasks.List.Count - 1].GetComponent<PannelItem>().TriggerOptions();
                }


                //trigger route calculations
                if (Application.isPlaying) TaskCalculation.Instance.CalculateTask(pannelTasks);

            }

            //last bit
            UpdateTime();
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

        public void UpdateTime() {

            for (int i = 0; i < PannelItems.Length; i++)
            {

                OptionItem option= pannelTasks.List[i].returnSelectedOption();

                PannelItem PannelItemManager = PannelItems[i].GetComponent<PannelItem>();               

                PannelItemManager.setTime(pannelTasks.List[i].Duration);

            }

        }

    }
}