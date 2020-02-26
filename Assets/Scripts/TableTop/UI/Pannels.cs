using UnityEngine;
using System.Collections.Generic;

namespace TableTop{
    public class Pannels : Singleton<Pannels>
    {

        public string JsonName = "Pannels.json";

        private PannelsList pannelsData;

        public GameObject PannelPrefab;

        private GameObject PannelsParent;

        public List<GameObject> pannelsGameObjects = new List<GameObject>();

        public void Start()
        {

            if(pannelsData== null) GetPannelList();

            if (PannelsParent == null) CreatePannelsParent();



#if UNITY_EDITOR

            if (PannelPrefab == null) GetPannelPrefabFromResources();

#endif

        }
        
        private void GetPannelList()
        {
 
            string pannelstext = LoadResourceTextfile(JsonName);

            pannelsData = JsonUtility.FromJson<PannelsList>(pannelstext);

        }

        private void  GetPannelPrefabFromResources() {

            string filePath = "Prefab/Pannel_prefab";

            PannelPrefab = Resources.Load<GameObject>(filePath);

        }
        
        public void GeneratePanels()
        {

#if UNITY_EDITOR

            if (pannelsData == null) GetPannelList();

            if (PannelsParent == null) CreatePannelsParent();
#endif

            foreach (PannelTasks t in pannelsData.List) {

                //pannel object
                var newPannelGameObject = Instantiate(PannelPrefab);

                newPannelGameObject.name = t.Title;

                //set parent 
                newPannelGameObject.transform.parent = PannelsParent.transform;

                //rotation
                newPannelGameObject.transform.eulerAngles = new Vector3(t.Rotation[0], t.Rotation[1], t.Rotation[2]);

                //position
                newPannelGameObject.transform.position = new Vector3(t.Position[0], t.Position[1], t.Position[2]);

                //scale
                Vector3 scale = newPannelGameObject.transform.localScale;

                scale.Set(t.Scale[0], t.Scale[1], t.Scale[2]);

                newPannelGameObject.transform.transform.localScale = scale;


                //pannel manager
                
                Pannel newPanelManager = newPannelGameObject.GetComponentInChildren<Pannel>();

                newPanelManager.pannelTasks = t;

                newPanelManager.Generate();

                pannelsGameObjects.Add(newPannelGameObject);

            }
        }

        public void DeleteAll()
        {
            DeleteData();
            DeletePannels();

        }
        
        private void DeleteData() {
            if(pannelsData != null) pannelsData = null;
        }

        public static string LoadResourceTextfile(string name)
        {

            string filePath = "SetupData/" + name.Replace(".json", "");

            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            return targetFile.text;
        }

        public void DeletePannels()
        {

            foreach (GameObject go in pannelsGameObjects)
            {
             
#if UNITY_EDITOR
                DestroyImmediate(go);
#else
                Destroy(go);
#endif

            }

        }

        private void CreatePannelsParent() {

            PannelsParent =  new GameObject();

            PannelsParent.name = "Pannels";

        }
    }
}

