﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputsManager : Singleton<inputsManager>
{

        public GameObject localAvatar = null;
        public GameObject remoteAvatar = null;


        //controllers

        [SerializeField]
        private Transform _controller = null;
        public Transform Controller
        {
            get 
            {

                if (_controller == null && localAvatar!=null) 
                {
                    _controller = DeepChildSearch(localAvatar, "hand_right");
            }

                return _controller; 
            }
                 
        }

        [SerializeField]
        private Transform _remoteController = null;
        public Transform RemoteController
        {
            get
            {
                if (_remoteController == null && remoteAvatar != null)
                {
                    _remoteController = DeepChildSearch(remoteAvatar, "hand_right");
            }

                return _remoteController;
            }
          
        }


        //heads
        
        [SerializeField]
        private Transform _localHead;
        public Transform LocalHead
        {
            get
            {
                if (_localHead == null && localAvatar != null)
                {
                    _localHead = DeepChildSearch(localAvatar, "head_JNT");
            }

                return _localHead;
            }

        }

        [SerializeField]
        private Transform _remoteHead = null;
        public Transform RemoteHead
        {
            get
            {
                if (_remoteHead == null && remoteAvatar != null)
                {
                    _remoteHead = DeepChildSearch(remoteAvatar,"head_JNT");
                }

                return _remoteHead;
            }

        }

        //heads tilted

        [SerializeField]
        private Transform _localHeadtilt;
        public Transform LocalHeadTilt
        {
            get
            {
                if (_localHeadtilt == null && localAvatar != null)
                {

                Transform t = DeepChildSearch(localAvatar, "head_JNT");

                var headTilt = new GameObject();

                headTilt.transform.rotation = t.rotation;

                //headTilt.transform.eulerAngles = new Vector3 (headTilt.transform.eulerAngles.x+ 18f, headTilt.transform.eulerAngles.y, headTilt.transform.eulerAngles.z);

                headTilt.transform.position = t.position;

                headTilt.transform.parent = t;

                headTilt.AddComponent<anglePointer>();

                _localHeadtilt = headTilt.transform;

                }

                return _localHeadtilt;
            }

        }

        [SerializeField]
        private Transform _remoteHeadTilt = null;
        public Transform RemoteHeadTilt
        {
            get
            {
                if (_remoteHeadTilt == null && remoteAvatar != null)
                {

                Transform t = DeepChildSearch(remoteAvatar, "head_JNT");

                var headTiltPrefab = new GameObject();

                var headTilt = new GameObject();

                headTilt.transform.rotation = t.rotation;

                //headTilt.transform.eulerAngles = new Vector3(headTilt.transform.eulerAngles.x + 18f, headTilt.transform.eulerAngles.y, headTilt.transform.eulerAngles.z);

                headTilt.transform.position = t.position;

                headTilt.transform.parent = t;

                headTilt.AddComponent<anglePointer>();

                _remoteHeadTilt = headTilt.transform;

            }

                return _remoteHeadTilt;
            }

        }

    public Transform DeepChildSearch(GameObject g, string childName) {

        Transform child = null;

        for (int i = 0; i< g.transform.childCount; i++) {

            Transform currentchild = g.transform.GetChild(i);

            if (currentchild.gameObject.name == childName)
            {

                return currentchild;
            }
            else {

                child = DeepChildSearch(currentchild.gameObject, childName);

                if (child != null) return child;
            }

        }

        return null;
    }
}
