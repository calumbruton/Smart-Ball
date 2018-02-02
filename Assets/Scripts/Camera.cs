using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraaa : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// Late Update is called once per frame like update but is guaranteed to be run last
	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}
