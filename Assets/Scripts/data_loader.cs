using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_loader : MonoBehaviour
{

    [SerializeField]
    public GameObject[] dataprefabs;

    GameObject currentData;

    private Queue<GameObject> dataPrefabsQueue;


    private void Start()
    {
        dataPrefabsQueue = new Queue<GameObject>();

        foreach(GameObject g in dataprefabs) dataPrefabsQueue.Enqueue(g);
    }

    public void LoadNext() {

        if (!this.enabled) return;

        if(currentData!=null) Destroy(currentData);

        if (dataPrefabsQueue.Count > 0) {
            
            currentData = Instantiate(dataPrefabsQueue.Dequeue());

            if (!currentData.activeSelf) currentData.SetActive(true);

            IgnoreCollistion();

        }

    }

    public void IgnoreCollistion() { 

            CharacterController charCon = FindObjectOfType<CharacterController>();

            Collider[] colliders = currentData.GetComponentsInChildren<Collider>();

            foreach (Collider c in colliders) Physics.IgnoreCollision(charCon, c);


    }

}      


