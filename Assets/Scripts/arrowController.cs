using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowController : MonoBehaviour
{
    Material mat;
    Color target = Color.red;
    Color orgcol;

    void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        orgcol = mat.color;

    }

    void OnMouseEnter()
    {
        mat.color = target;
    }

    void OnMouseExit()
    {
        mat.color = orgcol;
    }

    void OnMouseUpAsButton()
    {
        Destroy(this.gameObject);
    }



}

