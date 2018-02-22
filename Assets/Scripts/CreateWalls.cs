using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWalls : MonoBehaviour {

	public static bool wall_build_mode;
	bool creating;
	public GameObject start;
	public GameObject end;
	public Camera camera;

	public GameObject wallPrefab;
	GameObject wall; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		getInput ();
	}

	void getInput(){
		if (wall_build_mode) {
			if (Input.GetMouseButtonDown (0)) {
				setStart ();
			} else if (Input.GetMouseButtonUp (0)) {
				setEnd ();
			} else {
				if (creating) {
					adjust ();
				}
			}
		}
	}

	void setStart(){
		creating = true;
		start.transform.position = getWorldPoint ();
		wall = Instantiate (wallPrefab, start.transform.position, Quaternion.identity) as GameObject;

	}

	void setEnd(){
		creating = false;
		end.transform.position = getWorldPoint ();

	}

	void adjust(){
		end.transform.position = getWorldPoint ();
		adjustWall ();
	}

	void adjustWall(){
		start.transform.LookAt (end.transform.position);
		end.transform.LookAt (start.transform.position);
		float distance = Vector3.Distance (start.transform.position, end.transform.position);
		wall.transform.position = start.transform.position + distance / 2 * start.transform.forward;
		wall.transform.rotation = start.transform.rotation;
		wall.transform.localScale = new Vector3 (wall.transform.localScale.x, wall.transform.localScale.y, distance);
	}

	Vector3 getWorldPoint(){
		//A ray that is from the mouse
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);

		int wallsLayerNum = 8;

		// Bit shift the index of the layer (8) to get a bit mask
		int layerMask = 1 << wallsLayerNum;
		// This would cast rays only against colliders in layer 8.
		// But instead we want to collide against everything except layer 8 (walls). The ~ operator does this, it inverts a bitmask.
		layerMask = ~layerMask;

		RaycastHit hit;
		// Does the ray intersect with any objects excluding the wall layer
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
			return hit.point;
		}
		return Vector3.zero;
	}

}
