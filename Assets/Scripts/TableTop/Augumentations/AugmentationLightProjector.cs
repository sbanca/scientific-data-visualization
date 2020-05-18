using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class AugmentationLightProjector : MonoBehaviour
    {
        private GameObject LightProjector;

        private GameObject LightProjectorPrefab;

        [SerializeField]
        private Transform _parent;

        public Transform Parent {

            get { return _parent; }
            set {

                _parent = value;

                LightProjector.transform.parent = _parent;

                LightProjector.transform.localPosition = new Vector3(0f,0f,0f) ;
                LightProjector.transform.localRotation = Quaternion.identity;
            }
        }


        private Renderer[] rl;

        private Material m;

        void Start()
        {
            CreateLightProjector();
            HideLightProjector();
        }

        private void CreateLightProjector()
        {

            if (LightProjectorPrefab == null) GetLightProjectorPrefab();

            LightProjector = Instantiate(LightProjectorPrefab,_parent);

            rl = LightProjector.GetComponentsInChildren<Renderer>();

            foreach (Renderer r in rl) r.material = m;

        }

        public void UpdateLightProjectorLocation()
        {

            if (LightProjector == null) CreateLightProjector();

            if (LightProjector.activeSelf == false) ShowLightProjector();


        }

        public void HideLightProjector()
        {

            if (LightProjector != null) LightProjector.SetActive(false);

        }

        private void ShowLightProjector()
        {

            if (LightProjector != null) LightProjector.SetActive(true);

        }

        public void GetLightProjectorPrefab()
        {

            LightProjectorPrefab = Resources.Load("Prefabs/cone", typeof(GameObject)) as GameObject;
            m = Resources.Load("Materials/highlightVolume", typeof(Material)) as Material; 
        }


        public void ChanegMaterialColor(Color c)
        {

            m.color = new Color(c.r, c.g, c.b, 1f);

        }
    }

}

