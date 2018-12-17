using System.Collections;
using UnityEngine;

public class GridInputManager : MonoBehaviour
{

	// [HideInInspector]
	public Grid grid;
	// Use this for initialization
	void Start ()
	{
		grid = GetComponent<Grid> ();
	}
	Vector3 dragStartPosition = Vector3.zero;
	// Update is called once per frame

	void RightClickDown ()
	{
		if (dragStartPosition == Vector3.zero)
		{
			dragStartPosition = Input.mousePosition;
		}
		Debug.Log (dragStartPosition);
		Debug.Log (Input.mousePosition);
		Debug.DrawLine (Vector3.zero, Input.mousePosition - dragStartPosition, Color.red, 0.0f, false);
		// grid.RightClickDrag(Input.mousePosition );
	}
	void RightClickEnded ()
	{

	}
	void Update ()
	{
		if (!grid.isActive)
		{
			return;
		}
		if (Input.GetButtonDown ("Roll") && grid.currentShape != null)
		{
			grid.currentShape.rotate (true, false);
			grid.RefreshGrid ();

		}
		else if (Input.GetButtonDown ("RollReverse") && grid.currentShape != null)
		{
			grid.currentShape.rotate (true, true);
			grid.RefreshGrid ();

		}

		if (Input.GetButtonDown ("Yaw") && grid.currentShape != null)
		{
			grid.currentShape.rotate (false, false);
			grid.RefreshGrid ();

		}
		else if (Input.GetButtonDown ("YawReverse") && grid.currentShape != null)
		{
			grid.currentShape.rotate (false, true);
			grid.RefreshGrid ();

		}

		if (Input.GetButtonDown ("Deselect") && grid.currentShape == null)
		{

			grid.RemoveCurrentShape ();
			grid.RefreshGrid ();

		}
		if (Input.GetMouseButtonDown (0))
		{
			if (grid.currentHover == null || grid.currentShape == null)
			{
				grid.GetClickTarget ();
			}
			else
			{
				grid.PlaceCurrentShape ();
				grid.RefreshGrid ();
			}
		}

		// if (Input.GetMouseButton(1)) {
		// 	if (grid.currentHover != null && grid.currentShape != null) {
		// 		RightClickDown();
		// 	}
		// }
		// if (Input.GetMouseButtonUp(1)) {
		// 	if (dragStartPosition != Vector3.zero) {
		// 		RightClickEnded();
		// 	}
		// }
	}
}