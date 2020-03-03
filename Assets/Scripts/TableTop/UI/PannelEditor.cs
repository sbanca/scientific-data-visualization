using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace TableTop
{


    [CustomEditor(typeof(Pannel))]
    public class PannelEditor : UnityEditor.Editor
    {


        private Pannel pannel;

        void OnEnable()
        {
            this.pannel = (Pannel)target;
        }

        
        public override void OnInspectorGUI()
        {

            serializedObject.Update();

          

            EditorGUILayout.PropertyField(serializedObject.FindProperty("pannelTasks"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("PannelItemPrefab"), true);

            if (GUILayout.Button("generate "))
            {
                pannel.Relayout();
            }


            if (GUILayout.Button("delete "))
            {
                pannel.DeletePannelsItems();
            }

            serializedObject.ApplyModifiedProperties();

        }
    }


}
#endif