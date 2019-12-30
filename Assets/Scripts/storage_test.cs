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