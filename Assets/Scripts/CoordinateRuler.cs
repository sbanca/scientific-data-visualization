using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoordinateRuler : MonoBehaviour
{

    [SerializeField]

    private Vector2 rangeticks = new Vector2(0f, 1f);

    public Vector2 Rangeticks {
        
        get { return rangeticks; }
        set {
            if (rangeticks != value)
            {
                createticks();
                rangeticks = value;
            }
        }
    
    }

    [SerializeField]

    private int ticksnumber = 5;
    public int Ticksnumber
    {

        get { return ticksnumber; }
        set
        {
            if (ticksnumber != value)
            {
                createticks();
                ticksnumber = value;
            }
        }

    }

    [SerializeField]

    private float length = 0f;

    public float Length
    {
        get { return length; }
        set
        {
            if (length != value)
            {
                gameObject.transform.localScale.Set(0f,0f,value);
                length = value;
            }
        }
    }

    private List<GameObject> ticks;

    private void createticks() { 
        


    }
}
