using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeholderManager : MonoBehaviourPun
{
    public GameObject placeholderPrefab;

    public List<GameObject> placeholderList = new List<GameObject>();



    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            spawnLocalPlaceholder();
        }
        else if (Input.GetKeyDown("p")) {

            object[] data = new object[] { 0 };

            PhotonNetwork.RaiseEvent(MasterManager.GameSettings.DeletePlaceHolders,data, Photon.Realtime.RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);

            DeleteAllPlaceholders();
        }


    }

    public void RaiseNetworkEvent(int i)
    {

        object[] data = new object[] { i };

        PhotonNetwork.RaiseEvent(MasterManager.GameSettings.SpawnPlaceholder, data, Photon.Realtime.RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);

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
        if (obj.Code == MasterManager.GameSettings.SpawnPlaceholder)
        {

            object[] datas = (object[])obj.CustomData;

            spawnRemotePlaceholder((int)datas[0]);

        } else if (obj.Code == MasterManager.GameSettings.DeletePlaceHolders)
        {

            DeleteAllPlaceholders();
        }
    }

    private void spawnLocalPlaceholder()
    {

        GameObject placeholder = Instantiate(placeholderPrefab);

        PhotonView photonView = placeholder.GetComponent<PhotonView>();

        if (PhotonNetwork.AllocateViewID(photonView))
        {
            RaiseNetworkEvent(photonView.ViewID);

            placeholderList.Add(placeholder);
        }
        else
        {
            Debug.LogError("[PUN] Failed instantiate LocalAvatar, Failed to allocate a ViewId.");

            Destroy(placeholder);
        }

       
    }

    private void spawnRemotePlaceholder(int id) {

        GameObject placeholder = Instantiate(placeholderPrefab);

        PhotonView pv = placeholder.GetComponent<PhotonView>();
            
        pv.ViewID = id;

        placeholderList.Add(placeholder);

    }

    private void DeleteAllPlaceholders() {

        foreach (GameObject g in placeholderList)   Destroy(g);
        
    }
}
