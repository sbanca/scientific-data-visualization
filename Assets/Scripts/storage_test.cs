using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Siccity.GLTFUtility;
using System.IO;                                                        


public class storage_test : MonoBehaviour
{
 

    // Start is called before the first frame update
    IEnumerator  Start()
    {
        
    string filelisturl = "https://europe-west2-gltf-storage.cloudfunctions.net/fileList";
    string fileurl = "https://europe-west2-gltf-storage.cloudfunctions.net/fileUrl";

    CoroutineWithData list = new CoroutineWithData(this, GetTextFromUrl(filelisturl) );
    
    yield return list.coroutine;

    var objectlist = JsonUtility.FromJson<listjson>(list.result.ToString());

    var smallobjectlist = new List<string> {objectlist.list[0],objectlist.list[1]};
    
   
    //file one 

    CoroutineWithData fileDownloadUrl1 = new CoroutineWithData(this, GetTextFromUrl(fileurl,"filename",objectlist.list[0]) );

    yield return fileDownloadUrl1.coroutine;

    Debug.Log("file is => "+objectlist.list[0] + " url is =>" + fileDownloadUrl1.result);

    
    CoroutineWithData fileDownload1 = new CoroutineWithData(this, DownloadFileFromUrl( fileDownloadUrl1.result.ToString() ,objectlist.list[0]) );

    yield return fileDownload1.coroutine;

    Debug.Log("file  => "+objectlist.list[0] + " is stored at =>" + fileDownload1.result);


    
    //file two
    
    CoroutineWithData fileDownloadUrl2 = new CoroutineWithData(this, GetTextFromUrl(fileurl,"filename",objectlist.list[1]) );

    yield return fileDownloadUrl2.coroutine;

    Debug.Log("file is => "+objectlist.list[0] + " url is =>" + fileDownloadUrl2.result);

    
    CoroutineWithData fileDownload2 = new CoroutineWithData(this, DownloadFileFromUrl( fileDownloadUrl2.result.ToString() ,objectlist.list[1]) );

    yield return fileDownload2.coroutine;

    Debug.Log("file  => "+objectlist.list[0] + " is stored at =>" + fileDownload2.result);


    //load 

    
    GameObject gltfObject = Importer.LoadFromFile(fileDownload2.result.ToString()); 
     
   
    }
    
    IEnumerator GetTextFromUrl(string url, string ParameterName = "", string ParameterValue = "") {
       
        if ( ParameterName != "" )   url = url + "?" + ParameterName + "=" +ParameterValue;
        
        Debug.Log("Start Downalod: " + url);

        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();
        
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            yield return www.downloadHandler.text;                       
        }
    }

    IEnumerator DownloadFileFromUrl(string url, string filename) {
        
        Debug.Log("Downalod from: " + url);
        
        string local_file = System.IO.Path.Combine(Application.persistentDataPath,filename);

        Debug.Log("Save to" + local_file);

        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();
        
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {

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

 public class CoroutineWithData {
     public Coroutine coroutine { get; private set; }
     public object result;
     private IEnumerator target;
     public CoroutineWithData(MonoBehaviour owner, IEnumerator target) {
         this.target = target;
         this.coroutine = owner.StartCoroutine(Run());
     }
 
     private IEnumerator Run() {
         while(target.MoveNext()) {
             result = target.Current;
             yield return result;
         }
     }
 }
 /*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Storage;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using Siccity.GLTFUtility;
using System.IO;                                                        // The System.IO namespace contains functions related to loading and saving files


public class storage_test : MonoBehaviour
{
    // Get a reference to the storage service, using the default Firebase App
    FirebaseStorage storage = FirebaseStorage.DefaultInstance;
    StorageReference reference;


    // Start is called before the first frame update
    private async void  Start()
    {
        
        //string url = await DownloadStringAsync("gs://gltf-storage.appspot.com","list.json");

        //StartCoroutine(DownloadFromUrl(url));

        string fileLocation = await DownloadFile("gs://gltf-storage.appspot.com","list.json");

        string dataAsJson = LoadJsonDataFromFile(fileLocation);

        List<string> allfiles = SerializeJsonTolistObject(dataAsJson);

        Debug.Log("list"+allfiles[0]);

        string fileLocationgltf = await DownloadFile("gs://gltf-storage.appspot.com",allfiles[0]);

        string fileLocationbin = await DownloadFile("gs://gltf-storage.appspot.com",allfiles[1]);

        GameObject gltfObject = Siccity.GLTFUtility.Importer.LoadFromFile(fileLocationgltf);


    }
    
    private async Task<string> DownloadStringAsync(string firebaseStorageURL, string filename){

        string downloadUrl ="";

        // Get a non-default Storage bucket
        storage = FirebaseStorage.GetInstance(firebaseStorageURL);
        reference = storage.GetReference("/"+filename);

        // Fetch the download URL
        await reference.GetDownloadUrlAsync().ContinueWith((task) => {
        if (!task.IsFaulted && !task.IsCanceled) {           
            downloadUrl = task.Result.ToString();
            Debug.Log("Download URL: " + downloadUrl );     
        }
        });
        
        return downloadUrl;

    }

    
    private async Task<string> DownloadFile(string firebaseStorageURL, string filename){

        // Get a non-default Storage bucket
        storage = FirebaseStorage.GetInstance(firebaseStorageURL);
        reference = storage.GetReference("/"+filename);

        // Create local filesystem URL
        //string local_file = "file:///local/images/island.jpg";
        string local_file = System.IO.Path.Combine(Application.streamingAssetsPath,filename);

        // Start downloading a file
        Task task = reference.GetFileAsync(local_file,
        new Firebase.Storage.StorageProgress <DownloadState>((DownloadState state) => {
            // called periodically during the download
            Debug.Log(string.Format(
            "Progress: {0} of {1} bytes transferred.",
            state.BytesTransferred,
            state.TotalByteCount
            ));
        }));

        await task.ContinueWith(resultTask => {
        if (!resultTask.IsFaulted && !resultTask.IsCanceled) {
            Debug.Log("Download finished.");
        }
        });
        
        return local_file;

    }

    IEnumerator DownloadFromUrl(string url) {
       
        Debug.Log("Start Downalod: " + url);
        
        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();
        
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            //parse json 
            var myObject = JsonUtility.FromJson<listjson>(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
                        
        }
    }

    private string LoadJsonDataFromFile(string filePath)
    {
        string dataAsJson="";
       
        if(File.Exists(filePath))
        {
            // Read the json from the file into a string
            return File.ReadAllText(filePath);    
            
        }
        else
        {             
            Debug.LogError("Cannot load the file");
            return dataAsJson;
        }
    }

    private List<string> SerializeJsonTolistObject(string dataAsJson){
        
        // Pass the json to JsonUtility, and tell it to create a GameData object from it
        listjson loadedData = JsonUtility.FromJson<listjson>(dataAsJson);
        // Retrieve the allRoundData property of loadedData
        List<string> allfiles = loadedData.list;

        return allfiles;
    }
}


[Serializable]
public class listjson
{
    public List<string> list;
}
*/