﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using Oculus.Avatar;
using System;

public class Launcher : MonoBehaviourPunCallbacks, IConnectionCallbacks, IMatchmakingCallbacks, IOnEventCallback
{
    private GameObject localAvatar;

    public GameObject rig;

    public GameObject loading;

    public GameObject data;


    public bool voiceDebug = true;

    public bool remoteAvatarAnimationDebug = true;

    [SerializeField]
    public string animationfilename;

    void Start()
    {
        Resources.LoadAll("ScriptableObjects");
        Debug.Log("[PUN] connecting to server");
        PhotonNetwork.NickName = MasterManager.GameSettings.Nickname;
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.UserId = MasterManager.GameSettings.UserID;
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

        Debug.Log("[PUN] instantiate LocalAvatar" );

        ActivateAndPositionRig(rig);

        GameObject TrackingSpace = GameObject.Find("TrackingSpace");
        localAvatar = Instantiate(Resources.Load("LocalAvatar"), TrackingSpace.transform.position, TrackingSpace.transform.rotation, TrackingSpace.transform) as GameObject;
        PhotonView photonView = localAvatar.GetComponent<PhotonView>();

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
            Debug.Log("[PUN] Instantiatate an avatar for user " + player.NickName + "\n with user ID "+ player.UserId);

            GameObject remoteAvatar = Instantiate(Resources.Load("RemoteAvatar")) as GameObject;
            ActivateAndPositionRig(remoteAvatar, photonEvent.Sender);

            //TableTop.RayOnMap.Instance.RemoteHead = remoteAvatar.transform;

            PhotonView photonView = remoteAvatar.GetComponent<PhotonView>();
            photonView.ViewID = (int)photonEvent.CustomData;

            OvrAvatar ovrAvatar = remoteAvatar.GetComponent<OvrAvatar>();
            ovrAvatar.oculusUserID = player.UserId;

            Debug.Log("[PUN] RemoteAvatar instantiated" );

            
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[PUN] disconnected from server because of " + cause.ToString());
    }

    private void LocalAvatarInstantiated() {

        StartCoroutine(PhotonVoiceInstantiation());
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

        ////add recorder to the local avatar 
        Recorder recorder = localAvatar.AddComponent<Recorder>();
        recorder.DebugEchoMode = true;

        ////add Photonvoice view to the local avatar
        PhotonVoiceView voiceView = localAvatar.AddComponent<PhotonVoiceView>();
        voiceView.RecorderInUse = recorder;
        voiceView.SpeakerInUse = speaker;
        voiceView.SetupDebugSpeaker = true;

        ////start transmission 
        yield return voiceView.RecorderInUse.TransmitEnabled = true;
        voiceView.RecorderInUse.StartRecording();

        //activate 
        if(rig.GetComponentInChildren<Menu>()!=null) rig.GetComponentInChildren<Menu>().enabled = true; //rig menu  
        loading.SetActive(false);
        data.SetActive(true);
        if (remoteAvatarAnimationDebug) remoteAvatarAnimationDebugInstantiation();

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

    private void remoteAvatarAnimationDebugInstantiation() {
      
        Debug.Log("[PUN] RemoteAvatar anaimation instantiation started");

        GameObject remoteAvatar = Instantiate(Resources.Load("RemoteAvatar")) as GameObject;

        remoteAvatar.SetActive(false);

        //set position and rotation to second place

        remoteAvatar.transform.position = new Vector3(0f, 0f, 2.5f);

        remoteAvatar.transform.eulerAngles = new Vector3(0f, 90f , 0f);

       
        //remove components unneded from the prefab

        PhotonAvatarView pav = remoteAvatar.GetComponent<PhotonAvatarView>();
        if (pav != null) 
            Destroy(pav);

        PhotonVoiceView pvv = remoteAvatar.GetComponent<PhotonVoiceView>();
        if (pvv != null) 
            Destroy(pvv);

        Speaker spk = remoteAvatar.GetComponentInChildren<Speaker>();
        if (spk != null) 
            Destroy(spk.gameObject);

        PhotonView pv = remoteAvatar.GetComponent<PhotonView>();
        if (pv != null)
            Destroy(pv);


        //turn on sdk packets and remotedriver
        remoteAvatar.GetComponent<OvrAvatar>().UseSDKPackets = true;
        remoteAvatar.GetComponent<OvrAvatarRemoteDriver>().enabled = true;

        //add playback 
        Playback plbk = gameObject.AddComponent<Playback>();
        plbk.playback = true;
        plbk.fileName = animationfilename;
        plbk.LoopbackAvatar = remoteAvatar.GetComponent<OvrAvatar>();

        //set remote avatar active
        remoteAvatar.SetActive(true);

        //add reote avatar head to rayonmap augmentatoin 
        StartCoroutine(AddRemoteAvatarHeadToAugmentation(remoteAvatar));

        Debug.Log("[PUN] RemoteAvatar anaimation instantiation completed");
    }

    private IEnumerator AddRemoteAvatarHeadToAugmentation(GameObject remoteAvatar)
    {

        while (!data.active)
        {
            yield return new WaitForSeconds(0.1f);
        }

        
        
        GazeTarget thisgt = null;


        while (thisgt == null)
        {

            GazeTarget[] list = remoteAvatar.GetComponentsInChildren<GazeTarget>();

            foreach (GazeTarget gt in list) {

                if (gt.gameObject.name == "head_JNT") thisgt = gt;
            }

            yield return new WaitForSeconds(0.1f);

        }


        while (TableTop.RayOnMap.Instance ==null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        TableTop.RayOnMap.Instance.AddRemoteHead(thisgt.transform);
    }

 }
