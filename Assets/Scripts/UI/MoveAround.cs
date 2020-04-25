using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAround : MonoBehaviour
{
    Vector3 center;

    LaserPointer lp;

    Vector3 startVec;

    Vector3 currentVec;

    Vector3 startpoint;
    Vector3 endpoint;
    Vector3 translation;

    GameObject RightHandAnchor;

    GameObject LeftHandAnchor;

    Camera CenterEyeAnchor;

    bool isMove;

    public void StartMove()
    {

        isMove = true;

        center = new Vector3(RightHandAnchor.transform.position.x, 0f, RightHandAnchor.transform.position.z);

        startVec = new Vector3(RightHandAnchor.transform.forward.x, 0f, RightHandAnchor.transform.forward.z);


    }


    void OnEnable()
    {
        RightHandAnchor = GameObject.Find("RightHandAnchor");

        LeftHandAnchor = GameObject.Find("LeftHandAnchor");

        CenterEyeAnchor = GameObject.Find("CenterEyeAnchor").GetComponentInChildren<Camera>();
    }

    public void Update()
    {
        if (isMove)
        {
            MoveAngle();

            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown) || OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown)) MoveFurther();

            else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp)|| OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp)) MoveCloser();

        }
        

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            isMove = false;

            lp = FindObjectOfType<LaserPointer>();

            lp.laserBeamBehavior = LaserPointer.LaserBeamBehavior.OnWhenHitTarget;

        }

   
    }

    public void MoveAngle()
    {

        currentVec = new Vector3(RightHandAnchor.transform.forward.x, 0f, RightHandAnchor.transform.forward.z);

        float angle = Vector3.SignedAngle(startVec, currentVec, Vector3.up);

        gameObject.transform.RotateAround(center, Vector3.up, angle);

        startVec = currentVec;

    }

    public void MoveFurther()
    {
        endpoint = new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z);

        startpoint = new Vector3(CenterEyeAnchor.transform.position.x, 0f, CenterEyeAnchor.transform.position.z);

        translation = (endpoint - startpoint) ;

        gameObject.transform.Translate(translation.normalized * 0.25f, Space.World);

        Vector3 target = new Vector3(CenterEyeAnchor.transform.position.x, gameObject.transform.position.y, CenterEyeAnchor.transform.position.z);

        transform.LookAt(target + translation * 2, Vector3.up);
    }

    public void MoveCloser()
    {

        endpoint = new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z);

        startpoint = new Vector3(CenterEyeAnchor.transform.position.x, 0f, CenterEyeAnchor.transform.position.z);

        translation = (startpoint - endpoint);

        gameObject.transform.Translate(translation.normalized * 0.25f, Space.World);

        Vector3 target = new Vector3(CenterEyeAnchor.transform.position.x, gameObject.transform.position.y, CenterEyeAnchor.transform.position.z);

        transform.LookAt(target+ translation * -2, Vector3.up);

    }

    public void Place()
    {

        gameObject.transform.position = CenterEyeAnchor.transform.position;

        translation = new Vector3(CenterEyeAnchor.transform.forward.x, 0f, CenterEyeAnchor.transform.forward.z);

        gameObject.transform.Translate(translation * 10f, Space.World);

        AlignToCamera();

    }


    public void AlignToCamera() {

        endpoint = new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z);

        startpoint = new Vector3(CenterEyeAnchor.transform.position.x, 0f, CenterEyeAnchor.transform.position.z);

        translation = ( endpoint - startpoint);

        Vector3 target = new Vector3(CenterEyeAnchor.transform.position.x, gameObject.transform.position.y, CenterEyeAnchor.transform.position.z);

        transform.LookAt(target + translation * 2, Vector3.up);

    }

    public void Show() { gameObject.SetActive(true); }

    public void Hide() { gameObject.SetActive(false); }
}
