using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;

public class QuestMenu : MonoBehaviour
{
    public static QuestMenu Instance;
    bool inMenu;

    [SerializeField]
    private RectTransform ConsolePrefab = null;

    private ConsoleViewer ConsoleManager;

   

    void Start()
    {
        ConsoleManager = GameObject.Instantiate(ConsolePrefab).GetComponent<ConsoleViewer>();

        ConsoleManager.Show();

        inMenu = true;

    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (inMenu) {

                //DebugUIBuilderExtension.instance.Hide();
                ConsoleManager.Show();
            } 
            else {

                //DebugUIBuilderExtension.instance.Show();
                ConsoleManager.Hide();
            } 
            inMenu = !inMenu;
        }
    }

    

}
