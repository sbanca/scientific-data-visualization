using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class labels_Management_network : MonoBehaviour
{
    public GameObject prefab;

    public bool UseMesh;

    [serializable]
    public GameObject  labelmeshprefab ;


    // Start is called before the first frame update
    void Start()
    {


        if (UseMesh) usemesh();
        else createLabels();

    }

    // Update is called once per frame
    void Update()
    {

        foreach (Transform child in transform)
        {
            child.LookAt(Camera.main.transform, Vector3.up);

        }

    }

    void createLabels() {

        TextAsset fileData = Resources.Load<TextAsset>("Prefabs/3D/new_network/grasshopper_labels");

        string[] lines = fileData.text.Split('\n');

        Vector3 remapaxes = new Vector3(-1f, 0f, -1f);

        foreach (string l in lines)
        {

            string[] lineData = (l.Split(','));

            GameObject Label = Instantiate(prefab);

            Label.name = lineData[0];

            Vector3 v = Vector3.zero;

            float.TryParse(lineData[1], out v.x);
            float.TryParse(lineData[2], out v.z);
            float.TryParse(lineData[3], out v.y);

            v = new Vector3(-v.x, v.y, -v.z);

            Label.transform.position = transform.localToWorldMatrix * v;

            Label.transform.position += transform.position;

            Label.transform.parent = transform;

            TextMeshPro text = Label.GetComponent<TextMeshPro>();

            text.text = lineData[0];

            float fontsize;

            float.TryParse(lineData[4], out fontsize);

            text.fontSize = fontsize;

            Label.transform.localScale = new Vector3(-30f, 30f, 30f);
        }

    }

    void usemesh() {

        
        GameObject g = Instantiate(labelmeshprefab);

        MeshCombiner.Instance.RemoveOffset(g);


        MeshFilter[] filters = g.GetComponentsInChildren<MeshFilter>();

        int i = 0;

        foreach (MeshFilter f in filters)
        {

            f.transform.position = transform.parent.transform.localToWorldMatrix * f.transform.position;

            f.transform.position += transform.parent.transform.position;

            f.transform.rotation = transform.parent.transform.rotation;

            f.transform.localScale = transform.parent.transform.localScale;

            f.transform.parent = transform;

            //f.transform.position += transform.parent.transform.position;


        }

    }



    


}
