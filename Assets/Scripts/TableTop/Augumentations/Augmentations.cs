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
        [SerializeField]
        public bool manualInitialize = false;

        [SerializeField]
        public bool AugmentMouseLocationOnMap = true;

        [SerializeField]
        public AUGMENTOPTIONS option = AUGMENTOPTIONS.sphere;

        [SerializeField]
        public AUGMENTINPUT optionInput = AUGMENTINPUT.mouse;

   
        public RayOnMap RayOnMap
        {
            get { return _rayOnMap; }
            set { _rayOnMap = value; }
        }

        //private

        private Vector3? PointOnMap;

        private Vector3? RemotePointOnMap;

        [SerializeField]
        private RayOnMap _rayOnMap;


        //methods

        void Start()
        {
            if (!manualInitialize)
            {

                _rayOnMap = RayOnMap.Instance;

            }  
          
        }

        void Update()
        {
            if (AugmentMouseLocationOnMap)
            {


                switch (optionInput)
                {

                    case AUGMENTINPUT.mouse:

                        PointOnMap = _rayOnMap.MouseRay();

                        break;


                    case AUGMENTINPUT.head:

                        PointOnMap = _rayOnMap.HeadRay();

                        RemotePointOnMap = _rayOnMap.RemoteHeadRay();

                        break;

                }

                
                switch (option) {

                    case AUGMENTOPTIONS.sphere:

                        SphereAugmentation(Sphere,PointOnMap);

                        SphereAugmentation(RemoteSphere,RemotePointOnMap);

                        break;


                    case AUGMENTOPTIONS.circle:

                        CircleAugmentation(PointOnMap);

                        CircleAugmentation(RemotePointOnMap);

                        break;


                    case AUGMENTOPTIONS.lightProjector:

                        LightProjectorAugmentation(PointOnMap);

                        LightProjectorAugmentation(RemotePointOnMap);

                        break;

                }
            }
        }



        //Agumentation Sphere
        private AugmentationSphere Sphere;
        private AugmentationSphere RemoteSphere;

        private void SphereAugmentation(AugmentationSphere sphere, Vector3? pointOnMap) {

            if (sphere == null)
            {

                CreateAugmentationSpheres();

                return;

            }
            else if (pointOnMap == null) {

                sphere.HideSphere();

                return;

            }
            else
            {

                Vector3 pointOnMapsafe = (Vector3)pointOnMap;

                sphere.UpdateSphereLocation(pointOnMapsafe);

                return;

            }

        }

        private void CreateAugmentationSpheres()
        {

            Sphere = gameObject.AddComponent<AugmentationSphere>();
            RemoteSphere = gameObject.AddComponent<AugmentationSphere>();

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

