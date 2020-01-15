
namespace FireStorage {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;
    using System;
    using Siccity.GLTFUtility;
    using System.IO;

    public class FireManager : MonoBehaviour
    {

        public static FireManager instance;

        [Tooltip("The Url to get the file list")]
        public string filelisturl = "https://europe-west2-gltf-storage.cloudfunctions.net/fileList";

        [Tooltip("The Url to download a file from the file list")]
        public string fileurl = "https://europe-west2-gltf-storage.cloudfunctions.net/fileUrl";

        public List<string> objectlist = null;        

        // Start is called before the first frame update
        void Awake()
        {
            instance = this;

        }

        public  IEnumerator GetFileList() {

            CoroutineWithData list = new CoroutineWithData(this, GetTextFromUrl(filelisturl));

            yield return list.coroutine;

            listjson jsonObjectlist = JsonUtility.FromJson<listjson>(list.result.ToString());

            objectlist = jsonObjectlist.list;

            Debug.Log("[FireManager] " + "Files List Downloaded" + string.Join("", objectlist.ToArray() ));

        }

        public  IEnumerator DownloadFile(string filename)
        {

            Debug.Log("[FireManager] DownloadFile ");

            // get url 

            CoroutineWithData fileDownloadUrl = new CoroutineWithData(this, GetTextFromUrl(fileurl, "filename", filename));

            yield return fileDownloadUrl.coroutine;

            Debug.Log("[FireManager] " + " file is => " + filename + " url is =>" + fileDownloadUrl.result);


            // download file 

            CoroutineWithData fileDownload = new CoroutineWithData(this, DownloadFileFromUrl(fileDownloadUrl.result.ToString(), filename));

            yield return fileDownload.coroutine;

            Debug.Log("[FireManager] " + "file  => " + filename + " is stored at =>" + fileDownload.result);

            GameObject gltfObject = Importer.LoadFromFile(fileDownload.result.ToString());

        }

        private IEnumerator GetTextFromUrl(string url, string ParameterName = "", string ParameterValue = "")
        {

            if (ParameterName != "") url = url + "?" + ParameterName + "=" + ParameterValue;

            Debug.Log("[FireManager] " + "Start Downalod: " + url);

            UnityWebRequest www = new UnityWebRequest(url);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                yield return www.downloadHandler.text;
            }
        }

        private IEnumerator DownloadFileFromUrl(string url, string filename)
        {

            Debug.Log("[FireManager] "+"Downalod from: " + url);

            string local_file = System.IO.Path.Combine(Application.persistentDataPath, filename);

            Debug.Log("[FireManager] " + "Save to" + local_file);

            UnityWebRequest www = new UnityWebRequest(url);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("[FireManager] " + www.error);
            }
            else
            {

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;

                File.WriteAllBytes(local_file, results);

                yield return local_file;

            }
        }

    }

    [Serializable]
    public class listjson
    {
        public List<string> list;
    }

    public class CoroutineWithData
    {
        public Coroutine coroutine { get; private set; }
        public object result;
        private IEnumerator target;
        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                result = target.Current;
                yield return result;
            }
        }
    }


}

