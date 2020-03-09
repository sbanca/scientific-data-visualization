using UnityEngine;
using System.Collections.Generic;

namespace TableTop{
    public class Pannels : Singleton<Pannels>
    {

        public string JsonName = "Pannels.json";

        private PannelsList pannelsData;

        public GameObject PannelPrefab;

        private GameObject PannelsParent;

        public GameObject[] pannelsGameObjects;

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

            PannelPrefab = Resources.Load<GameObject>("Prefabs/Pannel_prefab");

        }
        
        public void GeneratePanels()
        {

#if UNITY_EDITOR

            if (pannelsData == null || pannelsData.List.Count<1 ) GetPannelList();

            if (PannelsParent == null) CreatePannelsParent();

            if (PannelPrefab == null) GetPannelPrefabFromResources();
#endif

            pannelsGameObjects = new GameObject[pannelsData.List.Count];

            for (int i=0; i<pannelsData.List.Count;i++) {

                PannelTasks t = pannelsData.List[i];

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

                pannelsGameObjects[i]=newPannelGameObject ;

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
            if (PannelsParent == null) GetParent();

            if (PannelsParent == null) return;

            if (pannelsGameObjects.Length < 1) getChildFromParent();

            if (pannelsGameObjects.Length > 0)
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

            pannelsGameObjects = null;

#if UNITY_EDITOR
            DestroyImmediate(PannelsParent);
#else
                Destroy(PannelsParent);
#endif

        }

        private void getChildFromParent()
        {

            PannelsParent = GameObject.Find("Pannels");

            if (PannelsParent == null) return;

            int childCount = PannelsParent.transform.childCount;

            if (childCount < 1) return;

            GameObject child;

            pannelsGameObjects = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {

                child = PannelsParent.transform.GetChild(i).gameObject;

            }

        }

        private void GetParent()
        {

            PannelsParent = GameObject.Find("Pannels");

        }

        private void CreatePannelsParent() {

            PannelsParent = GameObject.Find("Pannels");

            if (PannelsParent != null) return;

            PannelsParent =  new GameObject();

            PannelsParent.name = "Pannels";

        }

        

    }
}

