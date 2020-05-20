using System.Collections;
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
                    _controller =localAvatar.transform.Find("hand_right");
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
                    _remoteController = remoteAvatar.transform.Find("hand_right");
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
                if (_localHead == null && remoteAvatar != null)
                {
                    _localHead = remoteAvatar.transform.Find("head_JNT");
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
                    _remoteHead = remoteAvatar.transform.Find("head_JNT");
                }

                return _remoteHead;
            }

        }


}
