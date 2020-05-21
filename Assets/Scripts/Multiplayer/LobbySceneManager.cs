using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class LobbySceneManager : MonoBehaviour
{
 
    [serializable]
    public InputField nickname;

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

        //UserId
        dropdown.value = 1;        
        dropdown.onValueChanged.AddListener(SetUserId);

        //switch
        btn.onClick.AddListener(delegate { SwitchScene(); });

    }


    public void SetUserId(int i) {

        MasterManager.GameSettings.UserID = dropdown.options[i].text; 

        Debug.Log("[UI] update UserID to: " + MasterManager.GameSettings.UserID);


    }

    public void SetNickName(string arg0){

        MasterManager.GameSettings.Nickname = arg0;

        Debug.Log("[UI] update nickname to: " + MasterManager.GameSettings.Nickname);

    }



    public void SwitchScene() {

        Debug.Log("[UI] ready to switch to: " + MasterManager.GameSettings.RoomName);

        SceneManager.LoadScene(sceneName: "Sample_scene_networking");

    }


}
