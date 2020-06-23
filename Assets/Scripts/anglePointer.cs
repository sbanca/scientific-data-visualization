using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anglePointer : MonoBehaviour
{
    private void Start()
    {
 
        CharacterController charCon = FindObjectOfType<CharacterController>();

        if (charCon == null) return;

        Collider[] colliders = FindObjectsOfType<Collider>();

        foreach (Collider c in colliders) 
            if(c.gameObject.name != "Plane")  Physics.IgnoreCollision(charCon, c);

        
    }
    void Update()
    {

        transform.localEulerAngles = new Vector3(transform.parent.eulerAngles.x * 0.4f, 0f, 0f); 
    }
}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
