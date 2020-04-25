using UnityEngine;


public class Menu : Singleton<Menu>
{
    public GameObject[] menuHandList;
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
    [Tooltip("The Prefab of the performance monitor")]
    private GameObject PerformancesPrefab;
    public GameObject Performances;

    [SerializeField]
    [Tooltip("The Console Log ")]
    private GameObject ConsoleLogPrefab;
    public GameObject ConsoleLog;

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
        MainMenu = InstantiateMenuHand( MainMenuPrefab);
        DataMenu = InstantiateMenuHand( DataMenuPrefab);
        ConsoleLog = InstantiateMenu(ConsoleLogPrefab);
        Performances = InstantiateMenu(PerformancesPrefab);

        menuHandList = new GameObject[2];
        menuHandList[0] = MainMenu;
        menuHandList[1] = DataMenu;

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

        Hide();

    }

    public void switchActiveMenu(int i) {

        foreach (GameObject go in menuHandList) go.SetActive(false);

        menuHandList[i].SetActive(true);

        ActiveMenu = i;


    }

    GameObject InstantiateMenuHand( GameObject prefab) {

        GameObject go;

        go = Instantiate(prefab);

        go.gameObject.transform.position = transform.position;

        go.gameObject.transform.rotation = transform.rotation;

        go.gameObject.transform.parent = transform;

        go.gameObject.transform.localPosition = position;

        go.gameObject.transform.localScale = scale;

        return go;
    }

    GameObject InstantiateMenu(GameObject prefab)
    {

        GameObject go;

        go = Instantiate(prefab);

        go.SetActive(false);

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

    public void Hide()
    {
        foreach (GameObject go in menuHandList) go.SetActive(false);
    }

    void Show()
    {
        MainMenu.SetActive(true);
        ActiveMenu = 0;
    }

}
