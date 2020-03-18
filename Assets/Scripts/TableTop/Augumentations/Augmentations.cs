using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop {

    public enum AUGMENTOPTIONS
    {
        sphere = 0,
        circle = 1,
        lightProjector=2
    }

    public enum AUGMENTINPUT
    {
        head = 0,
        mouse = 1
    }

    public class Augmentations : Singleton<Augmentations>
    {
        //public 

        public bool AugmentMouseLocationOnMap = true;

        public AUGMENTOPTIONS option = AUGMENTOPTIONS.sphere;

        public AUGMENTINPUT optionInput = AUGMENTINPUT.mouse;

        //private

        private Vector3? PointOnMap;

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


                switch (optionInput)
                {

                    case AUGMENTINPUT.mouse:

                        PointOnMap = rayOnMap.MouseRay();


                        break;

                    case AUGMENTINPUT.head:

                        PointOnMap = rayOnMap.HeadRay();


                        break;

                }

                
                switch (option) {

                    case AUGMENTOPTIONS.sphere:

                        SphereAugmentation(PointOnMap);

                        break;

                    case AUGMENTOPTIONS.circle:

                        CircleAugmentation(PointOnMap);

                        break;

                    case AUGMENTOPTIONS.lightProjector:

                        LightProjectorAugmentation(PointOnMap);

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

        //Agumentation Sphere
        private AugmentationCircle ACircle;

        private void CircleAugmentation(Vector3? pointOnMap)
        {

            if (ACircle == null) GetAugmentationCircle();

            if (pointOnMap == null)
            {


                ACircle.HideCircle();

                return;

            }
            else
            {

                Vector3 pointOnMapsafe = (Vector3)pointOnMap;

                ACircle.UpdateCircleLocation(pointOnMapsafe);

                return;

            }

        }

        private void GetAugmentationCircle()
        {

            ACircle = AugmentationCircle.Instance;
        }

        //Agumentation Sphere
        private AugmentationLightProjector ALightProjector;

        private void LightProjectorAugmentation(Vector3? pointOnMap)
        {

            if (ALightProjector == null) GetLightProjector();

            if (pointOnMap == null)
            {


                ALightProjector.HideLightProjector();

                return;

            }
            else
            {

                Vector3 pointOnMapsafe = (Vector3)pointOnMap;

                ALightProjector.UpdateLightProjectorLocation(pointOnMapsafe);

                return;

            }

        }

        private void GetLightProjector()
        {

            ALightProjector = AugmentationLightProjector.Instance;
        }

    }
}

