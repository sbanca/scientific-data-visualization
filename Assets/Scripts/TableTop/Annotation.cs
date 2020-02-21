using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class Annotation : Singleton<Annotation>
    {
      

        public GameObject AnnotationPrefab;

        List<GameObject> Alist = new List<GameObject>();


        public void SpawnAnnotation(Vector3 LocalPosition)
        {

            GameObject Annotation = Instantiate(AnnotationPrefab);

            Annotation.transform.position = LocalPosition;

            Annotation.transform.parent = Map.Instance.gameObject.transform;

            Alist.Add(Annotation);

        }

        public void DeleteAllAnnotations()
        {

            foreach (GameObject A in Alist) Destroy(A);

        }
    }
}
