using System;
using UnityEngine;
using UnityEngine.Events;

namespace TableTop
{

    public enum ColliderType { 
    
        Xpos= 0,
        Xneg= 1,
        Ypos = 2,
        Yneg = 3
    }

    public class Controls : MonoBehaviour
    {
        private Bounds mapbounds;

        private GameObject target;

        private Vector4 size;

        public Vector3 extent;

        public int[] rulerIndex = new int[2];

        private void Start()
        {
            mapbounds = Boundaries.Instance.MapBounds;

            target = Map.Instance.gameObject;

            size = new Vector4(Boundaries.Instance.TableBounds.min.x, Boundaries.Instance.TableBounds.min.z, Boundaries.Instance.TableBounds.max.x, Boundaries.Instance.TableBounds.max.z);

            extent = new Vector3(size.z - size.x, 0f, size.w - size.y);

        }
        public void Trigger(ColliderType type)
        {
            Debug.Log("collision detected");

            Vector3 direction = Direction(type);

            Vector3 Axe = new Vector3(Math.Abs(direction.normalized.x), Math.Abs(direction.normalized.y), Math.Abs(direction.normalized.z));
            float maxPosition = 0f;
            float minPosition = Math.Abs(Vector3.Dot(extent, direction.normalized)) - Math.Abs(Vector3.Dot(mapbounds.size, direction.normalized));

            float futureposition = Vector3.Dot(target.transform.position + direction.normalized * 0.0025f, Axe);

            if (futureposition < minPosition || futureposition > maxPosition) return;

            target.transform.position += direction.normalized * 0.0025f;
            
            Rulers.Instance.rulers[rulerIndex[0]].transform.position += direction.normalized * 0.0025f;
            Rulers.Instance.rulers[rulerIndex[1]].transform.position += direction.normalized * 0.0025f;

        }

        private Vector3 Direction(ColliderType type) {

            Vector3 direction = Vector3.zero;

            switch (type) {

                case (ColliderType.Xneg):
                    direction = Vector3.right;
                    rulerIndex[0] = 0;
                    rulerIndex[1] = 1;
                    break;
                case (ColliderType.Xpos):
                    direction = Vector3.left;
                    rulerIndex[0] = 0;
                    rulerIndex[1] = 1;
                    break;
                case (ColliderType.Yneg):
                    direction = Vector3.back;
                    rulerIndex[0] = 2;
                    rulerIndex[1] = 3;
                    break;
                case (ColliderType.Ypos):
                    direction = Vector3.forward;
                    rulerIndex[0] = 2;
                    rulerIndex[1] = 3;
                    break;

            }

            return direction;
        }

    }
}