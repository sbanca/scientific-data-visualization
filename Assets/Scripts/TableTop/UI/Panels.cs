using UnityEngine;
using System.Collections.Generic;

namespace TableTop{
    public class Panels : Singleton<Panels>
    {

        public string JsonName = "data.json";

        private TasksDataGroups panelsData;

        public GameObject PanelPrefab;

        private GameObject PanelsParent;

        public GameObject[] panelsGameObjects;



        public void Start()
        {

            if(panelsData== null) GetPanelList();

            if (PanelsParent == null) CreatePanelsParent();

#if UNITY_EDITOR

            if (PanelPrefab == null) GetPanelPrefabFromResources();

#endif

         
        }
        
        private void GetPanelList()
        {
 
            string panelsText = LoadResourceTextfile(JsonName);

            panelsData = JsonUtility.FromJson<TasksDataGroups>(panelsText);

        }

        private void  GetPanelPrefabFromResources() {

            PanelPrefab = Resources.Load<GameObject>("Prefabs/Panel");

        }
        
        public void GeneratePanels()
        {

#if UNITY_EDITOR

            if (panelsData == null || panelsData.List.Count<1 ) GetPanelList();

            if (PanelsParent == null) CreatePanelsParent();

            if (PanelPrefab == null) GetPanelPrefabFromResources();
#endif

            panelsGameObjects = new GameObject[panelsData.List.Count];

            for (int i=0; i<panelsData.List.Count;i++) {

                TasksData t = panelsData.List[i];

                //panel object
                var newPanelGameObject = Instantiate(PanelPrefab);

                newPanelGameObject.name = t.Title;

                //set parent 
                newPanelGameObject.transform.parent = PanelsParent.transform;

                //rotation
                newPanelGameObject.transform.eulerAngles = new Vector3(t.Rotation[0], t.Rotation[1], t.Rotation[2]);

                //position
                newPanelGameObject.transform.position = new Vector3(t.Position[0], t.Position[1], t.Position[2]);

                //scale
                Vector3 scale = newPanelGameObject.transform.localScale;

                scale.Set(t.Scale[0], t.Scale[1], t.Scale[2]);

                newPanelGameObject.transform.transform.localScale = scale;


                //panel manager
                
                Panel newPanelManager = newPanelGameObject.GetComponentInChildren<Panel>();

                newPanelManager.panelTasks = t;

                newPanelManager.Generate();

                panelsGameObjects[i] = newPanelGameObject;

            }
        }

        public void DeleteAll()
        {
            DeleteData();
            DeletePanels();

        }
        
        private void DeleteData() {
            if(panelsData != null) panelsData = null;
        }

        public static string LoadResourceTextfile(string name)
        {

            string filePath = "SetupData/" + name.Replace(".json", "");

            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            return targetFile.text;
        }

        public void DeletePanels()
        {
            if (PanelsParent == null) GetParent();

            if (PanelsParent == null) return;

            if (panelsGameObjects.Length < 1) getChildFromParent();

            if (panelsGameObjects.Length > 0)
            {

                foreach (GameObject go in panelsGameObjects)
                {

#if UNITY_EDITOR
                    DestroyImmediate(go);
#else
                Destroy(go);
#endif

                }
            }

            panelsGameObjects = null;

#if UNITY_EDITOR
            DestroyImmediate(PanelsParent);
#else
                Destroy(PanelsParent);
#endif

        }

        private void getChildFromParent()
        {

            PanelsParent = GameObject.Find("Panels");

            if (PanelsParent == null) return;

            int childCount = PanelsParent.transform.childCount;

            if (childCount < 1) return;

            GameObject child;

            panelsGameObjects = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {

                child = PanelsParent.transform.GetChild(i).gameObject;

            }

        }

        private void GetParent()
        {

            PanelsParent = GameObject.Find("Panels");

        }

        private void CreatePanelsParent() {

            PanelsParent = GameObject.Find("Panels");

            if (PanelsParent != null) return;

            PanelsParent =  new GameObject();

            PanelsParent.name = "Panels";

        }

        

    }
}

