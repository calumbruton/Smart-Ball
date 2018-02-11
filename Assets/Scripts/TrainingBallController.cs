using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using NeuralNetwork;

public class TrainingBallController : MonoBehaviour {


	public MasterController ctrl;
	public float movementSpeed;
	public float turnSpeed;
	public float rayLength;


	//Need to access fitness and brain from Master Controller
	public float fitness;
	public NN brain;

	//Distances to nearest wall left, front left, front, front right, right
	private float[] distances = {0,0,0,0,0};


	//Start is called on the first frame the script is active
	void Start (){
		fitness = 0;
		GameObject programStart = GameObject.Find ("StartProgram");
		ctrl = programStart.GetComponent<MasterController>();
	}

	//Fixed update will constantly move the object forward in the Z direction that it is facing
	void FixedUpdate ()
	{
		transform.position += transform.forward * Time.deltaTime * movementSpeed;

	}



	//Rotate the object in update and draw lines
	void Update()
	{

		//Rotate based on brain decision
		float output = this.brain.decision(distances);
		transform.Rotate (new Vector3 (0, output, 0) * Time.deltaTime * turnSpeed, Space.World);


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



		//Update Fitness Score
		for (int i = 0; i < distances.Length; i++) {
			fitness += (float)Math.Max((distances [i] / 10), 0.5);
		}


	} //End of Update


	//If hit a wall remove it from the Master Controller training ball list
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag("Wall")){
			ctrl.ball_alive.Remove (this);
		}

	}

}

