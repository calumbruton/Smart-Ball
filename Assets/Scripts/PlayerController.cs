using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using NeuralNetwork;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float turnSpeed;
	public float rayLength;
	public Text fitnessText;

	//Distances to nearest wall left, front left, front, front right, right
	private float[] distances = {0,0,0,0,0};
	private float fitness;
	private NN brain;

	//Start is called on the first frame the script is active
	void Start (){
		fitness = 0;
		brain = new NN(); 

	}



	//Fixed update will constantly move the object forward in the Z direction that it is facing
	void FixedUpdate ()
	{
		transform.position += transform.forward * Time.deltaTime * movementSpeed;

	}



	//Rotate the object in update and draw lines
	void Update()
	{


		float output = this.brain.decision (distances);
		print(output);
		transform.Rotate (new Vector3 (0, output, 0) * Time.deltaTime * turnSpeed, Space.World);

		/*
		if (Input.GetKey (KeyCode.D)) {
			//Rotate the sprite about the Y axis in the positive direction
			transform.Rotate (new Vector3 (0, 1, 0) * Time.deltaTime * turnSpeed, Space.World);
		}

		if (Input.GetKey (KeyCode.A)) {
			//Rotate the sprite about the Y axis in the negative direction
			transform.Rotate (new Vector3 (0, -1, 0) * Time.deltaTime * turnSpeed, Space.World);
		}
		*/
	
		//Stores the object that a raycast hits
		RaycastHit hit;

		//Raycast directions array
		Vector3[] feelers = new Vector3[]{
			transform.TransformDirection(Vector3.left),
			transform.TransformDirection(Vector3.left+Vector3.forward),
			transform.TransformDirection(Vector3.forward),
			transform.TransformDirection(Vector3.right + Vector3.forward),
			transform.TransformDirection(Vector3.right),
		};


		//update the distances to each wall from the object in each raycast direction
		for (int i = 0; i < feelers.Length; i++) {
		
			//If the raycast collides with something that isn't the player return the min distance between it
			//and the designated rayLength parameter
			if (Physics.Raycast (transform.position, feelers[i], out hit)) {
				if (hit.collider.tag != "Player") {
					distances[i] = Math.Min (hit.distance, rayLength);
				}
			}
		}
		
		//Draw the Rays in the debugger
		Debug.DrawRay (transform.position, feelers[0] * rayLength);
		Debug.DrawRay (transform.position, feelers[1] * rayLength);
		Debug.DrawRay (transform.position, feelers[2] * rayLength);
		Debug.DrawRay (transform.position, feelers[3] * rayLength);
		Debug.DrawRay (transform.position, feelers[4] * rayLength);


		//Draw a line on the screen that goes infront of the player
		LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, transform.forward * distances[2] + transform.position);

		if (distances[2] > 15){
			lineRenderer.material = Resources.Load("Materials/Green Laser", typeof(Material)) as Material;
		}
		else if (distances[2] > 8){
			lineRenderer.material = Resources.Load("Materials/Yellow Laser", typeof(Material)) as Material;
		}
		else lineRenderer.material = Resources.Load("Materials/Red Laser", typeof(Material)) as Material;
 

		//Left Line
		LineRenderer leftLine = GameObject.FindWithTag("leftLine").GetComponent<LineRenderer>();
		leftLine.SetPosition(0, transform.position);
		leftLine.SetPosition(1, transform.right * -distances[0] + transform.position);

		if (distances[0] > 7){
			leftLine.material = Resources.Load("Materials/Green Laser", typeof(Material)) as Material;
		}
		else if (distances[0] > 3.5){
			leftLine.material = Resources.Load("Materials/Yellow Laser", typeof(Material)) as Material;
		}
		else leftLine.material = Resources.Load("Materials/Red Laser", typeof(Material)) as Material;


		//Right Line
		LineRenderer rightLine = GameObject.FindWithTag("rightLine").GetComponent<LineRenderer>();
		rightLine.SetPosition(0, transform.position);
		rightLine.SetPosition(1, transform.right * distances[4] + transform.position);

		if (distances[4] > 7){
			rightLine.material = Resources.Load("Materials/Green Laser", typeof(Material)) as Material;
		}
		else if (distances[4] > 3.5){
			rightLine.material = Resources.Load("Materials/Yellow Laser", typeof(Material)) as Material;
		}
		else rightLine.material = Resources.Load("Materials/Red Laser", typeof(Material)) as Material;




		//Update Fitness Score
		for (int i = 0; i < distances.Length; i++) {
			fitness += distances [i]/10;
		}
		fitnessText.text = "Fitness: " + fitness.ToString ();



	} //End of Update

}

