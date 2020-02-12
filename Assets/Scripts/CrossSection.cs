using UnityEngine;
using System.Collections;



public class CrossSection : MonoBehaviour {

	public bool activOnStart;
    public Material backFaceMaterial;
	private Vector3 tempPos;
	private Quaternion tempRot;



	void Awake(){

		if(activOnStart) SetActiveTrue();
		else  SetActiveFalse();

        setBackfaces();
	}


	void OnDisable() {

		SetActiveFalse();
	}

    void OnEnable()
    {

        SetActiveTrue();
    }

	void Update () {
	
		if(tempPos!=transform.position || tempRot != transform.rotation){

			tempPos = transform.position;
			tempRot = transform.rotation;
			SetSection();
		}
	}


	void OnApplicationQuit(){

		SetActiveFalse();
        Shader.SetGlobalVector("_SectionCentre", new Vector4(0,0,0,1));
	}


	public void SetSection(){

		Shader.SetGlobalVector("_SectionCentre", transform.position);
		Shader.SetGlobalFloat("_Angle", -Mathf.Deg2Rad*transform.eulerAngles.y);
	}


	public void SetActiveFalse(){

		Shader.SetGlobalFloat("_OpacityOverride", 1.0f);
		print("Setting CrossSection-Shader off"); 
	}


	public void SetActiveTrue(){
		
		Shader.SetGlobalFloat("_OpacityOverride", 0.0f);
		print("Setting CrossSection-Shader active"); 
	}

    void setBackfaces()
    {
        Renderer[] allRenderers = Object.FindObjectsOfType<Renderer>();
        foreach (Renderer r in allRenderers)
        {
            Material[] materials = r.materials;
            foreach (Material m in materials)
            {
                if (m.shader.name == "Custom/ClipVolume")
                {
                    GameObject rgo = r.gameObject;
                    //GameObject ngo = new GameObject(rgo.name + "Backfaces");
                    GameObject ngo = Instantiate(rgo);
                    ngo.name = rgo.name + "Backfaces";
                    ngo.transform.parent = rgo.transform.parent;
                    ngo.transform.position = rgo.transform.position;
                    ngo.transform.rotation = rgo.transform.rotation;
                    ngo.transform.localScale = rgo.transform.localScale;

                    Material[] newMaterials = new Material[materials.Length];
                    for (int i = 0; i < materials.Length; i++) newMaterials[i] = backFaceMaterial;
                    ngo.GetComponent<Renderer>().materials = newMaterials;
                    break;
                }
            }
        }
    }
}