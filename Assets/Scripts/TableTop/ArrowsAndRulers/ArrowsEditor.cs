using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR

namespace TableTop
{


    [CustomEditor(typeof(Arrows))]
    public class ArrowsEditor : UnityEditor.Editor
    {
        private Arrows arrows;


        [MenuItem("Examples/Editor GUILayout Popup usage")]

        void OnEnable()
        {
            this.arrows = (Arrows)target;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("arrowdistance"), true);

            if (GUILayout.Button("Generate "))
            {
                arrows.CreateArrows();
            }


            if (GUILayout.Button("Delete "))
            {
                arrows.DeleteArrows();
            }



            serializedObject.ApplyModifiedProperties();

        }
    }
}

#endif