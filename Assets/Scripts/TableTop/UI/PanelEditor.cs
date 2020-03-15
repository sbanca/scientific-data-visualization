using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace TableTop
{


    [CustomEditor(typeof(Panel))]
    public class PanelEditor : UnityEditor.Editor
    {


        private Panel panel;

        void OnEnable()
        {
            this.panel = (Panel)target;
        }

        
        public override void OnInspectorGUI()
        {

            serializedObject.Update();

          

            EditorGUILayout.PropertyField(serializedObject.FindProperty("panelTasks"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("TaskUiItemPrefab"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("RouteUiItemPrefab"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("MetricsUiItemPrefab"), true);

            if (GUILayout.Button("generate "))
            {
                panel.Relayout();
            }


            if (GUILayout.Button("delete "))
            {
                panel.DeletePanelsItems();
            }

            serializedObject.ApplyModifiedProperties();

        }
    }


}
#endif