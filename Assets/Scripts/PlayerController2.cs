using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour {

	public float speed;
	public Text countText;

	private Rigidbody rb;
	private int count;

	//Start is called on the first frame the script is active
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		setCountText ();
	}

	//Fixed update is called just before rendering any physics calculations
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag("Pick Up")){
			other.gameObject.SetActive(false);
			count += 1;
			setCountText ();
		}
			
	}

	void setCountText(){
		countText.text = "Count: " + count.ToString ();
	}
}

