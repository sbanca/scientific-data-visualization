using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class AugmentationLightProjector : Singleton<AugmentationLightProjector>
    {
        private GameObject LightProjector;

        private GameObject LightProjectorPrefab;

        private Vector3 offsety = new Vector3 (0f,1f,0f); //projector nbeeds to be placed above the map
        void Start()
        {
            CreateLightProjector();
            HideLightProjector();
        }

        private void CreateLightProjector()
        {

            if (LightProjectorPrefab == null) GetLightProjectorPrefab();

            LightProjector = Instantiate(LightProjectorPrefab);

        }

        public void UpdateLightProjectorLocation(Vector3 newPosition)
        {

            if (LightProjector == null) CreateLightProjector();

            if (LightProjector.activeSelf == false) ShowLightProjector();

            LightProjector.transform.position = newPosition + offsety;

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

            LightProjectorPrefab = Resources.Load("Prefabs/LightProjector", typeof(GameObject)) as GameObject;

        }
    }

}

