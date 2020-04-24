using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Tayx.Graphy;
using FireStorage;

public class Menu : Singleton<Menu>
{
    public GameObject[] menuList;
    bool inMenu;

    [SerializeField]
    [Tooltip("The Prefab of the menu")]
    private GameObject MainMenuPrefab;
    private GameObject MainMenu;

    [SerializeField]
    [Tooltip("The Prefab of the data menu")]
    private GameObject DataMenuPrefab ;
    private GameObject DataMenu;


    [SerializeField]
    [Tooltip("The transform used to align the menu")]
    Transform transform;

    [SerializeField]
    [Tooltip("Ui Helper to instantiate")]
    GameObject UiHelprefab;
    GameObject UiHelper;

    LaserPointer lp;

    public LaserPointer.LaserBeamBehavior laserBeamBehavior;

    [SerializeField]
    [Tooltip("menu scaling")]
    public Vector3 scale = new Vector3(0.25f,0.25f,0.25f);

    [SerializeField]
    [Tooltip("menu positioning")]
    public Vector3 position = new Vector3(0f, 0.25f, 0.1f);

    private int ActiveMenu = 0;

    void Start()
    {

        //instantiate menus
        MainMenu = InstantiateMenus( MainMenuPrefab);
        DataMenu = InstantiateMenus( DataMenuPrefab);

        menuList = new GameObject[2];
        menuList[0] = MainMenu;
        menuList[1] = DataMenu;

        switchActiveMenu(0);

        //instantiate UiHelper

        UiHelper = Instantiate(UiHelprefab);

        lp = FindObjectOfType<LaserPointer>();
        if (!lp)
        {
            Debug.LogError("UI requires use of a LaserPointer and will not function without it. Add one to your scene, or assign the UIHelpers prefab to the DebugUIBuilder in the inspector.");
            return;
        }
        lp.laserBeamBehavior = laserBeamBehavior;

        //GetComponent<OVRRaycaster>().pointer = lp.gameObject;

        Hide();

    }

    public void switchActiveMenu(int i) {

        foreach (GameObject go in menuList) go.SetActive(false);

        menuList[i].SetActive(true);
        
    
    }

    GameObject InstantiateMenus( GameObject prefab) {

        GameObject go;

        go = Instantiate(prefab);

        go.gameObject.transform.position = transform.position;

        go.gameObject.transform.rotation = transform.rotation;

        go.gameObject.transform.parent = transform;

        go.gameObject.transform.localPosition = position;

        go.gameObject.transform.localScale = scale;

        return go;
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
        foreach (GameObject go in menuList) go.SetActive(false);
        UiHelper.SetActive(false);
    }

    void Show()
    {
        MainMenu.SetActive(true);
        UiHelper.SetActive(true);
    }

}
