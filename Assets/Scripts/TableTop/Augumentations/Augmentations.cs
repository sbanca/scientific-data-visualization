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

        [SerializeField]
        public bool sphereAugmentation = true;

        [SerializeField]
        public bool coneAugmentation = true;

        [SerializeField]
        public bool circleAugmentation = true;


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


                if (sphereAugmentation)
                {
                    SphereAugmentation(Sphere,PointOnMap);

                    SphereAugmentation(RemoteSphere,RemotePointOnMap);

                }

                if (circleAugmentation)
                {
                    CircleAugmentation(Circle,PointOnMap);

                    CircleAugmentation(RemoteCircle,RemotePointOnMap);

                }

                if (coneAugmentation)
                {
                    LightProjectorAugmentation(LightProjector,PointOnMap);

                    LightProjectorAugmentation(RemoteLightProjector,RemotePointOnMap);

                }
    
                
            }
        }



        //Agumentation Sphere
        [SerializeField]
        public AugmentationSphere Sphere;
        [SerializeField]
        public AugmentationSphere RemoteSphere;

        private void SphereAugmentation(AugmentationSphere asphere, Vector3? pointOnMap) {

            if (asphere == null)
            {

                CreateAugmentationSpheres();

                return;

            }
            else if (pointOnMap == null) {

                asphere.HideSphere();

                return;

            }
            else
            {

                Vector3 pointOnMapsafe = (Vector3)pointOnMap;

                asphere.UpdateSphereLocation(pointOnMapsafe);

                return;

            }

        }

        private void CreateAugmentationSpheres()
        {

            Sphere = gameObject.AddComponent<AugmentationSphere>();
            RemoteSphere = gameObject.AddComponent<AugmentationSphere>();

        }

        //Agumentation Sphere
        [SerializeField]
        private AugmentationCircle Circle;
        [SerializeField]
        private AugmentationCircle RemoteCircle;

        private void CircleAugmentation(AugmentationCircle acircle, Vector3? pointOnMap)
        {

            if (acircle == null) {
                
                GreateAugmentationCircle();

                return;

            }

            if (pointOnMap == null)
            {


                acircle.HideCircle();

                return;

            }
            else
            {

                Vector3 pointOnMapsafe = (Vector3)pointOnMap;

                acircle.UpdateCircleLocation(pointOnMapsafe);

                return;

            }

        }

        private void GreateAugmentationCircle()
        {

            Circle = gameObject.AddComponent<AugmentationCircle>();
            RemoteCircle = gameObject.AddComponent<AugmentationCircle>();
        }

        //Agumentation Sphere
        [SerializeField]
        public AugmentationLightProjector LightProjector;
        [SerializeField]
        public AugmentationLightProjector RemoteLightProjector;

        private void LightProjectorAugmentation(AugmentationLightProjector ALightProjector, Vector3? pointOnMap)
        {

            if (ALightProjector == null) { 
            
                GetLightProjector();

                return;
            
            }

            if (ALightProjector.Parent == null) {

                SetLightProjectorParents();

                return;
            }

            if (pointOnMap == null)
            {

                ALightProjector.HideLightProjector();

                return;

            }
            else
            {

                ALightProjector.UpdateLightProjectorLocation();

                return;

            }

        }

        private void GetLightProjector()
        {

            LightProjector = gameObject.AddComponent<AugmentationLightProjector>();
         
            RemoteLightProjector = gameObject.AddComponent<AugmentationLightProjector>();

            SetLightProjectorParents();


        }

        private void SetLightProjectorParents() {

            LightProjector.Parent = _rayOnMap.MainCam.transform;

            RemoteLightProjector.Parent = _rayOnMap.RemoteHead;
        }

    }
}

