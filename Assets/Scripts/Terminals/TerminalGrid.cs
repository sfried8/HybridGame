using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerminalGrid : MonoBehaviour
{
	public GameObject buttonPrefab;
	private RectTransform panel;
	public Shape currentShape;
	private List<Shape> placedShapes = new List<Shape> ();

	private Dictionary<Point, Shape> occupiedTiles = new Dictionary<Point, Shape> ();
	private Color[] colors = { Color.white, new Color (0.75f, 0.75f, 0.75f), Color.red };
	public Point currentHover = null;

	public List<GridPuzzle> puzzles = new List<GridPuzzle> ();
	private int puzzleIndex = 0;
	private int[, ] grid;

	private HashSet<Point> slots = new HashSet<Point> ();
	private GameObject[, ] buttons;
	// Use this for initialization

	private int rowCount;
	private int colCount;
	public bool isActive = false;
	private bool wasIncomplete = true;

	public Vector3 clickedVoxel;
	public void OnActivated ()
	{
		isActive = true;
		for (int row = 0; row < rowCount; row++)
		{
			for (int col = 0; col < colCount; col++)
			{
				buttons[row, col].SetActive (true);
			}
		}
		RefreshGrid ();
	}
	public void OnDeactivated ()
	{
		isActive = false;
		if (currentShape != null)
		{
			ReturnToInventory ();

		}
		ButtonHoverExit ();
		for (int row = 0; row < rowCount; row++)
		{
			for (int col = 0; col < colCount; col++)
			{
				buttons[row, col].SetActive (false);
			}
		}
	}
	void Start ()
	{
		panel = GameObject.FindGameObjectsWithTag ("GridPanel") [0].GetComponent<RectTransform> ();
		grid = puzzles[puzzleIndex].Grid;
		rowCount = grid.GetLength (0);
		colCount = grid.GetLength (1);
		buttons = new GameObject[rowCount, colCount];
		// CreateNewShape ();
		for (int row = 0; row < rowCount; row++)
		{
			for (int col = 0; col < colCount; col++)
			{

				GameObject buttonGameObject = (GameObject) Instantiate (buttonPrefab);
				buttonGameObject.transform.SetParent (panel, false);
				int newRow = row;
				int newCol = col;
				EventTrigger eventTrigger = buttonGameObject.GetComponent<EventTrigger> ();
				EventTrigger.Entry eventEntry = new EventTrigger.Entry ();
				eventEntry.eventID = EventTriggerType.PointerEnter;
				eventEntry.callback.AddListener ((data) => ButtonHover (newRow, newCol));
				eventTrigger.triggers.Add (eventEntry);
				EventTrigger.Entry eventExit = new EventTrigger.Entry ();
				eventExit.eventID = EventTriggerType.PointerExit;
				eventExit.callback.AddListener ((data) => ButtonHoverExit ());
				eventTrigger.triggers.Add (eventExit);
				buttons[row, col] = buttonGameObject;
				buttonGameObject.SetActive (false);
				if (grid[row, col] == 1)
				{
					slots.Add (Point.GridCoord (row, col));
				}
			}
		}

		RefreshGrid ();
	}
	void ButtonHover (int row, int col)
	{
		if (!isActive)
		{
			return;
		}
		currentHover = Point.GridCoord (Mathf.Clamp (row, 1, 6), Mathf.Clamp (col, 1, 6));

		RefreshGrid ();

	}
	void ButtonHoverExit ()
	{
		currentHover = null;
		currentShape?.ClearColorVoxels ();
		RefreshGrid ();
	}
	public void RefreshGrid ()
	{
		if (!isActive)
		{
			return;
		}
		for (int row = 0; row < rowCount; row++)
		{
			for (int col = 0; col < colCount; col++)
			{
				Image buttonImage = buttons[row, col].GetComponent<Image> ();
				buttonImage.color = colors[grid[row, col]];

			}
		}

		foreach (Point placedTile in occupiedTiles.Keys)
		{
			Image buttonImage = buttons[placedTile.Row, placedTile.Col].GetComponent<Image> ();
			if (grid[placedTile.Row, placedTile.Col] != 0)
			{
				buttonImage.color = new Color (0.4f, 1.0f, 0.4f);
			}
			else
			{
				buttonImage.color = Color.red;
			}
		}

		if (currentShape != null && currentHover != null)
		{
			List<Point> hoverTiles = currentShape.GetTileLocations (currentHover);
			foreach (Point hoverTile in hoverTiles)
			{
				if (hoverTile.Row >= 0 && hoverTile.Row < 8 && hoverTile.Col >= 0 && hoverTile.Col < 8)
				{

					Image buttonImage = buttons[hoverTile.Row, hoverTile.Col].GetComponent<Image> ();
					if (grid[hoverTile.Row, hoverTile.Col] == 1)
					{
						buttonImage.color = new Color (buttonImage.color.r * 0.5f, buttonImage.color.g, buttonImage.color.b * 0.5f);
					}
					else
					{
						buttonImage.color = new Color (buttonImage.color.r, buttonImage.color.g * 0.5f, buttonImage.color.b * 0.5f);
					}
				}

			}

			// int height = currentShape.FaceHeight ();
			// int width = currentShape.FaceWidth ();
			int height = currentShape.height;
			int width = currentShape.width;
			int[, ] hoverSection = new int[height, width];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (i + currentHover.Row >= 1 && i + currentHover.Row <= rowCount && j + currentHover.Col >= 1 && j + currentHover.Col <= colCount)
					{
						int hsy = height - i - 1;
						int hsx = width - j - 1;

						hoverSection[hsy, hsx] = grid[i + currentHover.Row - currentShape.CenterPoint.Y + currentShape.OffsetPoint.Y, j + currentHover.Col - currentShape.CenterPoint.X + currentShape.OffsetPoint.X];
					}
				}
			}
			currentShape.ColorVoxels (hoverSection);
		}
		if (currentShape == null && currentHover != null)
		{
			if (occupiedTiles.ContainsKey (currentHover))
			{
				Shape hoverShape = occupiedTiles[currentHover];
				foreach (Point p in hoverShape.GetTileLocations (hoverShape.location))
				{
					if (p.Row >= 0 && p.Row < 8 && p.Col >= 0 && p.Col < 8)
					{

						Image buttonImage = buttons[p.Row, p.Col].GetComponent<Image> ();
						if (grid[p.Row, p.Col] == 1)
						{
							buttonImage.color = new Color (buttonImage.color.r * 0.25f, buttonImage.color.g * 0.5f, buttonImage.color.b * 0.25f);
						}
						else
						{
							buttonImage.color = new Color (buttonImage.color.r * 0.5f, buttonImage.color.g * 0.25f, buttonImage.color.b * 0.25f);
						}
					}
				}
			}
		}

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
	}
	public void GetClickTarget ()
	{
		RaycastHit hit;
		Ray ray = Camera.allCameras[1].ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit))
			if (hit.transform != null)
			{
				Shape clickedShape = hit.transform.parent.GetComponent<Shape> ();
				if (clickedShape != null)
				{
					clickedVoxel = hit.transform.localPosition;
					clickedVoxel.z = 0;
					if (currentShape != null)
					{
						// currentShape.gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 0.9f);
					}
					SetCurrentShape (clickedShape);
					// 
				}
			}
	}
	public void CyclePuzzle ()
	{
		puzzleIndex = (puzzleIndex + 1) % puzzles.Count;
		grid = puzzles[puzzleIndex].Grid;
		slots.Clear ();
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
		RefreshGrid ();
		CheckIsComplete ();
	}
	// Update is called once per frame
	static bool mouseActive = true;

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.H))
		{
			mouseActive = !mouseActive;
		}
		if (isActive && mouseActive && currentShape != null && !Input.GetMouseButton (1))
		{
			Vector3 pos = Input.mousePosition;
			pos.z = 10; //currentShape.gameObject.transform.position.z - Camera.allCameras[1].transform.position.z;
			currentShape.DragToPosition (Camera.allCameras[1].ScreenToWorldPoint (pos)) /* + Camera.allCameras[1].ScreenToWorldPoint (clickedVoxel)*/ ;

			// currentShape.targetPosition = currentShape.gameObject.transform.localPosition;
		}

	}
}