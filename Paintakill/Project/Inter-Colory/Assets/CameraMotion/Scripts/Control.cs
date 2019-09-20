using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Control : MonoBehaviour 
{
	Rigidbody rb;
	public float speedMultiplier;
	Vector3 inputVector()
	{
		Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		return input;
	}

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () 
	{
		rb.AddForce (new Vector3 (inputVector ().x * speedMultiplier, 0, 0));
	}
}
