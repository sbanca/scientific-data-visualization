using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace TableTop
{


    [CustomEditor(typeof(Pannels))]
    public class PannelsEditor : UnityEditor.Editor
    {
  
        [MenuItem("Examples/Editor GUILayout Popup usage")]

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("PannelPrefab"), true);

            if (GUILayout.Button("generate "))
            {
                Pannels.Instance.GeneratePanels();
            }

            serializedObject.ApplyModifiedProperties();

        }
    }


}
#endif