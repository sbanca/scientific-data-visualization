using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace TableTop
{


    [CustomEditor(typeof(SpatialAnchors))]
    public class SpatialAnchorsEditor : UnityEditor.Editor
    {


        private SpatialAnchors spatialAnchors;

        void OnEnable()
        {
            this.spatialAnchors = (SpatialAnchors)target;

            spatialAnchors.loadPrefabs();
        }


        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("spatialAnchorsList"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("ElectronicShopIconPrefeab"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("HotelIconPrefeab"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("PrintShopIconPrefeab"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("RestaurantIconPrefeab"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("WorkMeetingPrefeab"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultIconPrefab"), true);

            if (GUILayout.Button("generate "))
            {
                spatialAnchors.GenerateSpatialAnchors();
            }


            if (GUILayout.Button("DeleteALL "))
            {
                spatialAnchors.DeleteAll();
            }

            serializedObject.ApplyModifiedProperties();

        }
    }


}
#endif