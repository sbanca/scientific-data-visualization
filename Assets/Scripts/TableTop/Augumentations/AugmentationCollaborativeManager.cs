using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop {
    public class AugmentationCollaborativeManager : MonoBehaviour
    {
        [SerializeField]
        public bool ColorDistance = false;

        //Agumentation Sphere
        [SerializeField]
        public AugmentationSphere Sphere;
        [SerializeField]
        public AugmentationSphere RemoteSphere;

        //Agumentation Cone
        [SerializeField]
        public AugmentationLightProjector Cone;
        [SerializeField]
        public AugmentationLightProjector RemoteCone;

        private float? distance;

        public Color a;
        public Color b;
        private Color r;

        
        [SerializeField]
        public float maxDistance;
        [SerializeField]
        public float minDistance;



        private void Update()
        {
            distanceComputation();

            if (ColorDistance) colorDistance();

        }

        void colorDistance() {

            if (distance != null) { 

                float value = remap((float)distance, maxDistance, minDistance, 0, 1);

                r = Color.Lerp(a, b, value);

                Sphere.ChanegMaterialColor(r);
                RemoteSphere.ChanegMaterialColor(r);
                Cone.ChanegMaterialColor(r);
                RemoteCone.ChanegMaterialColor(r);
            }
            else {


                Sphere.ChanegMaterialColor(b);
                RemoteSphere.ChanegMaterialColor(b);
                Cone.ChanegMaterialColor(b);
                RemoteCone.ChanegMaterialColor(b);
            }
        }

        void distanceComputation()
        {
            Vector3? pos1 = Sphere.Position();
            Vector3? pos2 = RemoteSphere.Position();

            if (pos1 != null & pos2 != null) distance = Vector3.Distance((Vector3)pos1, (Vector3)pos2);

            else distance = null;

        }

        float remap( float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

    }
}
