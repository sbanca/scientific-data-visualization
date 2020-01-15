using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Tayx.Graphy;
using FireStorage;

public class QuestMenu : MonoBehaviour
{
    public static QuestMenu Instance;
    bool inMenu;

    [SerializeField]
    [Tooltip("The Prefab of the console")]
    private RectTransform ConsolePrefab = null;

    [SerializeField]
    [Tooltip("The Prefab of the menu")]
    private RectTransform MenuPrefab = null;

    [SerializeField]
    [Tooltip("The Prefab of GRAPHY")]
    private RectTransform GraphyPrefab = null;

    private Text sliderText;

    OVRCameraRig rig;

    void Start()
    {
        //get camera reference
        rig = FindObjectOfType<OVRCameraRig>();

        // Instantiate Menu and Console
        Instantiate(ConsolePrefab);
        Instantiate(MenuPrefab);
        Instantiate(GraphyPrefab);

        // Menu
        UIBuilder.instance.AddLabel("Data Download",0);
        UIBuilder.instance.AddDivider(0);

        StartCoroutine(LoadButtons());

        UIBuilder.instance.AddLabel("Debug Controls", 1);
        UIBuilder.instance.AddDivider(1);
        UIBuilder.instance.AddButton("Clear Debug Log", ClearLog , 1);

       
        UIBuilder.instance.Show();
        ConsoleManager.instance.Show();

        GraphyManager.Instance.Enable();
        GraphyManager.Instance.transform.position = rig.transform.TransformPoint(-2.57f, -0.7f, 2f);
        GraphyManager.Instance.transform.rotation = rig.transform.rotation;

        inMenu = true;

    }

    IEnumerator LoadButtons()
    {

        yield return gameObject.AddComponent<FireManager>();

        if (FireManager.instance.objectlist == null)
            yield return FireManager.instance.GetFileList();

        foreach (string obj in FireManager.instance.objectlist) {
            UIBuilder.instance.AddButton(obj, DownloadData);
        }

        ResetMenu();

    }

    void LogButtonPressed( string name)
    {
        Debug.Log("[MENU] Button pressed --> "  + name );
       
    }

    void ClearLog(string name)
    {
        Debug.Log("[MENU] Button pressed --> " + name);
        ConsoleManager.instance.ClearLog();

    }

    void DownloadData(string name)
    {
        Debug.Log("[MENU] Trigger FireManager to download --> " + name);

        StartCoroutine(FireManager.instance.DownloadFile(name));

    }

    void ResetMenu() {

        UIBuilder.instance.Show();
        ConsoleManager.instance.Show();

    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (inMenu) {

                UIBuilder.instance.Hide();
                ConsoleManager.instance.Hide();
                GraphyManager.Instance.Disable();
            } 
            else {

                UIBuilder.instance.Show();
                ConsoleManager.instance.Show();
                GraphyManager.Instance.Enable();
                GraphyManager.Instance.transform.position = rig.transform.TransformPoint(-2.57f,-0.7f,2f);
                GraphyManager.Instance.transform.rotation = rig.transform.rotation;
            } 
            inMenu = !inMenu;
        }
    }

}
