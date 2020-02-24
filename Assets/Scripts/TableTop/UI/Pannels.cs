using UnityEngine;
using System.Collections.Generic;

namespace TableTop{
    public class Pannels : Singleton<Pannels>
    {

        public string JsonName = "Pannels.json";

        private PannelsList pannelsData;

        public GameObject PannelPrefab;

        private List<GameObject> pannelsGameObjects = new List<GameObject>();

        public void Start()
        {
            GetPannelList();

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
#endif

            foreach (PannelTasks t in pannelsData.List) {

                var newPannelGameObject = Instantiate(PannelPrefab);

                Pannel newPanelManager = newPannelGameObject.GetComponentInChildren<Pannel>();

                newPanelManager.pannelTasks = t;

                newPanelManager.Generate();

                pannelsGameObjects.Add(newPannelGameObject);

            }
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

                Destroy(go);

            }
        }
    }
}

