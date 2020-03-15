using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class OrientCoordinatesToCamera : Singleton<OrientCoordinatesToCamera>
    {
        Camera main;
        Vector3 coordinateCurrentDirection= Vector3.back;
        public List<GameObject> objects = new List<GameObject>();


        // Update is called once per frame
        void Update()
        {
            
            var newDirection =new Vector3(Camera.main.transform.forward.x,0f, Camera.main.transform.forward.z).normalized;

            if (newDirection != coordinateCurrentDirection) {

                float angle = Vector3.Angle(coordinateCurrentDirection,newDirection);

                foreach (GameObject g in objects) {

                    var center = g.transform.GetComponent<Renderer>().bounds.center;

                    g.transform.RotateAround(center,Vector3.up,-angle);

                }

                coordinateCurrentDirection = newDirection;

            }

            
        }
    }

}
