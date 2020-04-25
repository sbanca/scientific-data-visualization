using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   
    void Start()
    {
        Button[] btnlist = gameObject.GetComponentsInChildren<Button>();

        int i = 0;


        GameObject.Find("Data Download").GetComponentInChildren<Button>().onClick.AddListener(delegate { 

            Menu.Instance.switchActiveMenu(1); 
        
        });

        GameObject.Find("Debug Console").GetComponentInChildren<Button>().onClick.AddListener(delegate
        {

            Menu.Instance.ConsoleLog.gameObject.GetComponentInChildren<MoveAround>().Place();

            Menu.Instance.ConsoleLog.gameObject.SetActive(true);

            Menu.Instance.Hide();

        });

        GameObject.Find("Performances").GetComponentInChildren<Button>().onClick.AddListener(delegate {

            Menu.Instance.Performances.gameObject.GetComponentInChildren<MoveAround>().Place();

            Menu.Instance.Performances.gameObject.SetActive(true);

            Menu.Instance.Hide();

        });

    }

   
}
