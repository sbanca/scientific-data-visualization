using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapzen.Unity;

public class TableTopMapNavigation : MonoBehaviour
{
    private Vector4 size; // field

    public MapStyle Style;

    public Vector4 Size   // property
    {
        get { return size; }   // get method
        set {

            if (size != value) initializeMaterialClipping(value);
            size = value; 
        
        }  // set method
    }

    public Bounds mapbounds;

    public void Initialize()
    {
        calculateMapsBounds();
    }

    private void calculateMapsBounds() {

        mapbounds = new Bounds(transform.position, Vector3.one);
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            mapbounds.Encapsulate(renderer.bounds);
        }
    }
    
    private void initializeMaterialClipping(Vector4 size) {

#if UNITY_EDITOR

        foreach (FeatureLayer layer in Style.Layers) {


            Debug.Log(layer.Name);
            
            if (layer.Style.PolygonBuilder.Material != null || layer.Style.PolylineBuilder.Material != null)
            {
                string name;

                if (layer.Style.PolygonBuilder.Material != null) name = layer.Style.PolygonBuilder.Material.name;
                else name = layer.Style.PolylineBuilder.Material.name;

                string path = "Materials/" + name;

                Debug.Log(path);

                Material m = Resources.Load(path, typeof(Material)) as Material;

                if ( m != null)  m.SetVector("_Corners", size);

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
                    m.SetVector("_Corners", size );
                }
            }
        }

#endif

}

}
