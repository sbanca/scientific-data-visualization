using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnnotations : Singleton<TableTopMapCoordinates>
{
    public TableTopMapNavigation parent;

    public GameObject AnnotationPrefab;

    List<GameObject> Alist = new List<GameObject>();


    public void SpawnAnnotation(Vector3 LocalPosition) {

        GameObject Annotation = Instantiate(AnnotationPrefab);

        Annotation.transform.position = LocalPosition;

        Annotation.transform.parent = parent.gameObject.transform;

        Alist.Add(Annotation);

    }

    public void DeleteAllAnnotations() {

        foreach (GameObject A in Alist) Destroy(A);
        
    }

}
