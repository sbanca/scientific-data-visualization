using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop { 
    public class AugmentationSphere : MonoBehaviour
    {
        private GameObject Sphere;
        void Start()
        {
            CreateSphere();
            HideSphere();
        }

        private void CreateSphere()
        {

            Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            //scale
            Vector3 scale = Sphere.transform.localScale;
            scale.Set(0.02f, 0.02f, 0.02f);
            Sphere.transform.localScale = scale;

            //material
            var r = Sphere.GetComponent<Renderer>();
            Material m = Resources.Load("Materials/agumentation_white", typeof(Material)) as Material;
            r.material = m;
        }
        
        public void UpdateSphereLocation(Vector3 newPosition)
        {

            if (Sphere == null) CreateSphere();

            if (Sphere.activeSelf == false) ShowSphere();

            Sphere.transform.position = newPosition;

        }
        
        public void HideSphere()
        {

            if (Sphere != null) Sphere.SetActive(false);

        }

        private void ShowSphere()
        {

            if (Sphere != null) Sphere.SetActive(true);

        }
    }

}

