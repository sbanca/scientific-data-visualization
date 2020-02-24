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

        public void AddLabel()
        {

        }




    }
}