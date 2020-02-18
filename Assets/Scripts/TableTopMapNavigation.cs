using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapzen.Unity;

public class TableTopMapNavigation : MonoBehaviour
{
    //public variables

    public MapStyle Style;

    public bool useSlippyMap = true;

    public float rulerdistance = 2.5f;

    public float arrowdistance = 8f;

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

    private GameObject[] arrows = new GameObject[4];


    public void Initialize()
    {
        CalculateMapsBounds();

        InitializeMaterialClipping();

        CreateRulers();

        if (useSlippyMap) CreateArrows();
    }

    private void CreateRulers() {

        
        // X top ruler 
        var RulerCenter = new Vector3(mapbounds.center.x , 0f, mapbounds.center.z + (mapbounds.size.z / 2));
        Rect VisibilityRectagle = new Rect(mapbounds.min.x - 20, mapbounds.min.z - 20 , mapbounds.size.x + 20, mapbounds.size.z + 20 );
        if (useSlippyMap)
        {
            RulerCenter = new Vector3(mapbounds.center.x, 0f, size.z + rulerdistance);
            VisibilityRectagle = new Rect(size.x, size.z,size.z,size.w); 
        }        
        CreateRuler("ruler-top",0, new Vector2(0, 20), 20 ,mapbounds.size.x, Vector3.right, RulerCenter, VisibilityRectagle);

        // X Bottom ruler 
        RulerCenter = new Vector3(mapbounds.center.x, 0f, mapbounds.center.z - (mapbounds.size.z / 2));
        if (useSlippyMap)
        {
            RulerCenter = new Vector3(mapbounds.center.x, 0f, size.y - rulerdistance);
            VisibilityRectagle = new Rect(size.x, -size.z, size.z, size.w);
        }
        CreateRuler("ruler-bottom", 1, new Vector2(0, 20), 20, mapbounds.size.x, Vector3.left, RulerCenter, VisibilityRectagle);


        // Z left ruler 
        RulerCenter = new Vector3(mapbounds.center.x + (mapbounds.size.x / 2), 0f, mapbounds.center.z);
        if (useSlippyMap)
        {
            RulerCenter = new Vector3(size.w + rulerdistance, 0f, mapbounds.center.z);
            VisibilityRectagle = new Rect(size.w, size.y, size.z, size.w);
        }
        CreateRuler("ruler-right", 2, new Vector2(0, 20), 30, mapbounds.size.z, Vector3.back, RulerCenter, VisibilityRectagle);

        // Z Right ruler 
        RulerCenter = new Vector3(mapbounds.center.x - (mapbounds.size.x / 2), 0f, mapbounds.center.z);
        if (useSlippyMap)
        {
            RulerCenter = new Vector3(size.x - rulerdistance, 0f, mapbounds.center.z);
            VisibilityRectagle = new Rect(-size.w, size.y, size.z, size.w);
        }
        CreateRuler("ruler-left", 3, new Vector2(0, 20), 30, mapbounds.size.z, Vector3.forward, RulerCenter, VisibilityRectagle);

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

    private void CreateArrows() {

        // X rulers
        GameObject[] Xrulers = new GameObject[2];
        Xrulers[0] = rulers[2];
        Xrulers[1] = rulers[3];

        // X top arrow 
        var ArrowCenter = new Vector3(size.w/2, 0f, size.z + rulerdistance + arrowdistance);
        CreateArrow("arrow-top", 0, 10,  Vector3.back, ArrowCenter, Xrulers);

        // X bottom arrow 
        ArrowCenter = new Vector3(size.w / 2, 0f,  0f - rulerdistance - arrowdistance);
        CreateArrow("arrow-bottom", 1, 10, Vector3.forward, ArrowCenter, Xrulers);


        // Y rulers
        GameObject[] Yrulers = new GameObject[2];
        Yrulers[0] = rulers[0];
        Yrulers[1] = rulers[1];

        // Y Right arrow 
        ArrowCenter = new Vector3(size.w + rulerdistance + arrowdistance, 0f, size.z / 2);
        CreateArrow("arrow-right", 2, 10, Vector3.left, ArrowCenter, Yrulers);

        // Y Left arrow 
        ArrowCenter = new Vector3(0f - rulerdistance - arrowdistance, 0f, size.z / 2);
        CreateArrow("arrow-left", 3, 10, Vector3.right, ArrowCenter, Yrulers);

    }

    private void CreateArrow(string name, int number, float Length, Vector3 Direction, Vector3 Center, GameObject[] rulers)
    {
        arrows[number] = new GameObject();

        arrows[number].name = name;

        var arrow = arrows[number].AddComponent<ArrowController>();

        arrow.target = gameObject;

        arrow.rulers = rulers;

        arrow.Direction = Direction;

        arrow.Center = Center;

        arrow.thickLength = Length;

        arrow.size = size;

        arrow.mapbounds = mapbounds;

        arrow.Generate();

    }

    private void CalculateMapsBounds() {

        mapbounds = new Bounds(transform.position, Vector3.one);
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer.gameObject.name == "Water" || renderer.gameObject.name == "Earth")
                mapbounds.Encapsulate(renderer.bounds);
        }
    }
    
    private void InitializeMaterialClipping() {

        var corners = useSlippyMap ? size : new Vector4(mapbounds.min.x, mapbounds.min.z, mapbounds.size.x, mapbounds.size.z);

#if UNITY_EDITOR

        foreach (FeatureLayer layer in Style.Layers) {
            
            if (layer.Style.PolygonBuilder.Material != null || layer.Style.PolylineBuilder.Material != null)
            {
                string name;

                if (layer.Style.PolygonBuilder.Material != null) name = layer.Style.PolygonBuilder.Material.name;
                else name = layer.Style.PolylineBuilder.Material.name;

                string path = "Materials/" + name;

                Material m = Resources.Load(path, typeof(Material)) as Material;


                if ( m != null )  m.SetVector("_Corners", corners);

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

}
