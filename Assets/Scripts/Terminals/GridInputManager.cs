using System.Collections;
using UnityEngine;
using static Shape;
using static Util;
public class GridInputManager : MonoBehaviour
{

	// [HideInInspector]
	public TerminalGrid grid;
	// Use this for initialization
	void Start ()
	{
		grid = GetComponent<TerminalGrid> ();
	}
	// Update is called once per frame

	void Update ()
	{
		if (!grid.isActive)
		{
			return;
		}
		if (Input.GetButtonDown ("Roll") && grid.currentShape != null)
		{
			grid.currentShape.rotate (ROTATE_DIRECTION.ROLL, false);
			grid.RefreshGrid ();

		}
		else if (Input.GetButtonDown ("RollReverse") && grid.currentShape != null)
		{
			grid.currentShape.rotate (ROTATE_DIRECTION.ROLL, true);
			grid.RefreshGrid ();

		}

		if (Input.GetButtonDown ("Yaw") && grid.currentShape != null)
		{
			grid.currentShape.rotate (ROTATE_DIRECTION.YAW, false);
			grid.RefreshGrid ();

		}
		else if (Input.GetButtonDown ("YawReverse") && grid.currentShape != null)
		{
			grid.currentShape.rotate (ROTATE_DIRECTION.YAW, true);
			grid.RefreshGrid ();

		}

		if (Input.GetKeyDown (KeyCode.W) && grid.currentShape != null)
		{
			grid.currentShape.rotate (ROTATE_DIRECTION.PITCH, false);
			grid.RefreshGrid ();

		}
		else if (Input.GetKeyDown (KeyCode.S) && grid.currentShape != null)
		{
			grid.currentShape.rotate (ROTATE_DIRECTION.PITCH, true);
			grid.RefreshGrid ();

		}

		if (Input.GetButtonDown ("Deselect"))
		{

			grid.RemoveCurrentShape ();
			grid.ReturnToInventory ();
			grid.RefreshGrid ();

		}
		if (Input.GetMouseButtonDown (0))
		{
			if (grid.currentHover == null)
			{
				grid.GetClickTarget ();
			}
			else
			{
				if (grid.currentShape != null)
				{
					grid.PlaceCurrentShape ();
				}
				else
				{
					grid.RemoveCurrentShape ();
				}
				grid.RefreshGrid ();
			}
		}

	}

}