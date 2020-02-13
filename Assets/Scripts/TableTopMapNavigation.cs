using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapzen.Unity;

public class TableTopMapNavigation : MonoBehaviour
{
    //public variables

    public MapStyle Style;

    [SerializeField]

    private Vector4 size;
    public Vector4 Size   
    {
        get { return size; }   
        set { if (size != value)   size = value; }  
    }

    //private variables 

    private Bounds mapbounds;

    private GameObject[] rulers = new GameObject[4];

    public void Initialize()
    {
        CalculateMapsBounds();

        InitializeMaterialClipping();

        CreateRulers();
    }

    private void CreateRulers() {

        float rulerdistance = 2.5f;

        // X top ruler 
        //var RulerCenter = new Vector3(mapbounds.center.x , 0f, mapbounds.center.z + (mapbounds.size.z / 2)); //aligned to the map
        var RulerCenter = new Vector3(size.x, 0f, size.z + rulerdistance);
        Rect VisibilityRectagle = new Rect(size.x, size.z,size.z,size.w); 
        CreateRuler("top",0, new Vector2(0, 20), 20 ,mapbounds.size.x, Vector3.right, RulerCenter, VisibilityRectagle);

        // X Bottom ruler 
        //RulerCenter = new Vector3(mapbounds.center.x, 0f , mapbounds.center.z - (mapbounds.size.z / 2)); //aligned to the map
        RulerCenter = new Vector3(size.x, 0f, size.y - rulerdistance);
        VisibilityRectagle = new Rect(size.x, -size.z,  size.z, size.w);
        CreateRuler("bottom", 0, new Vector2(0, 20), 20, mapbounds.size.x, Vector3.left, RulerCenter, VisibilityRectagle);

        // Z left ruler 
        //RulerCenter = new Vector3(mapbounds.center.x+ (mapbounds.size.x / 2), 0f, mapbounds.center.z ); //aligned to the map
        RulerCenter = new Vector3(size.w + rulerdistance, 0f, size.y );
        VisibilityRectagle = new Rect(size.w, size.y, size.z, size.w);
        CreateRuler("right", 0, new Vector2(0, 20), 30, mapbounds.size.z, Vector3.back, RulerCenter, VisibilityRectagle);

        // Z Right ruler 
        //RulerCenter = new Vector3(mapbounds.center.x - (mapbounds.size.x / 2), 0f, mapbounds.center.z); //aligned to the map 
        RulerCenter = new Vector3(size.x - rulerdistance, 0f, size.y);
        VisibilityRectagle = new Rect(-size.w, size.y, size.z, size.w);
        CreateRuler("left", 0, new Vector2(0, 20), 30, mapbounds.size.z, Vector3.forward, RulerCenter, VisibilityRectagle);

    }

    private void CreateRuler(string name, int number, Vector2 Rangeticks, int Ticksnumber, float Length, Vector3 Direction, Vector3 Center, Rect VisibilityRect) {

        rulers[number] = new GameObject();

        rulers[number].name = name;

        var coordinateruler = rulers[number].AddComponent<CoordinateRuler>();

        coordinateruler.Rangeticks = Rangeticks;

        coordinateruler.Ticksnumber = Ticksnumber;

        coordinateruler.Length = Length;

        coordinateruler.Direction = Direction;

        coordinateruler.Center = Center;

        coordinateruler.VisibilityRectagle = VisibilityRect;

        coordinateruler.Generate();

    }

    private void CalculateMapsBounds() {

        mapbounds = new Bounds(transform.position, Vector3.one);
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            mapbounds.Encapsulate(renderer.bounds);
        }
    }
    
    private void InitializeMaterialClipping() {

#if UNITY_EDITOR

        foreach (FeatureLayer layer in Style.Layers) {


   
            
            if (layer.Style.PolygonBuilder.Material != null || layer.Style.PolylineBuilder.Material != null)
            {
                string name;

                if (layer.Style.PolygonBuilder.Material != null) name = layer.Style.PolygonBuilder.Material.name;
                else name = layer.Style.PolylineBuilder.Material.name;

                string path = "Materials/" + name;

        

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
