using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Siccity.GLTFUtility;
using System.IO;


public class SingleFileDD : MonoBehaviour
{


    // Start is called before the first frame update
    IEnumerator Start()
    {

       
        string fileurl = "https://europe-west2-gltf-storage.cloudfunctions.net/fileUrl";


        //file two

        CoroutineWithData fileDownloadUrl2 = new CoroutineWithData(this, GetTextFromUrl(fileurl, "filename", "terrain.glb"));

        yield return fileDownloadUrl2.coroutine;

        Debug.Log("file is => " + "Imperial College London.glb" + " url is =>" + fileDownloadUrl2.result);


        CoroutineWithData fileDownload2 = new CoroutineWithData(this, DownloadFileFromUrl(fileDownloadUrl2.result.ToString(), "terrain.glb"));

        yield return fileDownload2.coroutine;

        Debug.Log("file  => " + "Imperial College London.glb" + " is stored at =>" + fileDownload2.result);


        //load 


        GameObject gltfObject = Siccity.GLTFUtility.Importer.LoadFromFile(fileDownload2.result.ToString());


    }

    IEnumerator GetTextFromUrl(string url, string ParameterName = "", string ParameterValue = "")
    {

        if (ParameterName != "") url = url + "?" + ParameterName + "=" + ParameterValue;

        Debug.Log("Start Downalod: " + url);

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

    IEnumerator DownloadFileFromUrl(string url, string filename)
    {

        Debug.Log("Downalod from: " + url);

        string local_file = System.IO.Path.Combine(Application.persistentDataPath, filename);

        Debug.Log("Save to" + local_file);

        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
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