﻿using UnityEngine;
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

        public Vector2 Origin;


        //instances of Tabletop Elements

        public Boundaries MapBoundaries;

        public Rulers MapRulers;

        public Arrows MapArrows;

        public Coordinates MapCoordinates;

        public Interaction MapInteraction;

        public Augmentations MapAugmentations;

        public Panels MapPanels;

        public SpatialAnchors MapSpatialAnchors;



        public void Initialize(bool useSlippyMap, Vector2 origin, Mapzen.TileBounds bounds, MapStyle style, Vector4 slippyMapSize, float unitsPerMeter)
        {
            this.useSlippyMap = useSlippyMap;
            this.Origin = origin;
            this.TileBounds = bounds;
            this.Style = style;
            this.SlippyMapSize = slippyMapSize;
            this.UnitsPerMeter = unitsPerMeter;

            //boundaries

            MapBoundaries = gameObject.AddComponent<Boundaries>();

            MapBoundaries.Initialize(SlippyMapSize);


            //rulers

            MapRulers = gameObject.AddComponent<Rulers>();

            MapRulers.Initialize(TileBounds);


            //Arrows

            MapArrows = gameObject.AddComponent<Arrows>();

            MapArrows.Initialize();


            //Coordinates 
            MapCoordinates = gameObject.AddComponent<Coordinates>();

            //Interaction 
            MapInteraction = gameObject.AddComponent<Interaction>();

            //Interaction 
            MapAugmentations = gameObject.AddComponent<Augmentations>();

            //Panels
            MapPanels = gameObject.AddComponent<Panels>();

            //SpatialAnchors
            MapSpatialAnchors = gameObject.AddComponent<SpatialAnchors>();


        }

        private void GetInstances()
        {
            MapBoundaries = Boundaries.Instance;

            MapRulers = Rulers.Instance;

            MapArrows = Arrows.Instance;

            MapCoordinates = Coordinates.Instance;

            MapInteraction = Interaction.Instance;

            MapAugmentations = Augmentations.Instance;

            MapPanels = Panels.Instance;

            MapSpatialAnchors = SpatialAnchors.Instance;


        }

        private void Start()
        {

            GetInstances();

        }



    }
}