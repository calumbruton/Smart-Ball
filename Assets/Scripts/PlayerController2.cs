using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {

	public float speed;

	private Rigidbody rb;

	//Start is called on the first frame the script is active
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	//Fixed update is called just before rendering any physics calculations
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	//Update is called before rendering a fram, this is where our game code will go
	void Update ()
	{}
}

