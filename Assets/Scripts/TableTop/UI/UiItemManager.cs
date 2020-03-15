using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiItemManager : MonoBehaviour
{

    private int _itemNumber;

    public int itemNumber
    {

        get { return _itemNumber; }
        set
        {
            _itemNumber = value;
            setPos();
        }

    }

    public float startingValue = 0.258f;

    public float height = 0.05f;


    //other methods
    public void setValue(string ValueName, string value)
    {
        TextMesh valueToSet = getTextMeshChildByName(ValueName);

        valueToSet.text = value;

    }

    private TextMesh getTextMeshChildByName(string name)
    {

        TextMesh[] TextMeshes = this.gameObject.GetComponentsInChildren<TextMesh>();

        TextMesh textMesh = null;

        foreach (TextMesh tm in TextMeshes)
        {

            if (tm.name == name)
            {

                textMesh = tm;

                break;
            }

        }

        return textMesh;

    }

    private void setPos()
    {
        float newValue = startingValue - (_itemNumber * height);

        this.gameObject.transform.localPosition = new Vector3(0f, 0f, newValue);

    }

    public string transformSecondsToTime(int secondsValue)
    {

        var Seconds = (int)secondsValue % 60;

        var Minutes = (int)(secondsValue / 60) % 60;

        var Hours = (int)((secondsValue / 60) / 60);

        string time = Hours.ToString() + ":" + Minutes.ToString() + ":" + Seconds.ToString();

        return time;
    }

}
