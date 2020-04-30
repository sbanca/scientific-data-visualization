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

        if (GUILayout.Button("Export to GLB"))
        {
           
        }
    }
} 

#endif