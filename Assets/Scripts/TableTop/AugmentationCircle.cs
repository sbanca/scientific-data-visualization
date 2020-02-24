using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class AugmentationCircle : Singleton<AugmentationCircle>
    {
        private GameObject Circle;

        private GameObject CirclePrefab;
        void Start()
        {
            CreateCircle();
            HideCircle();
        }

        private void CreateCircle()
        {

            if (CirclePrefab == null) GetCirclePrefab();

             Circle = Instantiate(CirclePrefab);

        }

        public void UpdateCircleLocation(Vector3 newPosition)
        {

            if (Circle == null) CreateCircle();

            if (Circle.activeSelf == false) ShowCircle();

            Circle.transform.position = newPosition;

        }

        public void HideCircle()
        {

            if (Circle != null) Circle.SetActive(false);

        }

        private void ShowCircle()
        {

            if (Circle != null) Circle.SetActive(true);

        }

        public void GetCirclePrefab()
        {

            CirclePrefab = Resources.Load("Prefabs/circle_prefab", typeof(GameObject)) as GameObject;

        }
    }

}

