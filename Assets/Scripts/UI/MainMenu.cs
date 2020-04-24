using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   
    void Start()
    {
        Button[] btnlist = gameObject.GetComponentsInChildren<Button>();

        int i = 0;

        foreach (Button btn in btnlist) {

            btn.onClick.AddListener(delegate { Menu.Instance.switchActiveMenu(i); });

            i=+1;
        }
    }

   
}
