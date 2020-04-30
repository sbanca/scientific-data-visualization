
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class RayOnMap : Singleton<RayOnMap>
    {

        [SerializeField]
        public bool staticSurface = false;

        private Vector4 _tableTopSize;

        [SerializeField]
        private Camera _mainCam;
        private Camera MainCam
        {
            get { return _mainCam; }
            set { _mainCam = value; }
        }

        [SerializeField]
        private Transform _remoteHead;
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

        [SerializeField]
        private Collider _mapCollider;
        private Collider MapCollider
        {
            get { return _mapCollider; }
            set { _mapCollider = value; }
        }

        private RaycastHit hit;

        private Ray ray;


        private void Start()
        {
            if (staticSurface)
            {
                _mapCollider = gameObject.GetComponent<Collider>();
            }
            else
            {

                _mapCollider = Map.Instance.gameObject.GetComponent<BoxCollider>();

                _tableTopSize = Boundaries.Instance.slippyMapSize;
            }

            _mainCam = Camera.main;


        }

        public Nullable<Vector3> MouseRay()
        {
            ray = _mainCam.ScreenPointToRay(Input.mousePosition);

            if (_mapCollider.Raycast(ray, out hit, 200f))
            {

                if (_tableTopSize == Vector4.zero) return hit.point;

                if (hit.point.x < _tableTopSize.x || hit.point.x > _tableTopSize.z || hit.point.z < _tableTopSize.y || hit.point.z > _tableTopSize.w)
                {

                    return null;
                }

                return hit.point;

            }

            return null;
        }

        public Nullable<Vector3> HeadRay()
        {

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (_mapCollider.Raycast(ray, out hit, 200f))
            {
                if (_tableTopSize == Vector4.zero)

                    return hit.point;

                if (hit.point.x < _tableTopSize.x || hit.point.x > _tableTopSize.z || hit.point.z < _tableTopSize.y || hit.point.z > _tableTopSize.w)
                {

                    return null;
                }

                return hit.point;

            }

            return null;

        }

        public Nullable<Vector3> RemoteHeadRay()
        {
            if (_remoteHead == null) return null;

            Ray ray = new Ray(_remoteHead.position, _remoteHead.forward);

            if (_mapCollider.Raycast(ray, out hit, 200f))
            {
                if (_tableTopSize == Vector4.zero) return hit.point;

                if (hit.point.x < _tableTopSize.x || hit.point.x > _tableTopSize.z || hit.point.z < _tableTopSize.y || hit.point.z > _tableTopSize.w)
                {

                    return null;
                }

                return hit.point;

            }

            return null;

        }

        public void AddRemoteHead(Transform t)
        {
            _remoteHead = t;

        }
    }
}
