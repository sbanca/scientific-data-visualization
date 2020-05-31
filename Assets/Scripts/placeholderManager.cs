using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeholderManager : MonoBehaviourPun
{
    public GameObject placeholderPrefab;

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            spawnLocalPlaceholder();         
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
        }
    }

    private void spawnLocalPlaceholder()
    {

        GameObject placeholder = Instantiate(placeholderPrefab);

        PhotonView photonView = placeholder.GetComponent<PhotonView>();

        if (PhotonNetwork.AllocateViewID(photonView))
        {
            RaiseNetworkEvent(photonView.ViewID);
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

       

    }

}
