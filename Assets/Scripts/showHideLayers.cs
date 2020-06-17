using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class showHideLayers : MonoBehaviourPun
{
    [serializable]
    public GameObject[] Layers;

    [serializable]
    public bool mutuallyExclusiveLayers = false;

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            hideUnhideLayer(0);
            RaiseNetworkEvent(0);
        }
        else if (Input.GetKeyDown("2"))
        {
            hideUnhideLayer(1);
            RaiseNetworkEvent(1);
        }
        else if (Input.GetKeyDown("3"))
        {
            hideUnhideLayer(2);
            RaiseNetworkEvent(2);
        }
        else if (Input.GetKeyDown("4"))
        {
            hideUnhideLayer(3);
            RaiseNetworkEvent(3);
        }
        else if (Input.GetKeyDown("5"))
        {
            hideUnhideLayer(4);
            RaiseNetworkEvent(4);
        }
        else if (Input.GetKeyDown("6"))
        {
            hideUnhideLayer(5);
            RaiseNetworkEvent(5);
        }
    }
    
    public void hideUnhideLayer(int i) {

        GameObject layer = Layers[i];

        if (mutuallyExclusiveLayers)
        {
            foreach (GameObject l in Layers)
            {
                if (l.name != layer.name) l.SetActive(false);
            }
        }

        if (layer.activeSelf)
        {
            layer.SetActive(false);

        }
        else
        {
            layer.SetActive(true);
            IgnoreCollistion(layer);
        }

       
    }
    
    public void RaiseNetworkEvent(int i)
    {

        object[] data = new object[] { i };

        PhotonNetwork.RaiseEvent(MasterManager.GameSettings.HideUnhideLayer, data, Photon.Realtime.RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);

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
        if (obj.Code == MasterManager.GameSettings.HideUnhideLayer)
        {

            object[] datas = (object[])obj.CustomData;

            hideUnhideLayer((int)datas[0]);
        }
    }

    public void IgnoreCollistion(GameObject obj)
    {

        CharacterController charCon = FindObjectOfType<CharacterController>();

        if (charCon == null) return;

        Collider[] colliders = obj.GetComponentsInChildren<Collider>();

        foreach (Collider c in colliders) Physics.IgnoreCollision(charCon, c);

    }

}
