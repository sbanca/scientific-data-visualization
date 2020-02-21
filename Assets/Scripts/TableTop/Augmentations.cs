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

        private Vector3? MouseLocationOnMap;

        private RayOnMap rayOnMap;

        //methods

        void Start()
        {
            rayOnMap = RayOnMap.Instance;
          
        }

        void Update()
        {
            if (AugmentMouseLocationOnMap)
            {
                MouseLocationOnMap = rayOnMap.MouseRay();

                switch (option) {

                    case AUGMENTOPTIONS.sphere:

                        SphereAugmentation(MouseLocationOnMap);

                        break;

                }
            }
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

