
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class RayOnMap : Singleton<RayOnMap>
    {
        public delegate void OnMeshHit(int i);
        public static event OnMeshHit MeshHit;

        [SerializeField]
        public bool staticData = false;

        private Vector4 _tableTopSize;

        [SerializeField]
        public MeshFilter Mesh;
        private Mesh mesh;
     

        [SerializeField]
        private Camera _mainCam;
        public Camera MainCam
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

            if (Mesh) { mesh = GetMesh(Mesh);  }

            if (staticData)
            {
                if (_mapCollider==null) _mapCollider = gameObject.GetComponent<Collider>();
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

                if (mesh!= null && MeshHit!=null) SubMesh(hit);
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

        public Nullable<int> SubMesh(RaycastHit hit)
        {
            
            int[] hittedTriangle = new int[]
            {
                mesh.triangles[hit.triangleIndex * 3],
                mesh.triangles[hit.triangleIndex * 3 + 1],
                mesh.triangles[hit.triangleIndex * 3 + 2]
            };
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                int[] subMeshTris = mesh.GetTriangles(i);
                for (int j = 0; j < subMeshTris.Length; j += 3)
                {
                    if (subMeshTris[j] == hittedTriangle[0] &&
                        subMeshTris[j + 1] == hittedTriangle[1] &&
                        subMeshTris[j + 2] == hittedTriangle[2])
                    {
                        //Debug.Log(string.Format("triangle index:{0} submesh index:{1} submesh triangle index:{2}", hit.triangleIndex, i, j / 3));

                        if (MeshHit != null) MeshHit(i);
                    }
                }
            }

            return null;

        }

        public void AddRemoteHead(Transform t)
        {
            _remoteHead = t;

        }

        static Mesh GetMesh(MeshFilter mf)
        {
   
                if (mf)
                {
                    Mesh m = mf.sharedMesh;
                    if (!m) { m = mf.mesh; }
                    if (m)
                    {
                        return m;
                    }
                }

            return (Mesh)null;
        }
    }
}
