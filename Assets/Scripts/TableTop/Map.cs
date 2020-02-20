using UnityEngine;
using Mapzen.Unity;


namespace TableTop
{

    public class Map : Singleton<Map>
    {

        //informations from Mapzen Maps Creation process

        public bool useSlippyMap = true;

        public Mapzen.TileBounds TileBounds; // Boundaries->MapBounds

        public Vector4 SlippyMapSize; // Boundaries->TableBounds

        public float UnitsPerMeter;

        public MapStyle Style;

        public Mapzen.MercatorMeters MercatorMetersOrigin;

        public Vector2 Origin;


        //instances of Tabletop Elements

        public Boundaries MapBoundaries;

        public Rulers MapRulers;

        public Arrows MapArrows;

        public Coordinates MapCoordinates;

        public Interaction MapInteraction;


        public void Initialize()
        {
            //boundaries

            MapBoundaries = gameObject.AddComponent<Boundaries>();

            MapBoundaries.Initialize(SlippyMapSize);


            //rulers

            MapRulers = gameObject.AddComponent<Rulers>();

            MapRulers.Initialize(TileBounds);


            //Arrows

            MapArrows = gameObject.AddComponent<Arrows>();

            MapArrows.Initialize();


            //Interaction 
            MapInteraction = gameObject.AddComponent<Interaction>();


        }

        private void GetInstances()
        {
            MapBoundaries = Boundaries.Instance;

            MapRulers = Rulers.Instance;

            MapArrows = Arrows.Instance;

            MapCoordinates = Coordinates.Instance;

        }

        private void Start()
        {

            GetInstances();

        }



    }
}