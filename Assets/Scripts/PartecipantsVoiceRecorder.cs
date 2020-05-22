using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PartecipantsVoiceRecorder : MonoBehaviour
{
    BinaryWriter writer;

    // Start is called before the first frame update
    void Start()
    {
        PhotonVoiceNetwork.Instance.RemoteVoiceAdded += OpenFileStream;
    }

    private void OpenFileStream(RemoteVoiceLink obj)
    {

        //Create and open file for the stream in RemoteVoiceAdded handler.

        string fileName = obj.PlayerId.ToString();

        string path = Application.dataPath + "\\" + MasterManager.GameSettings.DataFolder + "\\" + fileName + ".mp3";

        BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create));

        obj.FloatFrameDecoded += WriteFrameAudioData;
   
    }

    private void SaveAndCloseFile(int s)
    {
        //Save and close the file in RemoteVoiceRemoved handler.
        writer.Close();

    }

    private void WriteFrameAudioData(float[] obj)
    {
        //Write frame of audio data in FloatFrameDecoded handler.

        foreach (var value in obj)
        {
            writer.Write(value);
        }

    }


}
