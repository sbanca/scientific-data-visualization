using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : Singleton<MeshCombiner>
{
    public void CombineMesh(GameObject go)
    {

        //if the current object does not have a mesh add it 
        MeshFilter meshFilter = go.transform.GetComponent<MeshFilter>();
        if (meshFilter == null) meshFilter = go.transform.gameObject.AddComponent<MeshFilter>();

        //if the current object does not have a renderer add it 
        MeshRenderer r = go.transform.GetComponent<MeshRenderer>();
        if (r == null) r = go.transform.gameObject.AddComponent<MeshRenderer>();

        //get all mesh in childrens 
        MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
        List<MeshFilter> meshFiltersList = new List<MeshFilter>();

        //remove the current object mesh filter
        foreach (MeshFilter m in meshFilters)
        {

            if (go.gameObject == m.gameObject)
            {

                continue; // skipthe object itfself
            }

            meshFiltersList.Add(m);
        }
        meshFilters = meshFiltersList.ToArray();


        //initialize the mesh and add the submesh count to
        meshFilter.sharedMesh = new Mesh();
        meshFilter.sharedMesh.subMeshCount = meshFilters.Length;

        ///////////
        //materials
        ///////////

        //material Array 
        Material[] materialArray = new Material[meshFilters.Length];
        Dictionary<string, bool> materialCache = new Dictionary<string, bool>();

        //Loop to add materials 
        int i = 0;

        while (i < meshFilters.Length)
        {

            materialArray[i] = meshFilters[i].gameObject.GetComponent<MeshRenderer>().material;

            if (materialCache.ContainsKey(materialArray[i].name)) {

                materialArray[i].name = System.Guid.NewGuid().ToString();
                materialArray[i].mainTexture.name = materialArray[i].name;
                materialCache[materialArray[i].name] = true;

            }
            else { 

                materialCache[materialArray[i].name] = true; 

            }

            i++;

        }

        r.materials = materialArray;

        //////////////////
        //vertices and UV
        //////////////////

        int totalnumberofvertices = 0;
        foreach (MeshFilter m in meshFilters)
        {

            if (gameObject == m.gameObject) continue; // skipthe object itfself

            totalnumberofvertices += m.mesh.vertexCount;
        }

        List<Vector3> newVertexList = new List<Vector3>();
        List<Vector2> newUVList = new List<Vector2>();

        //Loop to add vertices 
        i = 0;
        int shiftLength = 0;
        while (i < meshFilters.Length)
        {

            int j = 0;

            Matrix4x4 localToWorld = meshFilters[i].gameObject.transform.localToWorldMatrix;
            Matrix4x4 WorldToParent = go.transform.worldToLocalMatrix;

            while (j < meshFilters[i].mesh.vertexCount)
            {
                Vector3 transformedVertex = localToWorld.MultiplyPoint3x4(meshFilters[i].mesh.vertices[j]);
                transformedVertex = WorldToParent.MultiplyPoint3x4(transformedVertex);
                newVertexList.Add(transformedVertex);
                newUVList.Add(meshFilters[i].mesh.uv[j]);
                j++;
            }

            i++;
        }

        meshFilter.sharedMesh.SetVertices(newVertexList);
        meshFilter.sharedMesh.SetUVs(0, newUVList);

        ///////////
        //triangles
        ///////////

        i = 0;
        shiftLength = 0;
        while (i < meshFilters.Length)
        {
            //shift
            int addtoshift = i == 0 ? 0 : meshFilters[i - 1].sharedMesh.vertexCount;
            shiftLength += addtoshift;

            int[] shiftedTriangles = meshFilters[i].sharedMesh.triangles;

            for (int j = 0; j < shiftedTriangles.Length; j++)
            {

                shiftedTriangles[j] = shiftedTriangles[j] + shiftLength;

            }

            meshFilter.sharedMesh.SetTriangles(shiftedTriangles, i);

            meshFilters[i].gameObject.SetActive(false);

            i++;
        }


        meshFilter.sharedMesh.RecalculateNormals();

        

    }
}
