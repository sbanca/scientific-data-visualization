﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using Oculus.Avatar;
using System;
using System.Collections.Generic;

public class Launcher : MonoBehaviourPunCallbacks, IConnectionCallbacks, IMatchmakingCallbacks, IOnEventCallback
{
    private GameObject localAvatar;

    private PhotonView photonView;

    public GameObject rig;

    public GameObject loading;

    public data_loader loader;

    public bool observer = false;

    public bool voiceDebug = true;
    void Start()
    {
        Resources.LoadAll("ScriptableObjects");
        Debug.Log("[PUN] connecting to server");

        PhotonNetwork.AuthValues = new AuthenticationValues();

        if (observer) {

            PhotonNetwork.NickName ="Observer";
            PhotonNetwork.AuthValues.UserId = "1";
        }
        else
        {
            PhotonNetwork.NickName = MasterManager.GameSettings.Nickname;           
            PhotonNetwork.AuthValues.UserId = MasterManager.GameSettings.UserID;           
        }
     
        PhotonNetwork.GameVersion = MasterManager.GameSettings.Gameversion;
        PhotonNetwork.ConnectUsingSettings();
  
    }

    public override void OnConnectedToMaster() {

        Debug.Log("[PUN] connected to server");

        Debug.Log("[PUN] connected with Nickname: " + PhotonNetwork.LocalPlayer.NickName + "\n UserID: " + PhotonNetwork.LocalPlayer.UserId);

        Debug.Log("[PUN] joining room " + MasterManager.GameSettings.RoomName);

        RoomOptions options = new RoomOptions();
        options.PublishUserId = true;
        PhotonNetwork.JoinOrCreateRoom(MasterManager.GameSettings.RoomName, options, TypedLobby.Default);
    }

    void IMatchmakingCallbacks.OnJoinedRoom()
    {
        Debug.Log("[PUN] joined room " + PhotonNetwork.CurrentRoom);

        if (!observer) InstantiateLocalAvatar();

    }

    void InstantiateLocalAvatar() {

        Debug.Log("[PUN] instantiate LocalAvatar");

        GameObject OVRPlayerController = GameObject.Find("OVRPlayerController");
        photonView = OVRPlayerController.AddComponent<PhotonView>();//Add a photonview to the OVR player controller 
        PhotonTransformView photonTransformView = OVRPlayerController.AddComponent<PhotonTransformView>();//Add a photonTransformView to the OVR player controller 
        photonTransformView.m_SynchronizeRotation = false;
        photonView.ObservedComponents = new List<Component>();
        photonView.ObservedComponents.Add(photonTransformView);
        photonView.Synchronization = ViewSynchronization.UnreliableOnChange; // set observeoption to unreliableonchange

        //instantiate the local avatr
        GameObject TrackingSpace = GameObject.Find("TrackingSpace");
        localAvatar = Instantiate(Resources.Load("LocalAvatar"), TrackingSpace.transform.position, TrackingSpace.transform.rotation, TrackingSpace.transform) as GameObject;
        PhotonAvatarView photonAvatrView = localAvatar.GetComponent<PhotonAvatarView>();
        photonAvatrView.photonView = photonView;
        photonAvatrView.ovrAvatar = localAvatar.GetComponent<OvrAvatar>();
        photonView.ObservedComponents.Add(photonAvatrView);


        if (PhotonNetwork.AllocateViewID(photonView))
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.Others
            };

            PhotonNetwork.RaiseEvent(MasterManager.GameSettings.InstantiateVrAvatarEventCode, photonView.ViewID, raiseEventOptions, SendOptions.SendReliable);

            OvrAvatar ovrAvatar = localAvatar.GetComponent<OvrAvatar>();
            ovrAvatar.oculusUserID = MasterManager.GameSettings.UserID;

            Debug.Log("[PUN] LocalAvatar instantiatiation triggered now waiting for OVRAvatar to initialize");

            OvrAvatar.LocalAvatarInstantiated += LocalAvatarInstantiated;
        }
        else
        {
            Debug.LogError("[PUN] Failed instantiate LocalAvatar, Failed to allocate a ViewId.");

            Destroy(localAvatar);
        }
    }

    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == MasterManager.GameSettings.InstantiateVrAvatarEventCode)
        {
            //sender 
            Player player = PhotonNetwork.CurrentRoom.Players[photonEvent.Sender];

            //if the player is an observer exit
            if (player.NickName == "Observer") return;

            Debug.Log("[PUN] Instantiatate an avatar for user " + player.NickName + "\n with user ID "+ player.UserId);

            GameObject remoteAvatar = Instantiate(Resources.Load("RemoteAvatar")) as GameObject;
          
            PhotonView photonView = remoteAvatar.GetComponent<PhotonView>();
            photonView.ViewID = (int)photonEvent.CustomData;

            OvrAvatar ovrAvatar = remoteAvatar.GetComponent<OvrAvatar>();
            ovrAvatar.oculusUserID = player.UserId;

            Debug.Log("[PUN] RemoteAvatar instantiated" );

            OvrAvatar.RemoteAvatarInstantiated += OvrAvatar_RemoteAvatarInstantiated;
        }
    }

    private GameObject OvrAvatar_RemoteAvatarInstantiated(GameObject remoteAvatar)
    {
        if (inputsManager.Instance.RemoteHead == null) inputsManager.Instance.RemoteHead = remoteAvatar.transform.Find("head_JNT");
        else if (inputsManager.Instance.LocalHead == null) inputsManager.Instance.LocalHead = remoteAvatar.transform.Find("head_JNT");
        else Debug.LogError("inputs manager cannot register any new transform");

        if (inputsManager.Instance.RemoteController == null) inputsManager.Instance.RemoteController = remoteAvatar.transform.Find("hand_right");
        else if (inputsManager.Instance.Controller == null) inputsManager.Instance.Controller = remoteAvatar.transform.Find("hand_right");
        else Debug.LogError("inputs manager cannot register any new transform");

        return remoteAvatar;
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[PUN] disconnected from server because of " + cause.ToString());
    }

    private void LocalAvatarInstantiated() {

        StartCoroutine(PhotonVoiceInstantiation());

        inputsManager.Instance.LocalHead = Camera.main.transform;
        inputsManager.Instance.RemoteController = localAvatar.transform.Find("hand_right");
    }
   
    private IEnumerator PhotonVoiceInstantiation()
    {
       
        Debug.Log("[PUN] OVRAvatar completed instantiation of LocalAvatar now we setup voice by adding Speaker,Recorder,VoiceView ");

        //get audiosource from the localavatar       
        while (localAvatar.GetComponentInChildren<AudioSource>() == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        AudioSource audioSource = localAvatar.GetComponentInChildren<AudioSource>();

        //////get the ovr 
        while (audioSource.gameObject.GetComponent<OVRLipSyncContext>() == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        OVRLipSyncContext LipSyncContext = audioSource.gameObject.GetComponent<OVRLipSyncContext>();
        LipSyncContext.audioSource = audioSource;
        if (voiceDebug) LipSyncContext.audioLoopback = true; // Don't use mic.
        else LipSyncContext.audioLoopback = false;
        LipSyncContext.skipAudioSource = false;

        ////add speaker to the element which holds the audio source 
        Speaker speaker = audioSource.gameObject.AddComponent<Speaker>();

        ////add recorder to the element that has the photonView
        Recorder recorder = photonView.gameObject.AddComponent<Recorder>();
        recorder.DebugEchoMode = true;

        ////add Photonvoice view to the local avatar
        PhotonVoiceView voiceView = photonView.gameObject.AddComponent<PhotonVoiceView>();
        voiceView.RecorderInUse = recorder;
        voiceView.SpeakerInUse = speaker;
        voiceView.SetupDebugSpeaker = true;

        ////start transmission 
        yield return voiceView.RecorderInUse.TransmitEnabled = true;
        voiceView.RecorderInUse.StartRecording();

        //activate Menu
        if(rig.GetComponentInChildren<Menu>()!=null) rig.GetComponentInChildren<Menu>().enabled = true; //rig menu  
        loading.SetActive(false);

        //load Data
        if (loader != null) loader.LoadNext();

    }
    
    private void ActivateAndPositionRig(GameObject go, int sender = 1000) {

        Vector3 position1 = new Vector3(0f, 0f, 0f);
        Vector3 position2 = new Vector3(0f, 0f, 2.5f);
        Vector3 position3 = new Vector3(2.5f, 0f, 2.5f);
        Vector3 position4 = new Vector3(2.5f, 0f, 0f);

        if (sender == 1000) sender = PhotonNetwork.CountOfPlayers;
        
        switch (sender) {

            case(1):
                go.transform.position = position1;
                go.transform.eulerAngles = new Vector3(0f, 45f, 0f);
                break;
            case (2):
                go.transform.position = position2;
                go.transform.eulerAngles = new Vector3(0f, 90f+45f, 0f);
                break;
            case (3):
                go.transform.position = position3;
                go.transform.eulerAngles = new Vector3(0f, 180f + 45f, 0f);
                break;
            case (4):
                go.transform.position = position4;
                go.transform.eulerAngles = new Vector3(0f, 270f + 45f, 0f);
                break;

        }

       
    }
}
