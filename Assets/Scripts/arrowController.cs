using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ArrowController : MonoBehaviour
{
    //pbulic var
    [SerializeField]
    public float thickness = 1f;
    [SerializeField]
    public float thickenssZ = 0.1f;
    [SerializeField]
    public float thickLength = 2.5f;

    [SerializeField]
    private Vector3 direction = Vector3.right;
    private Vector3 oppositeDirection = Vector3.forward;
    private Vector3 Axe;
    private float maxPosition;
    private float minPosition;
    public Vector3 Direction
    {
        get { return direction; }
        set
        {
            if (direction != value)
            {

                direction = value;
                oppositeDirection = Vector3.Cross(direction, Vector3.up);
             
            }
        }
    }

    [SerializeField]
    private Vector3 center = new Vector3(0f, 0f, 0f);
    public Vector3 Center
    {
        get { return center; }
        set
        {
            if (center != value)
            {

                center = value;
            }
        }
    }

    public GameObject target;
    public GameObject[] rulers;

    private Vector4 Size;
    public Vector4 size {

        get { return Size; }
        set {

            Size = value;
            extent = new Vector3(size.z - size.x, 0f, size.w - size.y);
        }

    }
    public Vector3 extent;

    [SerializeField]
    public Bounds mapbounds;
   


    //private var 
    private Bounds arrowbounds;
    private GameObject arrowLeft;
    private GameObject arrowRight;
    private MeshRenderer[] m_Renderer;
    private Color m_OriginalColor;
    private Color targetColor = Color.blue;

    void Start()
    {
        //Fetch the mesh renderer component from the GameObject
        m_Renderer = GetComponentsInChildren<MeshRenderer>();
        //Fetch the original color of the GameObject
        m_OriginalColor= m_Renderer[0].material.color;
    }

    public void Generate() {

        if (arrowLeft != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(arrowLeft);
#else
            Destroy(arrowLeft);
#endif
        }

        if (arrowRight != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(arrowRight);
#else
            Destroy(arrowLeft);
#endif
        }

        //Create cube as child
        arrowLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
        arrowRight = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //Position   
        var point = direction * (thickLength / 2);
        arrowLeft.transform.RotateAround(point, Vector3.up, 45f);         
        arrowRight.transform.RotateAround(point, Vector3.up, -45f);
        
        //Scale
        Vector3 scale = arrowLeft.transform.localScale;
        Vector3 thickenssZVec = Vector3.up * thickenssZ;
        Vector3 thicknessVec = oppositeDirection * thickness;
        Vector3 lengthVec = direction * (thickLength + thickness);
        Vector3 final = thicknessVec + thickenssZVec + lengthVec;
        scale.Set(final.x, final.y, final.z);
        arrowLeft.transform.localScale = scale;
        arrowRight.transform.localScale = scale;

        //Translate 
        arrowRight.transform.position = gameObject.transform.position - arrowRight.transform.position;
        arrowLeft.transform.position =  gameObject.transform.position - arrowLeft.transform.position;

        //parenthood
        arrowRight.transform.parent = gameObject.transform;
        arrowLeft.transform.parent = gameObject.transform;

        //Translate parent to center 
        gameObject.transform.position = center;

        //Boxcollider
        createBox();

    }

    private void CalculateArrowBounds()
    {

        arrowbounds = new Bounds(transform.position, Vector3.one);
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {          
                arrowbounds.Encapsulate(renderer.bounds);
        }
    }

    private BoxCollider createBox() {

        CalculateArrowBounds();

        BoxCollider boxcol = gameObject.AddComponent<BoxCollider>();

        boxcol.size = arrowbounds.size;

        return boxcol;
    }

    private void OnMouseOver()
    {

        Axe = new Vector3(Math.Abs(direction.normalized.x), Math.Abs(direction.normalized.y), Math.Abs(direction.normalized.z));
        maxPosition = 0f;
        minPosition = Math.Abs(Vector3.Dot(extent, direction.normalized)) - Math.Abs(Vector3.Dot(mapbounds.size, direction.normalized));

        float futureposition = Vector3.Dot(target.transform.position+direction.normalized, Axe);
        
        if (futureposition < minPosition || futureposition > maxPosition) return; 

        target.transform.position += direction.normalized;
        rulers[0].transform.position += direction.normalized;
        rulers[1].transform.position += direction.normalized;

    }

    void OnMouseEnter()
    {
        
        foreach(MeshRenderer r in m_Renderer) r.material.color = targetColor;
    }

    void OnMouseExit()
    {

        foreach (MeshRenderer r in m_Renderer) r.material.color = m_OriginalColor;
    }

}
