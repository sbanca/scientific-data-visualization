using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace TableTop
{


    [CustomEditor(typeof(Panels))]
    public class PanelsEditor : UnityEditor.Editor
    {

        
        private Panels panels;

        void OnEnable()
        {
            this.panels = (Panels)target;


        }

        [MenuItem("Examples/Editor GUILayout Popup usage")]

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("PanelPrefab"), true);

            if (GUILayout.Button("Generate Panels"))
            {
                panels.GeneratePanels();
            }

            if (GUILayout.Button("Delete Panels"))
            {
                panels.DeleteAll();
            }

            serializedObject.ApplyModifiedProperties();

        }
    }


}
#endif