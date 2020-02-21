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

            AnnotationPrefab = Resources.Load("Prefabs/annotation_prefab", typeof(GameObject)) as GameObject;

            GameObject Annotation = Instantiate(AnnotationPrefab);

            LocalPosition.y = getBuildingsElevation(LocalPosition);

            Annotation.transform.position = LocalPosition;

            Annotation.transform.parent = Map.Instance.gameObject.transform;

            Alist.Add(Annotation);

        }

        public float getBuildingsElevation(Vector3 LocalPosition) {

            float elevation = 0f;

            var buildings = GameObject.Find("Buildings");

            var childnumber = buildings.transform.childCount;

            for (int x=0; x<childnumber; x++) {

                Transform child = buildings.transform.GetChild(x);

                MeshRenderer r = child.GetComponent<MeshRenderer>();

                if (r.bounds.Contains(LocalPosition)){

                    MeshCollider collider = child.gameObject.AddComponent<MeshCollider>();

                    RaycastHit hit = new RaycastHit();

                    Vector3 startingPoint = LocalPosition + (Vector3.up * 2); //move up the point 


                    //this is to cast rays around to make sure surrunding buidlings or small sapce do not obscure the object 

                    var ray = new Ray(startingPoint, Vector3.down);

                    if (collider.Raycast(ray, out hit,200f)) { 

                        elevation = hit.point.y;

                        Object.Destroy(collider);

                        break;
                    }

                    Object.Destroy(collider);
                }
            }

            return elevation;

        }

        public void DeleteAllAnnotations()
        {

            foreach (GameObject A in Alist) Destroy(A);

        }
    }
}
