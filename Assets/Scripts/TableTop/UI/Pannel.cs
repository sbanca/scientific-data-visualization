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

        public void Generate()
        {
            Relayout();

        }
        public void Relayout()
        {
            for (int i =0; i< pannelTasks.List.Count; i++) {

                var NewPannelItem = Instantiate(PannelItemPrefab);

                PannelItem Manager = PannelItemPrefab.GetComponent<PannelItem>();

                Manager.panelItemNumber =i;

                Manager.pannelTask = pannelTasks.List[i];              

                NewPannelItem.transform.parent = this.gameObject.transform;

                PannelItems.Add(NewPannelItem);
            }

        }


        public void AddLabel()
        {

        }




    }
}