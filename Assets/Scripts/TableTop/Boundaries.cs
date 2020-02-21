
using UnityEngine;
using Mapzen.Unity;
using System.Threading.Tasks;
using System;

namespace TableTop {

    public class Boundaries : Singleton<Boundaries>
    {



        //MapBounds

        public Bounds MapBounds;

        //TableBounds 

        public Bounds TableBounds;

        public Vector4 slippyMapSize;

        public Vector4 SlippyMapSize {

            get { return slippyMapSize; }
            set
            {

                slippyMapSize = value;

                var (centerX, sizeX) = getCenterAndSizeFromMinAndMax(slippyMapSize.x, slippyMapSize.z);
                var (centerY, sizeY) = getCenterAndSizeFromMinAndMax(slippyMapSize.y, slippyMapSize.w);

                TableBounds = new Bounds(new Vector3(centerX, 0f, centerY), new Vector3(sizeX, 0f, sizeY));


            }
        }

        //methods

        public void Initialize( Vector4 slippymapsize) {

            SlippyMapSize = slippymapsize;

            

            CalculateMapsBounds();

            InitializeMaterialClipping();

            CreateBoxCollider();

        }
          
        private void CalculateMapsBounds()
        {

            MapBounds = new Bounds();
            MeshFilter[] meshfilter = GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter m in meshfilter)
            {
                if (m.gameObject.name == "Water" || m.gameObject.name == "Earth")
                    MapBounds.Encapsulate(m.sharedMesh.bounds);
            }
        

        }

        private void CreateBoxCollider()
        {

            BoxCollider b = gameObject.AddComponent<BoxCollider>();

            b.size = MapBounds.size;
            b.center = MapBounds.center;


        }

        private async void  InitializeMaterialClipping()
        {

            while (Map.Instance == null)
            {
                Console.WriteLine("Map is null");
                await Task.Delay(25);

            }

            var cornersBounds =  Map.Instance.useSlippyMap ? TableBounds : MapBounds;
            var corners = new Vector4(cornersBounds.min.x, cornersBounds.min.z, cornersBounds.size.x, cornersBounds.size.z);

#if UNITY_EDITOR

            foreach (FeatureLayer layer in Map.Instance.Style.Layers)
            {

                if (layer.Style.PolygonBuilder.Material != null || layer.Style.PolylineBuilder.Material != null)
                {
                    string name;

                    if (layer.Style.PolygonBuilder.Material != null) name = layer.Style.PolygonBuilder.Material.name;
                    else name = layer.Style.PolylineBuilder.Material.name;

                    string path = "Materials/" + name;

                    Material m = Resources.Load(path, typeof(Material)) as Material;


                    if (m != null) m.SetVector("_Corners", corners);

                }

            }

#else

        Renderer[] allRenderers = Object.FindObjectsOfType<Renderer>();
        //Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer r in allRenderers)
        {
            Material[] materials = r.materials;
            foreach (Material m in materials)
            {
                if (m.shader.name == "Custom/ClipVolume")
                {
                    m.SetVector("_Corners", corners );
                }
            }
        }

#endif

        }

        private (float, float) getCenterAndSizeFromMinAndMax(double min, double max)
        {

            var size = (max - min);

            var center = min + (size / 2);

            return ((float)center, (float)size);

        }

    }
}

