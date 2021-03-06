﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{

	public List<GameObject> shapes;
	private List<GameObject> shapeInstances;
	// Use this for initialization
	void Start ()
	{
		if (shapeInstances == null)
		{

			shapeInstances = new List<GameObject> ();
		}
		EventManager.StartListening (EventManager.EVENT_TYPE.SHAPE_REMOVED, RefreshSpacing);
	}
	public void AddShape (GameObject shape)
	{
		shapes.Add (shape);
		GameObject inventoryShapeInstance = shape.gameObject; //Instantiate (shape.gameObject);
		inventoryShapeInstance.GetComponentInChildren<Shape> ().FreezeRotation = false;
		inventoryShapeInstance.transform.rotation = Quaternion.identity;
		inventoryShapeInstance.GetComponentInChildren<Shape> ().ClearColorVoxels ();
		shapeInstances.Add (inventoryShapeInstance);
		inventoryShapeInstance.transform.SetParent (transform);
		inventoryShapeInstance.SetLayerRecursively (LayerMask.NameToLayer ("UI"));
		// inventoryShapeInstance.transform.position = new Vector3 (11f, spacing, 85f);
		inventoryShapeInstance.transform.localScale = new Vector3 (0.75f, 0.75f, 1f);
		inventoryShapeInstance.GetComponentInChildren<Shape> ().gameObject.transform.localScale = new Vector3 (0.75f, 0.75f, 1f);

		GetComponent<WorldLayoutGroup> ().UpdateSpacing ();
		// inventoryShapeInstance.GetComponentInChildren<Shape> ().Scale (new Vector3 (0.75f, 0.75f, 0.75f));
		// spacing += 2.95f;
	}
	// void InitializeGameObjects ()
	// {

	// 	foreach (GameObject inventoryShapeGO in shapes)
	// 	{

	// 		GameObject inventoryShapeInstance = Instantiate (inventoryShapeGO.gameObject);
	// 		inventoryShapeInstance.transform.position = new Vector3 (11f, spacing, 85f);
	// 		inventoryShapeInstance.transform.localScale = new Vector3 (0.75f, 0.75f, 1f);
	// 		// inventoryShapeInstance.GetComponentInChildren<Shape>().Scale(new Vector3(0.75f,0.75f,0.75f));
	// 		spacing += 2.95f;
	// 	}
	// }
	public void RefreshSpacing (EventInfo info)
	{
		GetComponent<WorldLayoutGroup> ().UpdateSpacing ();
		// spacing = -8.5f;
		// foreach (GameObject shape in shapeInstances)
		// {
		// 	// if (shape.activeSelf)
		// 	// {

		// 	shape.transform.position = new Vector3 (11f, spacing, 85f);
		// 	spacing += 2.95f;
		// 	// }
		// }
	}
	// Update is called once per frame
	void Update ()
	{

	}
}