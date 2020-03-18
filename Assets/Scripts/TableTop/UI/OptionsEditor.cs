using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace TableTop
{

    [CustomEditor(typeof(Options))]
    public class OptionsEditor : UnityEditor.Editor
    {


        private Options options;

        void OnEnable()
        {
            this.options = (Options)target;
        }


        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("OptionPrefab"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("OptionList"), true);

            if (GUILayout.Button("generate")) this.options.Generate();

            if (GUILayout.Button("delete")) this.options.DeleteAllOptions();


            serializedObject.ApplyModifiedProperties();

        }
    }

}

#endif