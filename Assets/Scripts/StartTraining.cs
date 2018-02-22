using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTraining : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Start the training by enabling master controller
	public void start(){
		MasterController training = GetComponent<MasterController>();
		training.enabled = true;
	}
}
