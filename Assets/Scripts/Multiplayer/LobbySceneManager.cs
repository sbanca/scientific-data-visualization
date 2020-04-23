using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class LobbySceneManager : MonoBehaviour
{
 
    [serializable]
    public InputField nickname;

    [serializable]
    public InputField userId;

    [serializable]
    public Dropdown dropdown;

    [serializable]
    public Button btn;

    void Start()
    {
        Resources.LoadAll("ScriptableObjects");

        //nickname
        nickname.text = MasterManager.GameSettings.Nickname;
        nickname.onEndEdit.AddListener(SetNickName);

        //User ID
        userId.text = MasterManager.GameSettings.UserID;
        userId.onEndEdit.AddListener(SetUserId);

        //RoomName
        dropdown.value = MasterManager.GameSettings.RoomName == RoomType.CollaborativeRoom.ToString() ? 0 : 1;        
        dropdown.onValueChanged.AddListener(SetRoomName);

        //switch
        btn.onClick.AddListener(delegate { SwitchScene(); });

    }


    public void SetUserId(string arg0) {

        MasterManager.GameSettings.UserID = arg0;

        Debug.Log("[UI] update UserID to: " + MasterManager.GameSettings.UserID);

    }

    public void SetNickName(string arg0){

        MasterManager.GameSettings.Nickname = arg0;

        Debug.Log("[UI] update nickname to: " + MasterManager.GameSettings.Nickname);

    }

    public void SetRoomName(int i) {

        MasterManager.GameSettings.RoomName = i == (int)RoomType.CollaborativeRoom ? RoomType.CollaborativeRoom.ToString() : RoomType.PersonalRoom.ToString();

        Debug.Log("[UI] update room name to: " + MasterManager.GameSettings.RoomName);

      
    }


    public void SwitchScene() {

        Debug.Log("[UI] ready to switch to: " + MasterManager.GameSettings.RoomName);

        SceneManager.LoadScene(sceneName: "Sample_scene_networking");

    }


}
