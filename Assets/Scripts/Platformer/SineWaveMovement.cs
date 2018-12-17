using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveMovement : MonoBehaviour
{

	// Use this for initialization
	public float VerticalSpeed;
	public float Magnitude;

	public float RotationVelocityX;
	public float RotationVelocityY;
	public float RotationVelocityZ;
	public Vector3 startingPosition;
	void Start ()
	{
		startingPosition = transform.position;
	}

	// Update is called once per frame
	void Update ()
	{
		transform.position = startingPosition + new Vector3 (0.0f, Mathf.Sin (Time.time * VerticalSpeed) * Magnitude, 0.0f);;
		transform.Rotate (Vector3.forward, Time.deltaTime * RotationVelocityZ);
		transform.Rotate (Vector3.up, Time.deltaTime * RotationVelocityY);
		transform.Rotate (Vector3.right, Time.deltaTime * RotationVelocityX);
	}
}