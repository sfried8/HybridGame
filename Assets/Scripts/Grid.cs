using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
	public GameObject buttonPrefab;
	public RectTransform panel;

	public List<GameObject> inventory;
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
	void Start ()
	{
		grid = puzzles[puzzleIndex].Grid;
		rowCount = grid.GetLength (0);
		colCount = grid.GetLength (1);
		panel = GetComponent<RectTransform> ();
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

				if (grid[row, col] == 1)
				{
					slots.Add (Point.GridCoord (row, col));
				}
			}
		}
		float spacing = -1.75f;
		foreach (GameObject inventoryShapeGO in inventory)
		{

			GameObject inventoryShapeInstance = Instantiate (inventoryShapeGO.gameObject);
			inventoryShapeInstance.transform.position = new Vector3 (5.88f, spacing, 0f);
			// inventoryShapeInstance.GetComponentInChildren<Shape>().Scale(new Vector3(0.75f,0.75f,0.75f));
			spacing += 2.75f;
		}
		RefreshGrid ();
	}
	public void RightClickDrag (Vector3 drag)
	{

	}
	void ButtonHover (int row, int col)
	{
		currentHover = Point.GridCoord (Mathf.Clamp (row, 1, 6), Mathf.Clamp (col, 1, 6));

		RefreshGrid ();

	}
	void ButtonHoverExit ()
	{
		currentHover = null;
		RefreshGrid ();
	}
	public void RefreshGrid ()
	{
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

			int[][] hoverSection = new int[3][];
			for (int i = 0; i < 3; i++)
			{
				hoverSection[2 - i] = new int[3];
				for (int j = 0; j < 3; j++)
				{
					if (i + currentHover.Row >= 1 && i + currentHover.Row <= rowCount && j + currentHover.Col >= 1 && j + currentHover.Col <= colCount)
					{
						hoverSection[2 - i][2 - j] = grid[i + (currentHover.Row - 1), j + (currentHover.Col - 1)];
					}
				}
			}
			currentShape.ColorVoxels (hoverSection);
		}

	}
	void CheckIsComplete ()
	{
		if (occupiedTiles.Count == slots.Count && slots.All ((p) => occupiedTiles.ContainsKey (p)))
		{
			Debug.Log ("You win!");
		}
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
		currentShape.gameObject.SetActive (false);
		currentShape = null;
		CheckIsComplete ();
	}
	public void RemoveCurrentShape ()
	{
		if (!occupiedTiles.ContainsKey (currentHover))
		{
			return;
		}
		Shape clickedShape = occupiedTiles[currentHover];
		clickedShape.gameObject.SetActive (true);
		if (placedShapes.Contains (clickedShape))
		{

			foreach (Point p in clickedShape.GetTileLocations (clickedShape.location))
			{
				occupiedTiles.Remove (p);
			}
		}
		currentShape = clickedShape;
	}
	public void GetClickTarget ()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit))
			if (hit.transform != null)
			{
				Shape clickedShape = hit.transform.parent.GetComponent<Shape> ();
				if (clickedShape != null)
				{
					if (currentShape != null)
					{
						currentShape.gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 0.9f);
					}
					currentShape = clickedShape;
					currentShape.gameObject.transform.localScale = new Vector3 (1.1f, 1.1f, 1.1f);
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
	void Update ()
	{
		if (currentShape != null && !Input.GetMouseButton (1))
		{
			Vector3 pos = Input.mousePosition;
			pos.z = currentShape.gameObject.transform.position.z - Camera.main.transform.position.z;
			currentShape.gameObject.transform.position = Camera.main.ScreenToWorldPoint (pos);
		}

	}
}