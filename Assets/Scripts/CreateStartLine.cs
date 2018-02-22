using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStartLine : MonoBehaviour {

	public Camera camera;
	public static bool stln_build_mode;
	public GameObject trainingBall;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		getInput ();
	}


	void getInput(){
		if (stln_build_mode) {
			if (Input.GetMouseButtonDown (0)) {
				setStart ();
			}
		}
	}

	void setStart(){
		trainingBall.transform.position = getWorldPoint ();
		stln_build_mode = false;
	}

	Vector3 getWorldPoint(){
		//A ray that is from the mouse
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			return hit.point;
		}
		return Vector3.zero;
	}
}
