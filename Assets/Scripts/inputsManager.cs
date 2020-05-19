using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputsManager : Singleton<inputsManager>
{


        [SerializeField]
        private Transform _controller;
        public Transform Controller
        {
            get { 

                return _controller; 
            }
            
        set { _controller = value; }
        }

        [SerializeField]
        private Transform _remoteController = null;
        public Transform RemoteController
        {
            get
            {
                return _remoteController;
            }
            set
            {
                _remoteController = value;
            }
        }

        [SerializeField]
        private Transform _localHead;
        public Transform LocalHead
    {
            get { return _localHead; }
            set { _localHead = value; }
        }

        [SerializeField]
        private Transform _remoteHead = null;
        public Transform RemoteHead
        {
            get
            {
                return _remoteHead;
            }
            set
            {
                _remoteHead = value;
            }
        }


}
