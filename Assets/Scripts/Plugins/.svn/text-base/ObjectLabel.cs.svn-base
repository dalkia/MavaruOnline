using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GUIText))]
public class ObjectLabel : MonoBehaviour {

	public Transform target;  // Object that this label should follow
	public Vector3 offset = Vector3.up;    // Units in world space to offset; 1 unit above object by default
	public float offsetMultiplier = 1;
	public bool clampToScreen = false;  // If true, label will be visible even if object is off screen
	public float clampBorderSize = 0.05f;  // How much viewport space to leave at the borders when a label is being clamped
	public bool useMainCamera = true;   // Use the camera tagged MainCamera
	public Camera cameraToUse ;   // Only use this if useMainCamera is false
	Camera cam ;
	Transform thisTransform;

    void Start () 
    {
        thisTransform = transform;
		//gameObject.GetComponent<GUIText>().text = target.GetComponent<Character>().user.Username;
		gameObject.GetComponent<GUIText>().text = "sdfsdhfkshdfkhsdkfhsjdhfksjdhfkjshdfjkskjdhfkjshdfkjhsdkfhsdkjfhkj";
		cam = Camera.main;
    }
	
	void OnLevelWasLoaded(){
		cam = Camera.main;
		Debug.Log("on level was loaded");
		
		/*if (useMainCamera)
        	cam = Camera.main;
    	else
        	cam = cameraToUse;
    	camTransform = cam.transform;*/
		
	}


    void Update()
    {
		if(cam == null) return;
        if (clampToScreen)
        {
            Vector3 relativePosition = cam.transform.InverseTransformPoint(target.position);
            relativePosition.z =  Mathf.Max(relativePosition.z, 1.0f);
            thisTransform.position = cam.WorldToViewportPoint(cam.transform.TransformPoint(relativePosition + offset * offsetMultiplier));
            thisTransform.position = new Vector3(Mathf.Clamp(thisTransform.position.x, clampBorderSize, 1.0f - clampBorderSize),
                                             Mathf.Clamp(thisTransform.position.y, clampBorderSize, 1.0f - clampBorderSize),
                                             thisTransform.position.z);

        }
        else
        {
            thisTransform.position = cam.WorldToViewportPoint(target.position + offset * offsetMultiplier);
        }
    }
	public Transform Target {
		get{
			return target;
		}
		set{
			target = value;
		}
	}
}
