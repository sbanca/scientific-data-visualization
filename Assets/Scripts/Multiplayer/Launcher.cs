using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
   
    void Start()
    {
        Resources.LoadAll("ScriptableObjects");
        Debug.Log("[PUN] connecting to server");
        PhotonNetwork.NickName = MasterManager.GameSettings.Nickname;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.Gameversion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {

        Debug.Log("[PUN] connected to server");
        Debug.Log("[PUN] connected with Nickname: " + PhotonNetwork.LocalPlayer.NickName);

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[PUN] disconnected from server because of " + cause.ToString());
    }
}
