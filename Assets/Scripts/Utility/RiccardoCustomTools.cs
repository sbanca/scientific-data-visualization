#if UNITY_EDITOR
     
   
using UnityEditor;
using UnityEngine;


public class RiccardoCustomTools : EditorWindow
{
    [MenuItem("RiccardoCustomTools/CombineMesh")]
    public static void OpenWindow()
    {
        GetWindow<RiccardoCustomTools>();
    }

    void OnEnable()
    {
        
    }

    void OnGUI()
    {

        if (GUILayout.Button("CombineSelectedMesh"))
        {
            GameObject SelectedObject = Selection.activeObject as GameObject;

            MeshCombiner.Instance.CombineMesh(SelectedObject);

        }

        if (GUILayout.Button("CombineMeshSimple"))
        {
            GameObject SelectedObject = Selection.activeObject as GameObject;

            MeshCombiner.Instance.CombineMeshSimple(SelectedObject);


        }

        if (GUILayout.Button("AddSubMesh"))
        {
            GameObject SelectedObject = Selection.activeObject as GameObject;

            MeshCombiner.Instance.AddSubMesh(SelectedObject);
        }

        if (GUILayout.Button("CountSubMesh"))
        {
            GameObject SelectedObject = Selection.activeObject as GameObject;

            MeshCombiner.Instance.CheckSubMeshCount(SelectedObject);
        }

        if (GUILayout.Button("RemoveOffset"))
        {
            GameObject SelectedObject = Selection.activeObject as GameObject;

            MeshCombiner.Instance.RemoveOffset(SelectedObject);
        }

        if (GUILayout.Button("Move"))
        {
            GameObject SelectedObject = Selection.activeObject as GameObject;

            MeshCombiner.Instance.MoveMeshForward(SelectedObject);
        }
    }
} 

#endif