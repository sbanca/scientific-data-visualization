using FireStorage;
using System.Collections;
using SpaceBear.VRUI;
using UnityEngine;
using UnityEngine.UI;

public class DataMenu : MonoBehaviour
{
    [serializable]
    [Tooltip("The Prefab of Radio Button")]
    public GameObject RadioPrefab = null;

    [serializable]
    [Tooltip("The Prefab of Button Button")]
    public GameObject ButtonPrefab = null;

    private Transform DropdownContainer;
    private void Start()
    {
        VRUIRadio[] radiolist = GetComponentsInChildren<VRUIRadio>();

        DropdownContainer = radiolist[0].gameObject.transform.parent ;

        foreach (VRUIRadio d in radiolist)  Destroy(d.gameObject);        

        StartCoroutine(LoadButtons());

        
    }


    IEnumerator LoadButtons()
    {

        yield return gameObject.AddComponent<FireManager>();

        if (FireManager.instance.objectlist == null)
            yield return FireManager.instance.GetFileList();

        foreach (string obj in FireManager.instance.objectlist)
        {
            GameObject dropdown = Instantiate(RadioPrefab, DropdownContainer);

            dropdown.GetComponentInChildren<Text>().text = obj;

        }

        Button btn = GetComponentInChildren<Button>();

        btn.onClick.AddListener(DownloadData);
    }

    void DownloadData()
    {
        string name = "";

        VRUIRadio[] radiolist = GetComponentsInChildren<VRUIRadio>();

        foreach (VRUIRadio d in radiolist) {

            if (d.isOn) name = d.gameObject.GetComponentInChildren<Text>().text;
        }

        if (name == "") return;

        Debug.Log("[MENU] Trigger FireManager to download --> " + name);

        StartCoroutine(FireManager.instance.DownloadFile(name));

    }
}
