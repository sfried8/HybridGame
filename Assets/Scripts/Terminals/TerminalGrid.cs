using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerminalGrid : MonoBehaviour
{
	public GameObject buttonPrefab;
	public Shape currentShape;
	private List<Shape> placedShapes = new List<Shape> ();

	public Dictionary<Point, Shape> occupiedTiles = new Dictionary<Point, Shape> ();
	public Point currentHover = null;

	public List<GridPuzzle> puzzles = new List<GridPuzzle> ();
	private int puzzleIndex = 0;
	public int[, ] grid;

	private HashSet<Point> slots = new HashSet<Point> ();
	// Use this for initialization

	public int rowCount;
	public int colCount;
	public bool isActive = false;
	private bool wasIncomplete = true;

	private TerminalGridUIManager terminalUI;
	public void OnActivated ()
	{
		isActive = true;
		terminalUI.OnActivated ();
		RefreshGrid ();
	}
	public void OnDeactivated ()
	{
		isActive = false;
		if (currentShape != null)
		{
			ReturnToInventory ();

		}
		terminalUI.OnDeactivated ();
		ButtonHoverExit ();
	}
	void Start ()
	{
		grid = puzzles[puzzleIndex].Grid;
		rowCount = grid.GetLength (0);
		colCount = grid.GetLength (1);
		// CreateNewShape ();
		for (int row = 0; row < rowCount; row++)
		{
			for (int col = 0; col < colCount; col++)
			{

				if (grid[row, col] == 1)
				{
					slots.Add (Point.GridCoord (row, col));
				}
			}
		}
		terminalUI = gameObject.AddComponent<TerminalGridUIManager> ();
		terminalUI.buttonPrefab = buttonPrefab;
		RefreshGrid ();
		currentHover = null;
	}

	public void ButtonHover (int row, int col)
	{
		if (!isActive)
		{
			return;
		}
		if (currentHover == null || currentHover.Row != row || currentHover.Col != col)
		{

			currentHover = Point.GridCoord (row, col);

			RefreshGrid ();
		}

	}
	public void ButtonHoverExit ()
	{
		if (currentHover == null)
		{
			return;
		}
		currentHover = null;
		currentShape?.ClearColorVoxels ();
		RefreshGrid ();
	}
	public List<Point> GetHoverTiles ()
	{
		if (currentShape == null || currentHover == null)
		{
			return null;
		}
		return currentShape.GetTileLocations (currentHover);
	}
	public void ColorCurrentShapeVoxels ()
	{
		// int height = currentShape.FaceHeight ();
		// int width = currentShape.FaceWidth ();
		int height = currentShape.height;
		int width = currentShape.width;
		int[, ] hoverSection = new int[height, width];
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				if (Util.IsInRange (i + currentHover.Row - currentShape.OffsetPoint.Y, 0, rowCount) &&
					Util.IsInRange (j + currentHover.Col - currentShape.OffsetPoint.X, 0, colCount))
				{
					int hsy = height - i - 1;
					int hsx = width - j - 1;

					hoverSection[hsy, hsx] = grid[i + currentHover.Row - currentShape.OffsetPoint.Y, j + currentHover.Col - currentShape.OffsetPoint.X];

				}
			}
		}
		currentShape.ColorVoxels (hoverSection);
	}
	public List<Point> GetHoverShapeTileLocations ()
	{
		if (currentShape == null && currentHover != null)
		{
			if (occupiedTiles.ContainsKey (currentHover))
			{
				Shape hoverShape = occupiedTiles[currentHover];
				return hoverShape.GetTileLocations (hoverShape.location);
			}
		}
		return null;
	}
	public void RefreshGrid ()
	{
		if (!isActive)
		{
			return;
		}

		terminalUI.RefreshGrid ();

	}
	bool CheckIsComplete ()
	{
		if (occupiedTiles.Count == slots.Count && slots.All ((p) => occupiedTiles.ContainsKey (p)))
		{
			if (wasIncomplete)
			{
				wasIncomplete = false;
				EventManager.TriggerEvent (EventManager.EVENT_TYPE.TERMINAL_COMPLETE, new TerminalPuzzleInfo () { terminalGrid = this });

			}
			return true;
		}
		if (!wasIncomplete)
		{
			wasIncomplete = true;
			EventManager.TriggerEvent (EventManager.EVENT_TYPE.TERMINAL_INCOMPLETE, new TerminalPuzzleInfo () { terminalGrid = this });
		}
		return false;
	}
	public void PlaceCurrentShape ()
	{

		List<Point> currentTiles = currentShape.GetTileLocations (currentHover);
		if (currentTiles.Any (i => occupiedTiles.ContainsKey (i)))
		{
			return;
		}
		foreach (Point currentTile in currentShape.GetTileLocations (currentHover))
		{
			occupiedTiles.Add (currentTile, currentShape);
		}
		placedShapes.Add (currentShape);

		currentShape.location = currentHover;
		currentShape.FreezePosition = false;
		currentShape.transform.parent.gameObject.SetActive (false);
		currentShape = null;
		CheckIsComplete ();
	}
	public void RemoveCurrentShape ()
	{
		if (currentShape == null && (currentHover == null || !occupiedTiles.ContainsKey (currentHover)))
		{
			return;
		}
		Shape shapeToRemove = currentShape ?? occupiedTiles[currentHover];

		shapeToRemove.transform.parent.gameObject.SetActive (true);
		shapeToRemove.ClearColorVoxels ();
		EventManager.TriggerEvent (EventManager.EVENT_TYPE.SHAPE_REMOVED, null);

		if (placedShapes.Contains (shapeToRemove))
		{

			foreach (Point p in shapeToRemove.GetTileLocations (shapeToRemove.location))
			{
				occupiedTiles.Remove (p);
			}
			placedShapes.Remove (shapeToRemove);
		}
		CheckIsComplete ();
		SetCurrentShape (shapeToRemove);
	}
	public void ReturnToInventory ()
	{
		if (currentShape != null)
		{
			currentShape.gameObject.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
			currentShape.ClearColorVoxels ();
			currentShape.FreezePosition = false;
		}
		currentShape = null;
		EventManager.TriggerEvent (EventManager.EVENT_TYPE.SHAPE_REMOVED, null);
	}
	public void ReturnAllToInventory ()
	{
		foreach (Shape shape in placedShapes)
		{
			shape.transform.parent.gameObject.SetActive (true);
			shape.ClearColorVoxels ();
			shape.gameObject.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);

			foreach (Point p in shape.GetTileLocations (shape.location))
			{
				occupiedTiles.Remove (p);
			}

		}
		placedShapes.Clear ();
		if (currentShape != null)
		{
			currentShape.FreezePosition = false;
		}
		currentShape = null;
		EventManager.TriggerEvent (EventManager.EVENT_TYPE.SHAPE_REMOVED, null);
		CheckIsComplete ();
		RefreshGrid ();
	}
	public void SetCurrentShape (Shape shape)
	{
		currentShape = shape;
		currentShape.FreezePosition = true;
		currentShape.gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		// DebugPanel.StartChecking ("currentHover", () => currentHover.ToString ());
	}
	public void GetClickTarget ()
	{
		terminalUI.GetClickTarget ();
	}
}