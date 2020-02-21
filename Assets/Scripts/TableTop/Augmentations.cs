using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop {

    public enum AUGMENTOPTIONS
    {
        sphere = 0,
        circle = 1
    }

    public class Augmentations : Singleton<Augmentations>
    {
        //public 

        public bool AugmentMouseLocationOnMap = true;

        public AUGMENTOPTIONS option = AUGMENTOPTIONS.sphere;
        
        //private

        private Camera mainCam;

        private Vector4 TableTopSize;

        private Vector3? MouseLocationOnMap;

        private BoxCollider MapCollider;

        private Ray ray;

        private RaycastHit hit;

        //methods

        void Start()
        {
            mainCam = Camera.main;

            TableTopSize = Boundaries.Instance.slippyMapSize;

            MapCollider = Map.Instance.gameObject.GetComponent<BoxCollider>();
          
        }

        void Update()
        {
            if (AugmentMouseLocationOnMap)
            {
                MouseLocationOnMap = GetMousePoint();

                switch (option) {

                    case AUGMENTOPTIONS.sphere:

                        SphereAugmentation(MouseLocationOnMap);

                        break;

                }
            }
        }

        private Nullable<Vector3> GetMousePoint()
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);           

            if (MapCollider.Raycast(ray, out hit, 100f))
            {

                if (hit.point.x < TableTopSize.x || hit.point.x > TableTopSize.z || hit.point.z < TableTopSize.y || hit.point.z > TableTopSize.w)
                {

                    return null;
                }

                return hit.point;

            }

            return null;
        }

        //Agumentation Sphere
        private AugmentationSphere ASphere;
        
        private void SphereAugmentation(Vector3? pointOnMap) {

            if (ASphere == null) GetAugmentationSphere();

            if (pointOnMap == null) {


                ASphere.HideSphere();

                return;

            }
            else
            {

                Vector3 pointOnMapsafe = (Vector3)pointOnMap;

                ASphere.UpdateSphereLocation(pointOnMapsafe);

                return;

            }

        }

        private void GetAugmentationSphere()
        {

            ASphere = AugmentationSphere.Instance;
        }


    }
}

