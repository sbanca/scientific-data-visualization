﻿using UnityEngine;

namespace TableTop
{
    public class Rulers : Singleton<Rulers>
    {

        //public variables

        public float rulerdistance = 0.025f;

        //priavet variables

        private Mapzen.TileBounds TileBounds;

        private Vector2 RangeticksX;

        private Vector2 RangeticksY;

        private int TicksnumberX;

        private int TicksnumberY;

        public GameObject[] rulers = new GameObject[4];

        private Map map;

        private GameObject MapUIParent;

        //methods

        public void Initialize(Mapzen.TileBounds tileBounds)
        {
            getMapUIParent();

            if (map == null) getMapInstance();

            TileBounds = tileBounds;

            CalculateRangeThick();

            CreateRulers();

        }

        private void CreateRulers()
        {

            var mapbounds = Map.Instance.MapBoundaries.MapBounds;
            var size = new Vector4(Boundaries.Instance.TableBounds.min.x, Boundaries.Instance.TableBounds.min.z, Boundaries.Instance.TableBounds.max.x, Boundaries.Instance.TableBounds.max.z);

            // X top ruler 
            var RulerCenter = new Vector3(mapbounds.center.x, 0f, mapbounds.center.z + (mapbounds.size.z / 2));
            Rect VisibilityRectagle = new Rect(mapbounds.min.x - 20, mapbounds.min.z - 20, mapbounds.size.x + 20, mapbounds.size.z + 20);
            if (Map.Instance.useSlippyMap)
            {
                RulerCenter = new Vector3(mapbounds.center.x, 0f, size.z + rulerdistance);
                VisibilityRectagle = new Rect(size.x, size.z, size.z, size.w);
            }
            CreateRuler("ruler-top", 0, RangeticksX, TicksnumberX, mapbounds.size.x, Vector3.right, RulerCenter, VisibilityRectagle);

            // X Bottom ruler 
            RulerCenter = new Vector3(mapbounds.center.x, 0f, mapbounds.center.z - (mapbounds.size.z / 2));
            if (Map.Instance.useSlippyMap)
            {
                RulerCenter = new Vector3(mapbounds.center.x, 0f, size.y - rulerdistance);
                VisibilityRectagle = new Rect(size.x, -size.z, size.z, size.w);
            }
            CreateRuler("ruler-bottom", 1, RangeticksX, TicksnumberX, mapbounds.size.x, Vector3.left, RulerCenter, VisibilityRectagle);


            // Z left ruler 
            RulerCenter = new Vector3(mapbounds.center.x + (mapbounds.size.x / 2), 0f, mapbounds.center.z);
            if (Map.Instance.useSlippyMap)
            {
                RulerCenter = new Vector3(size.w + rulerdistance, 0f, mapbounds.center.z);
                VisibilityRectagle = new Rect(size.w, size.y, size.z, size.w);
            }
            CreateRuler("ruler-right", 2, RangeticksY, TicksnumberY, mapbounds.size.z, Vector3.back, RulerCenter, VisibilityRectagle);

            // Z Right ruler 
            RulerCenter = new Vector3(mapbounds.center.x - (mapbounds.size.x / 2), 0f, mapbounds.center.z);
            if (Map.Instance.useSlippyMap)
            {
                RulerCenter = new Vector3(size.x - rulerdistance, 0f, mapbounds.center.z);
                VisibilityRectagle = new Rect(-size.w, size.y, size.z, size.w);
            }
            CreateRuler("ruler-left", 3, RangeticksY, TicksnumberY, mapbounds.size.z, Vector3.forward, RulerCenter, VisibilityRectagle);

        }

        private void CreateRuler(string name, int number, Vector2 Rangeticks, int Ticksnumber, float Length, Vector3 Direction, Vector3 Center, Rect VisibilityRect)
        {

            rulers[number] = new GameObject();

            rulers[number].name = name;

            rulers[number].transform.parent = MapUIParent.transform;

            var ruler = rulers[number].AddComponent<Ruler>();

            ruler.Rangeticks = Rangeticks;

            ruler.Ticksnumber = Ticksnumber;

            ruler.Length = Length;

            ruler.Direction = Direction;

            ruler.Center = Center;

            ruler.VisibilityRectagle = VisibilityRect;

            ruler.Generate();

        }

        private void CalculateRangeThick() {
          
            RangeticksX = new Vector2(TileBounds.max.x, TileBounds.min.x);
            TicksnumberX = System.Math.Abs(TileBounds.max.x - TileBounds.min.x) + 2;

            RangeticksY = new Vector2(TileBounds.max.y, TileBounds.min.y);
            TicksnumberY = System.Math.Abs(TileBounds.max.y - TileBounds.min.y) + 2;

        }


        private void getMapUIParent()
        {

            MapUIParent = GameObject.Find("ArrowsAndRulers");

            if (MapUIParent == null)
            {

                MapUIParent = new GameObject();

                MapUIParent.name = "ArrowsAndRulers";


            }
        }


        private void getMapInstance()
        {


            if (Map.Instance == null)
            {
                map = gameObject.GetComponent<Map>();
            }
            else
            {
                map = Map.Instance;
            }



        }
    }
}