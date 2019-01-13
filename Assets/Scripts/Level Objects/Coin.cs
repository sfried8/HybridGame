using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

	public Shape shape;
	public ShapeData shapeData;
	// Use this for initialization
	void Start ()
	{
		// if (shapeInstance != null)
		// {
		// 	DestroyImmediate (shapeInstance);
		// }
		// shapeInstance = (GameObject) Instantiate (shape);
		shape = GetComponentInChildren<Shape> ();
		// shapeInstance = shape.transform.parent.gameObject;

		gameObject.SetLayerRecursively (LayerMask.NameToLayer ("Default"));
		// shapeInstance.transform.SetParent (gameObject.transform);
		// shapeInstance.transform.localPosition = Vector3.zero;
		shape.transform.parent.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		shape.FreezeRotation = true;
		SineWaveMovement swm = shape.transform.parent.gameObject.AddComponent<SineWaveMovement> ();
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

	[ContextMenu ("Initialize")]
	public void InitShape ()
	{
		// if (shapeInstance != null)
		// {
		// 	DestroyImmediate (shapeInstance);
		// }
		// shapeInstance = (GameObject) Instantiate (shape);
		// shapeInstance.transform.SetParent (gameObject.transform);
		// shapeInstance.transform.localPosition = Vector3.zero;
		// shapeInstance.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		// shapeInstance.GetComponentInChildren<Shape> ().FreezeRotation = true;
		// shapeInstance.GetComponentInChildren<Shape> ().gridSource = shapeData.ShapeString;
		// shapeInstance.GetComponentInChildren<Shape> ().Regenerate ();
		shape = GetComponentInChildren<Shape> ();
		gameObject.SetLayerRecursively (LayerMask.NameToLayer ("Default"));
		// shapeInstance = shape.transform.parent.gameObject;
		shape.FreezeRotation = true;
		shape.gridSource = shapeData.ShapeString;
		shape.Regenerate ();
	}
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			Destroy (gameObject);
			other.gameObject.GetComponentInChildren<Player> ().AddToInventory (shape.gameObject);
			// EventManager.TriggerEvent (EventManager.EVENT_TYPE.COIN_COLLECTED);
		}

	}
}