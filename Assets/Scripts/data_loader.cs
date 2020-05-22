
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_loader : MonoBehaviourPun
{

    [SerializeField]
    public GameObject[] dataprefabs;

    GameObject currentData;

    private Queue<GameObject> dataPrefabsQueue;

    private const byte LOAD_NEXT_DATA = 10;

    public AvatarBehaviourRecorder avatarRecorder;

    public PartecipantsVoiceRecorder partecipantsVoiceRecorder;

    //public RecorderAll recorderAll;

    private void Start()
    {
        dataPrefabsQueue = new Queue<GameObject>();

        foreach(GameObject g in dataprefabs) dataPrefabsQueue.Enqueue(g);
    }

    IEnumerator LoadNext() {

        if (!this.enabled) yield break;

        if(currentData!=null)  Destroy(currentData);




        if (dataPrefabsQueue.Count > 0) {
            
            currentData = Instantiate(dataPrefabsQueue.Dequeue());

            if (!currentData.activeSelf) currentData.SetActive(true);

            IgnoreCollistion();

        }

    }

    public void IgnoreCollistion() { 

        CharacterController charCon = FindObjectOfType<CharacterController>();

        if (charCon == null) return;

        Collider[] colliders = currentData.GetComponentsInChildren<Collider>();

        foreach (Collider c in colliders) Physics.IgnoreCollision(charCon, c);


    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Next();
    }

    public void Next() {

        partecipantsVoiceRecorder.StartRecording();
        //if (!recorderAll.recOutput) recorderAll.StartRecording();

        if (dataPrefabsQueue.Count > 0)
        {

            StartCoroutine(LoadNext());

            RaiseNetworkEvent();

            RecordEvent();

        }
        else {

            StartCoroutine(LoadNext());

            StopRecorder();

        }
    
    }

    public void RecordEvent() {

        avatarRecorder.NewData(currentData);
    }

    public void StopRecorder()
    {

        avatarRecorder.enabled=false;
    }

    public void RaiseNetworkEvent() {

        object[] data = new object[] { };

        PhotonNetwork.RaiseEvent(MasterManager.GameSettings.NextDataDisplay, data, Photon.Realtime.RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);

    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClientEventReceived;

    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClientEventReceived;

    }

    private void NetworkingClientEventReceived(EventData obj)
    {
        if (obj.Code == MasterManager.GameSettings.NextDataDisplay)
        {

            StartCoroutine(LoadNext());

            object[] datas = (object[])obj.CustomData;

        }
    }



}      


