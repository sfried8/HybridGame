using System.Collections;
using UnityEngine;

public class GridInputManager : MonoBehaviour
{

	public Grid grid;
	// Use this for initialization
	void Start ()
	{
		grid = GetComponent<Grid> ();
	}

	// Update is called once per frame
	void Update ()
	{
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

		if (Input.GetButtonDown ("Deselect")) // && grid.currentShape == null)
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
	}
}