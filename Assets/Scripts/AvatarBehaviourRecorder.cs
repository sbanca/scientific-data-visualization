﻿using System.IO;
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
        if (writer == null ) return;

        if (inputsmanagerinstance.LocalHead != null) LocalHead = inputsmanagerinstance.LocalHead;
        if (inputsmanagerinstance.RemoteHead != null) RemoteHead = inputsmanagerinstance.RemoteHead;
        if (inputsmanagerinstance.Controller != null) Controller = inputsmanagerinstance.Controller;
        if (inputsmanagerinstance.RemoteController != null) RemoteController = inputsmanagerinstance.RemoteController;

        LocalHeadAbsolutePosition = LocalHead.localToWorldMatrix * LocalHead.position;
        RemoteHeadAbsolutePosition = RemoteHead.localToWorldMatrix * RemoteHead.position;
        ControllerAbsolutePosition = Controller.localToWorldMatrix * Controller.position;
        RemoteControllerAbsolutePosition = RemoteController.localToWorldMatrix * RemoteController.position;

        if (ainst == null)
        {
            ainst = FindObjectOfType<Augmentations>();
        }

        if (ainst != null)
        {
            Pointer1 = ainst.PointOnMap == null ? Pointer1 : (Vector3)ainst.PointOnMap;
            Pointer2 = ainst.RemotePointOnMap == null ? Pointer1 : (Vector3)ainst.RemotePointOnMap;        
        }


        currentTime = Time.time - startTime;

        line = currentTime.ToString("F3") + ","+
        LocalHeadAbsolutePosition.ToString("F3").Trim(remove) + "," + LocalHead.eulerAngles.ToString("F3").Trim(remove) + "," +
        ControllerAbsolutePosition.ToString("F3").Trim(remove) + "," + Controller.eulerAngles.ToString("F3").Trim(remove) + "," +
        Pointer1.ToString("F3").Trim(remove) + "," +
        RemoteHeadAbsolutePosition.ToString("F3").Trim(remove) + "," + RemoteHead.eulerAngles.ToString("F3").Trim(remove) + "," +
        RemoteControllerAbsolutePosition.ToString("F3").Trim(remove) + "," + RemoteController.eulerAngles.ToString("F3").Trim(remove) + "," +
        Pointer2.ToString("F3").Trim(remove);

        writer.WriteLine(line);
    }

    public void NewData(GameObject g) {

        closeWriter();

        if (inputsmanagerinstance == null) inputsmanagerinstance = inputsManager.Instance;

        string path = Application.dataPath + "\\" + MasterManager.GameSettings.DataFolder + "\\" + g.name + ".csv";
        writer = new StreamWriter(path, true);

        writer.WriteLine("time in s, LocalHeadX, LocalHeadY, LocalHeadZ,LocalHeadEulerX, LocalHeadEulerY, LocalHeadEulerZ, ControllerX, ControllerY, ControllerZ,ControllerEulerX, ControllerEulerY, ControllerEulerZ,Pointer1X,Pointer1Y,Pointer1Z," +
           "RemoteHeadX, RemoteHeadY, RemoteHeadZ,RemoteHeadEulerX, RemoteHeadEulerY, RemoteHeadEulerZ, RemoteControllerX, RemoteontrollerY, RemoteControllerZ,ControllerEulerX, RemoteControllerEulerY, RemoteControllerEulerZ,Pointer2X,Pointer2Y,Pointer2Z,");

        startTime = Time.time;

    }

    void OnDisable()
    {
        closeWriter();
    }

    void OnApplicationQuit()
    {
        closeWriter(); 
    }

    void closeWriter() {

        if (writer != null) writer.Close();

        writer = null;

    }

}
