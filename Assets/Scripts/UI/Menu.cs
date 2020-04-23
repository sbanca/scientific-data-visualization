using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Tayx.Graphy;
using FireStorage;

public class Menu : MonoBehaviour
{
    public static Menu Instance;
    private GameObject MainMenu;
    bool inMenu;

    [SerializeField]
    [Tooltip("The Prefab of the menu")]
    private GameObject MenuPrefab = null;

    private Text sliderText;

    [SerializeField]
    [Tooltip("The transform used to align the menu")]
    Transform transform;

    void Start()
    {
      
        MainMenu = Instantiate(MenuPrefab);

        MainMenu.gameObject.transform.position = transform.position;

        MainMenu.gameObject.transform.rotation = transform.rotation;

        MainMenu.gameObject.transform.parent = transform;

        MainMenu.gameObject.transform.localPosition = new Vector3(0f,0.25f,0.1f);

        MainMenu.gameObject.transform.localScale = new Vector3(0.25f,0.25f,0.25f);


        Hide();

    }


    void DownloadData(string name)
    {
        Debug.Log("[MENU] Trigger FireManager to download --> " + name);

        StartCoroutine(FireManager.instance.DownloadFile(name));

    }


    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start))
        {


            if (inMenu)
            {

                Hide();
            }
            else
            {
                Show();


            }
            inMenu = !inMenu;
        }
    }

    void Hide()
    {
        MainMenu.SetActive(true); 
        
    }

    void Show()
    {
        MainMenu.SetActive(false);
    }
}
