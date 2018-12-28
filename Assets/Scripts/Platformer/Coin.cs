using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

	public GameObject shape;
	public GameObject shapeInstance;
	// Use this for initialization
	void Start ()
	{
		shapeInstance = (GameObject) Instantiate (shape);
		shapeInstance.SetLayerRecursively (LayerMask.NameToLayer ("Default"));
		shapeInstance.transform.SetParent (gameObject.transform);
		shapeInstance.transform.localPosition = Vector3.zero;
		shapeInstance.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		shapeInstance.GetComponentInChildren<Shape> ().FreezeRotation = true;
		SineWaveMovement swm = shapeInstance.AddComponent<SineWaveMovement> ();
		swm.Magnitude = 0.09f;
		swm.VerticalSpeed = 2;
		swm.RotationVelocityX = Random.Range (-35, 35);
		swm.RotationVelocityY = Random.Range (-35, 35);
		swm.RotationVelocityZ = Random.Range (-35, 35);
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			Destroy (gameObject);
			other.gameObject.GetComponentInChildren<Player> ().AddToInventory (shape);
			// EventManager.TriggerEvent (EventManager.EVENT_TYPE.COIN_COLLECTED);
		}

	}
}