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
        
        public List<GameObject> PannelItems ;

        private TextMesh Title;

        public void Generate()
        {   
            SetTitle();
            Relayout();
            
        }

        public void Relayout()
        {

            DeletePannelsItems();

            for (int i =0; i< pannelTasks.List.Count; i++) {

                var NewPannelItem = Instantiate(PannelItemPrefab);


                //set same position and rotation as the aprent pannel

                NewPannelItem.transform.position = this.gameObject.transform.position;

                NewPannelItem.transform.rotation = this.gameObject.transform.rotation;

                NewPannelItem.transform.parent = this.gameObject.transform;


                //get pannel item manager and update the item details 

                PannelItem Manager = NewPannelItem.GetComponent<PannelItem>();

                Manager.panelItemNumber =i;

                Manager.pannelTask = pannelTasks.List[i];                    

                PannelItems.Add(NewPannelItem);
            }

        }

        public void SetTitle() {

            Title = gameObject.GetComponentInChildren<TextMesh>();
            Title.text = pannelTasks.Title;
        }

        public void DeletePannelsItems() {

            foreach (GameObject g in PannelItems) {

                Destroy(g);

            }

        }

        public void RemoveTask(string name) {

            for (int i =0; i< pannelTasks.List.Count; i++) {

                if (pannelTasks.List[i].Name == name) {

                    pannelTasks.List.Remove(pannelTasks.List[i]);

                    break;
                }

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