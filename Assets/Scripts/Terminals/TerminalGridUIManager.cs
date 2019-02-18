using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerminalGridUIManager : MonoBehaviour
{
    public TerminalGrid terminalGrid;
    public GameObject buttonPrefab;
    private RectTransform panel;

    private Color[] colors = { Color.white, new Color (0.75f, 0.75f, 0.75f), Color.red };

    private GameObject[, ] buttons;
    // Use this for initialization

    private float panelLeft;
    private float panelWidth;
    private float panelTop;
    private float panelHeight;
    public void OnActivated ()
    {
        for (int row = 0; row < terminalGrid.rowCount; row++)
        {
            for (int col = 0; col < terminalGrid.colCount; col++)
            {
                buttons[row, col].SetActive (true);
            }
        }
        RefreshGrid ();
        Vector3[] corners = new Vector3[4];
        panel.GetWorldCorners (corners);
        Vector3 bottomLeft = Camera.allCameras[1].WorldToScreenPoint (corners[0]);
        Vector3 topRight = Camera.allCameras[1].WorldToScreenPoint (corners[2]);
        panelLeft = bottomLeft.x;
        panelWidth = topRight.x - panelLeft;
        panelTop = topRight.y;
        panelHeight = bottomLeft.y - panelTop;

    }
    public void OnDeactivated ()
    {
        for (int row = 0; row < terminalGrid.rowCount; row++)
        {
            for (int col = 0; col < terminalGrid.colCount; col++)
            {
                buttons[row, col].SetActive (false);
            }
        }
    }
    void Start ()
    {
        terminalGrid = GetComponent<TerminalGrid> ();
        panel = GameObject.FindGameObjectsWithTag ("GridPanel") [0].GetComponent<RectTransform> ();
        buttons = new GameObject[terminalGrid.rowCount, terminalGrid.colCount];
        // CreateNewShape ();
        for (int row = 0; row < terminalGrid.rowCount; row++)
        {
            for (int col = 0; col < terminalGrid.colCount; col++)
            {

                GameObject buttonGameObject = (GameObject) Instantiate (buttonPrefab);
                buttonGameObject.transform.SetParent (panel, false);
                int newRow = row;
                int newCol = col;

                buttons[row, col] = buttonGameObject;
                buttonGameObject.SetActive (false);
            }
        }

        RefreshGrid ();
    }
    public void RefreshGrid ()
    {

        for (int row = 0; row < terminalGrid.rowCount; row++)
        {
            for (int col = 0; col < terminalGrid.colCount; col++)
            {
                Image buttonImage = buttons[row, col].GetComponent<Image> ();
                buttonImage.color = colors[terminalGrid.grid[row, col]];

            }
        }

        foreach (Point placedTile in terminalGrid.occupiedTiles.Keys)
        {
            Image buttonImage = buttons[placedTile.Row, placedTile.Col].GetComponent<Image> ();
            if (terminalGrid.grid[placedTile.Row, placedTile.Col] != 0)
            {
                buttonImage.color = new Color (0.4f, 1.0f, 0.4f);
            }
            else
            {
                buttonImage.color = Color.red;
            }
        }

        List<Point> hoverTiles = terminalGrid.GetHoverTiles ();
        if (hoverTiles != null)
        {
            foreach (Point hoverTile in hoverTiles)
            {
                if (hoverTile.Row >= 0 && hoverTile.Row < 8 && hoverTile.Col >= 0 && hoverTile.Col < 8)
                {

                    Image buttonImage = buttons[hoverTile.Row, hoverTile.Col].GetComponent<Image> ();
                    if (terminalGrid.grid[hoverTile.Row, hoverTile.Col] == 1)
                    {
                        buttonImage.color = new Color (buttonImage.color.r * 0.5f, buttonImage.color.g, buttonImage.color.b * 0.5f);
                    }
                    else
                    {
                        buttonImage.color = new Color (buttonImage.color.r, buttonImage.color.g * 0.5f, buttonImage.color.b * 0.5f);
                    }
                }

            }

            terminalGrid.ColorCurrentShapeVoxels ();
        }
        List<Point> hoverShapeTiles = terminalGrid.GetHoverShapeTileLocations ();
        if (hoverShapeTiles != null)
        {

            foreach (Point p in hoverShapeTiles)
            {
                if (p.Row >= 0 && p.Row < 8 && p.Col >= 0 && p.Col < 8)
                {

                    Image buttonImage = buttons[p.Row, p.Col].GetComponent<Image> ();
                    if (terminalGrid.grid[p.Row, p.Col] == 1)
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

                    terminalGrid.SetCurrentShape (clickedShape);
                    // 
                }
            }
    }

    // Update is called once per frame
    static bool mouseActive = true;

    void calculateButtonHoverFromMousePosition (bool isOddX, bool isOddY)
    {
        Vector3 pos = Input.mousePosition;
        Vector3 relativeMousePosition = mousePositionToButtonPosition (pos, isOddX, isOddY);
        if (relativeMousePosition.x < 0 || relativeMousePosition.x > 8 || relativeMousePosition.y < 0 || relativeMousePosition.y > 8)
        {

            terminalGrid.ButtonHoverExit ();

        }
        else
        {
            int row = (int) relativeMousePosition.y;
            int col = (int) relativeMousePosition.x;

            terminalGrid.ButtonHover (row, col);

        }
    }
    private Vector3 mousePositionToButtonPosition (Vector3 mousePosition, bool isOddX, bool isOddY)
    {
        float xPercent = ((mousePosition.x - panelLeft + (isOddX ? -22.5f : 0)) / panelWidth);
        float yPercent = ((mousePosition.y - panelTop + (isOddY ? 22.5f : 0)) / panelHeight);
        return new Vector3 (xPercent * 8.0f, yPercent * 8.0f, 0);
    }
    void Update ()
    {
        if (terminalGrid.isActive)
        {
            calculateButtonHoverFromMousePosition (terminalGrid.currentShape?.isOddX??false, terminalGrid.currentShape?.isOddY??false);

            if (mouseActive && terminalGrid.currentShape != null && !Input.GetMouseButton (1))
            {
                Vector3 pos = Input.mousePosition;
                // Debug.Log (mousePositionToButtonPosition (pos));

                pos.z = 10; //currentShape.gameObject.transform.position.z - Camera.allCameras[1].transform.position.z;
                terminalGrid.currentShape.DragToPosition (Camera.allCameras[1].ScreenToWorldPoint (pos)) /* + Camera.allCameras[1].ScreenToWorldPoint (clickedVoxel)*/ ;

                // currentShape.targetPosition = currentShape.gameObject.transform.localPosition;
            }
        }

    }
}