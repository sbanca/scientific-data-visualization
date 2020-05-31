using TMPro;
using UnityEngine;

public class clockControl : MonoBehaviour
{


    float avaliableTime = 300;

    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        

        UpdateTime();
    }

    // Update is called once per frame
    void Update()
    {
        avaliableTime -= Time.unscaledDeltaTime;

        UpdateTime();

        transform.LookAt(Camera.main.transform, Vector3.up);
    }

    void UpdateTime() {


        var min = Mathf.Floor(avaliableTime / 60);
        var sec = Mathf.RoundToInt((avaliableTime) % 60);

        text.text = min + ":" + sec;


    }
}
