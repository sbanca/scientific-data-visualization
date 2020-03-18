using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class RayOnMap : Singleton<RayOnMap>
    {

        private Vector4 TableTopSize;

        private Camera MainCam;

        private BoxCollider MapCollider;

        private RaycastHit hit;

        private Ray ray;
        

        private void Start()
        {

            MapCollider = Map.Instance.gameObject.GetComponent<BoxCollider>();

            MainCam = Camera.main;

            TableTopSize = Boundaries.Instance.slippyMapSize;

        }

        public Nullable<Vector3> MouseRay()
        {
            ray = MainCam.ScreenPointToRay(Input.mousePosition);

            if (MapCollider.Raycast(ray, out hit, 200f))
            {

                if (hit.point.x < TableTopSize.x || hit.point.x > TableTopSize.z || hit.point.z < TableTopSize.y || hit.point.z > TableTopSize.w)
                {

                    return null;
                }

                return hit.point;

            }

            return null;
        }

        public Nullable<Vector3> HeadRay() 
        {

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (MapCollider.Raycast(ray, out hit, 200f))
            {

                if (hit.point.x < TableTopSize.x || hit.point.x > TableTopSize.z || hit.point.z < TableTopSize.y || hit.point.z > TableTopSize.w)
                {

                    return null;
                }

                return hit.point;

            }

            return null;

        }


    }
}
