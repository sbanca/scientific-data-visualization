using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    GameObject cube;
    // Start is called before the first frame update
    //void Start()
    //{
       

    //    Debug.Log(gameObject.transform.position);

    //    Debug.Log(gameObject.transform.localToWorldMatrix * gameObject.transform.localPosition);

    //    Debug.Log(gameObject.transform.eulerAngles);

    //}

    //Update is called once per frame
    void Update()
    {
        if (cube == null)
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.position = gameObject.transform.position;


    }
}
