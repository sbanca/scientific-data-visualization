using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class Arrows : Singleton<Arrows>
    {

        public float arrowdistance = 0.08f;

        private GameObject[] arrows = new GameObject[4];

        private Vector4 size;

        public void Initialize()
        {

            CreateArrows();

        }

        private void CreateArrows()
        {
            
            var rulers = Rulers.Instance.rulers;
            size = new Vector4(Boundaries.Instance.TableBounds.min.x, Boundaries.Instance.TableBounds.min.z, Boundaries.Instance.TableBounds.max.x, Boundaries.Instance.TableBounds.max.z);
            var rulerdistance = Rulers.Instance.rulerdistance;
            
            // X rulers
            GameObject[] Xrulers = new GameObject[2];
            Xrulers[0] = rulers[2];
            Xrulers[1] = rulers[3];

            // X top arrow 
            var ArrowCenter = new Vector3(size.w / 2, 0f, size.z + rulerdistance + arrowdistance);
            CreateArrow("arrow-top", 0, 0.1f, Vector3.back, ArrowCenter, Xrulers);

            // X bottom arrow 
            ArrowCenter = new Vector3(size.w / 2, 0f, 0f - rulerdistance - arrowdistance);
            CreateArrow("arrow-bottom", 1, 0.1f, Vector3.forward, ArrowCenter, Xrulers);


            // Y rulers
            GameObject[] Yrulers = new GameObject[2];
            Yrulers[0] = rulers[0];
            Yrulers[1] = rulers[1];

            // Y Right arrow 
            ArrowCenter = new Vector3(size.w + rulerdistance + arrowdistance, 0f, size.z / 2);
            CreateArrow("arrow-right", 2, 0.1f, Vector3.left, ArrowCenter, Yrulers);

            // Y Left arrow 
            ArrowCenter = new Vector3(0f - rulerdistance - arrowdistance, 0f, size.z / 2);
            CreateArrow("arrow-left", 3, 0.1f, Vector3.right, ArrowCenter, Yrulers);

        }

        private void CreateArrow(string name, int number, float Length, Vector3 Direction, Vector3 Center, GameObject[] rulers)
        {
            arrows[number] = new GameObject();

            arrows[number].name = name;

            var arrow = arrows[number].AddComponent<Arrow>();

            arrow.target = gameObject;

            arrow.rulers = rulers;

            arrow.Direction = Direction;

            arrow.Center = Center;

            arrow.thickLength = Length;

            arrow.size = size;

            arrow.Generate();

        }


    }
}
