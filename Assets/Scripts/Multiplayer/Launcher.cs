using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
   
    void Start()
    {
        Debug.Log("[PUN] connecting to server");
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {

        Debug.Log("[PUN] connected to server");

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[PUN] disconnected from server because of " + cause.ToString());
    }
}
