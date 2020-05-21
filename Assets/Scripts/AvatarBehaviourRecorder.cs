using System.IO;
using UnityEngine;
using TableTop;
using System.Collections;
using System;

public class AvatarBehaviourRecorder : MonoBehaviour
{
    StreamWriter writer;

    string line;

    Vector3 LocalHeadAbsolutePosition = Vector3.zero;

    Vector3 RemoteHeadAbsolutePosition = Vector3.zero;

    Vector3 ControllerAbsolutePosition = Vector3.zero;

    Vector3 RemoteControllerAbsolutePosition = Vector3.zero;

    Vector3 Pointer1 = Vector3.zero;

    Vector3 Pointer2 = Vector3.zero;

    Transform LocalHead ;

    Transform RemoteHead;

    Transform Controller;

    Transform RemoteController;

    inputsManager inputsmanagerinstance;

    Augmentations ainst;

    public int frameRate = 25;

    float startTime;
    
    float currentTime;

    Char[] remove = new Char[] { ' ', '(', ')' };

    [SerializeField]
    public string folderName = "Data";

    private void OnEnable()
    {
        GameObject g = new GameObject();
        LocalHead = g.transform;
        RemoteHead = g.transform;
        Controller = g.transform;
        RemoteController = g.transform;

        Time.captureFramerate = frameRate;

    }

    void Update()
    {
        if (writer == null) return;

        if (inputsmanagerinstance.LocalHead != null) LocalHead = inputsmanagerinstance.LocalHead;
        if (inputsmanagerinstance.RemoteHead != null) RemoteHead = inputsmanagerinstance.RemoteHead;
        if (inputsmanagerinstance.Controller != null) Controller = inputsmanagerinstance.Controller;
        if (inputsmanagerinstance.RemoteController != null) RemoteController = inputsmanagerinstance.RemoteController;

        LocalHeadAbsolutePosition = LocalHead.localToWorldMatrix * LocalHead.position;
        RemoteHeadAbsolutePosition = RemoteHead.localToWorldMatrix * RemoteHead.position;
        ControllerAbsolutePosition = Controller.localToWorldMatrix * Controller.position;
        RemoteControllerAbsolutePosition = RemoteController.localToWorldMatrix * RemoteController.position;

        if (ainst == null && Augmentations.Instance != null) ainst = Augmentations.Instance;

        if (ainst != null)
        {
            Pointer1 = ainst.PointOnMap == null ? Pointer1 : (Vector3)ainst.PointOnMap;
            Pointer2 = ainst.RemotePointOnMap == null ? Pointer1 : (Vector3)ainst.RemotePointOnMap;        
        }


        currentTime = Time.time - startTime;

        line = currentTime.ToString() + ","+
        LocalHeadAbsolutePosition.ToString().Trim(remove) + "," + LocalHead.eulerAngles.ToString().Trim(remove) + "," +
        ControllerAbsolutePosition.ToString().Trim(remove) + "," + Controller.eulerAngles.ToString().Trim(remove) + "," +
        Pointer1.ToString().Trim(remove) + "," +
        RemoteHeadAbsolutePosition.ToString().Trim(remove) + "," + RemoteHead.eulerAngles.ToString().Trim(remove) + "," +
        RemoteControllerAbsolutePosition.ToString().Trim(remove) + "," + RemoteController.eulerAngles.ToString().Trim(remove) + "," +
        Pointer2.ToString().Trim(remove);

        writer.WriteLine(line);
    }

    public void NewData(GameObject g) {

        StartCoroutine(NewDataCorutine(g));

        

    }

    public IEnumerator NewDataCorutine(GameObject g) {

        yield return Directory.CreateDirectory(Application.dataPath + "\\" + folderName + "\\");

        if (writer != null) writer.Close();

        inputsmanagerinstance = inputsManager.Instance;

        string path = Application.dataPath + "\\" + folderName + "\\" + g.name + ".csv";
        writer = new StreamWriter(path, true);

        startTime = Time.time;
    }

    void OnDisable()
    {
        if (writer != null) writer.Close();
    }

    void OnApplicationQuit()
    {
        if (writer != null) writer.Close();
    }
}
