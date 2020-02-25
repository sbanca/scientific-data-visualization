﻿using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace TableTop
{


    [CustomEditor(typeof(Pannels))]
    public class PannelsEditor : UnityEditor.Editor
    {

        
        private Pannels pannels;

        void OnEnable()
        {
            this.pannels = (Pannels)target;


        }

        [MenuItem("Examples/Editor GUILayout Popup usage")]

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("PannelPrefab"), true);

            if (GUILayout.Button("generate "))
            {
                pannels.GeneratePanels();
            }

            if (GUILayout.Button("delete data "))
            {
                pannels.DeleteData();
            }

            serializedObject.ApplyModifiedProperties();

        }
    }


}
#endif