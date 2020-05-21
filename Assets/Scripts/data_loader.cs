
using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class data_loader : MonoBehaviourPun
{

    [SerializeField]
    public GameObject[] dataprefabs;

    GameObject currentData;

    private Queue<GameObject> dataPrefabsQueue;

    private const byte LOAD_NEXT_DATA = 0;

    private void Start()
    {
        dataPrefabsQueue = new Queue<GameObject>();

        foreach(GameObject g in dataprefabs) dataPrefabsQueue.Enqueue(g);
    }

    public void LoadNext() {

        if (!this.enabled) return;

        if(currentData!=null) Destroy(currentData);

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
        if (Input.GetKeyDown(KeyCode.Q)) {

            LoadNext();

            RaiseNetworkEvent();

        }
    }

    public void RaiseNetworkEvent() {

        object[] data = new object[] { };

        PhotonNetwork.RaiseEvent(LOAD_NEXT_DATA, data, Photon.Realtime.RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);

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
        if (obj.Code == LOAD_NEXT_DATA) {

            LoadNext();

            object[] datas = (object[])obj.CustomData;

        }
    }
}      


