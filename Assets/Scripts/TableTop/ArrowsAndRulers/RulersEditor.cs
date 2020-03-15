using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR

namespace TableTop
{


    [CustomEditor(typeof(Rulers))]
    public class RulersEditor : UnityEditor.Editor
    {
        private Rulers rulers;


        [MenuItem("Examples/Editor GUILayout Popup usage")]

        void OnEnable()
        {
            this.rulers = (Rulers)target;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();



            if (GUILayout.Button("Generate "))
            {
                rulers.CreateRulers();
            }


            if (GUILayout.Button("Delete "))
            {
                rulers.DeleteRulers();
            }



            serializedObject.ApplyModifiedProperties();

        }
    }
}

#endif