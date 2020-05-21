using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string _gameversion = "0.0.1";

    public string Gameversion { get { return _gameversion; } }

    [SerializeField]

    private string _nickname = "YourNameHere";

    public string Nickname {
        get {

            int value = UnityEngine.Random.Range(0, 9999);

            return _nickname + value.ToString();

        }

        set { _nickname = value; }
    }


    [SerializeField]

    private string _userID ;

    public string UserID { 
        get {
            return _userID; 
        }
        set {

            if (value == "Partecipant_0") _userID = "0";
            else if (value == "Partecipant_1") _userID = "2671308206268206";
            else if (value == "Partecipant_2") _userID = "2911531572263440";

        }
    }


    [SerializeField]
    private string _roomName = "CollaborationRoom"; 

    public string RoomName { 
        get { return _roomName; }
        set { _roomName = value; }
    }

  

    [SerializeField]
    public byte InstantiateVrAvatarEventCode = 1; // example code, change to any value between 1 and 199

}


public enum RoomType { 

    CollaborativeRoom =0 ,
    PersonalRoom =1
}

