using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shape : MonoBehaviour
{

    public GameObject voxelPrefab;
    [TextArea (11, 11)]
    public string gridSource;
    public int[, , ] grid;

    public Material materialRed;
    public Material materialGreen;
    public Material materialWhite;
    public GameObject[, , ] Voxels;
    public Point location;

    public int width;
    public int height;
    public int depth;
    private bool _freezeRotation = false;
    public bool FreezeRotation
    {
        get { return _freezeRotation; }
        set { _freezeRotation = value; }
    }

    public bool FreezePosition { get => _freezePosition; set => _freezePosition = value; }

    public Vector3 targetPosition;
    public List<Point> GetTileLocations (Point _location)
    {
        updateRendererFace ();
        List<List<int>> face = Face ();
        List<Point> ret = new List<Point> ();
        for (int row = 0; row < face.Count; row++)
        {

            for (int col = 0; col < face[0].Count; col++)
            {
                if (face[row][col] == 1)
                {
                    ret.Add (Point.GridCoord (row + _location.Row - 1, col + _location.Col - 1));
                }
            }
        }
        return ret;
    }

    private List<List<List<GameObject>>> rendererFace;
    private void updateRendererFace ()
    {
        rendererFace = new List<List<List<GameObject>>> ();

        for (int row = 0; row < grid.GetLength (1); row++)
        {
            rendererFace.Add (new List<List<GameObject>> ());
            for (int col = 0; col < grid.GetLength (2); col++)
            {
                rendererFace[row].Add (new List<GameObject> ());
                for (int plane = 0; plane < grid.GetLength (0); plane++)
                {
                    if (grid[plane, row, col] == 1)
                    {
                        rendererFace[row][col].Add (Voxels[plane, row, col]);
                    }
                }

            }
        }

    }
    public List<List<int>> Face ()
    {
        List<List<int>> ret = new List<List<int>> ();
        for (int row = 0; row < grid.GetLength (1); row++)
        {
            ret.Add (new List<int> ());
            for (int col = 0; col < grid.GetLength (2); col++)
            {
                ret[row].Add (0);
                for (int plane = 0; plane < grid.GetLength (0); plane++)
                {
                    if (grid[plane, row, col] == 1)
                    {
                        ret[row][col] = 1;
                    }
                }

            }
        }

        return ret;
    }

    private Color clearColor = new Color (1f, 1f, 1f, 53f / 255f);
    public void ClearColorVoxels ()
    {
        for (int row = 0; row < grid.GetLength (1); row++)
        {

            for (int col = 0; col < grid.GetLength (2); col++)
            {
                for (int plane = 0; plane < grid.GetLength (0); plane++)
                {
                    if (Voxels[plane, row, col] != null)
                    {
                        Voxels[plane, row, col].GetComponent<Renderer> ().materials = new Material[1] { materialGreen };
                        Voxels[plane, row, col].GetComponent<Renderer> ().material.color = clearColor;
                    }
                }
            }
        }
    }
    private Color greenColor = new Color (0.5f, 1.0f, 0.5f, 0.8f);
    private Color redColor = new Color (1.0f, 0.5f, 0.5f, 0.8f);
    public void ColorVoxels (int[, ] slots)
    {
        // return;
        updateRendererFace ();
        // Debug.Log (slots.PrettyPrint ());
        // Debug.Log (rendererFace.PrettyPrint ());
        for (int row = slots.GetLength (0) - 1; row >= 0; row--)
        {

            for (int col = 0; col < slots.GetLength (1); col++)
            {
                for (int plane = 0; plane < grid.GetLength (0); plane++)
                {
                    if (Voxels[plane, row, col] != null)
                    {
                        if (slots[2 - row, 2 - col] == 1)
                        {
                            // Voxels[plane,row,col].GetComponent<Renderer> ().materials.color = greenColor;
                            Voxels[plane, row, col].GetComponent<Renderer> ().materials = new Material[1] { materialGreen };

                        }
                        else
                        {
                            Voxels[plane, row, col].GetComponent<Renderer> ().materials = new Material[1] { materialRed };
                            // Voxels[plane,row,col].GetComponent<Renderer> ().material.color = redColor;

                        }
                    }
                }
            }
            // Console.Write (Environment.NewLine + Environment.NewLine);
        }
    }
    public enum ROTATE_DIRECTION
    {
        ROLL,
        PITCH,
        YAW
    }
    public Quaternion quaternion;
    public void rotate (ROTATE_DIRECTION dir, bool reverse)
    {
        int[, , ] newGrid = new int[grid.GetLength (0), grid.GetLength (1), grid.GetLength (2)]; // grid.GetLength(1), grid.GetLength(2)];
        GameObject[, , ] newVoxels = new GameObject[Voxels.GetLength (0), Voxels.GetLength (1), Voxels.GetLength (2)]; //Util.CreateNulled3DGameObjectArray (Voxels); //, Voxels[0].Length, Voxels[0,0].Length];
        if (dir == ROTATE_DIRECTION.ROLL)
        {

            quaternion = Quaternion.AngleAxis (reverse ? -90 : 90, Vector3.forward) * quaternion;

            for (int plane = 0; plane < grid.GetLength (0); plane++)
            {
                for (int row = 0; row < grid.GetLength (1); row++)
                {
                    for (int col = 0; col < grid.GetLength (2); col++)
                    {
                        if (reverse)
                        {
                            newGrid[plane, row, col] = grid[plane, width - col - 1, row];
                            newVoxels[plane, row, col] = Voxels[plane, width - col - 1, row];
                        }
                        else
                        {
                            newGrid[plane, width - col - 1, row] = grid[plane, row, col];
                            newVoxels[plane, width - col - 1, row] = Voxels[plane, row, col];
                        }
                    }

                }
            }
        }
        else if (dir == ROTATE_DIRECTION.YAW)
        {
            quaternion = Quaternion.AngleAxis (reverse ? 90 : -90, Vector3.up) * quaternion;
            for (int plane = 0; plane < grid.GetLength (0); plane++)
            {

                for (int row = 0; row < grid.GetLength (1); row++)
                {

                    for (int col = 0; col < grid.GetLength (2); col++)
                    {

                        if (reverse)
                        {
                            newGrid[plane, row, col] = grid[col, row, depth - plane - 1];
                            newVoxels[plane, row, col] = Voxels[col, row, depth - plane - 1];
                        }
                        else
                        {
                            newGrid[col, row, depth - plane - 1] = grid[plane, row, col];
                            newVoxels[col, row, depth - plane - 1] = Voxels[plane, row, col];
                        }

                    }

                }
            }

        }
        else if (dir == ROTATE_DIRECTION.PITCH)
        {
            quaternion = Quaternion.AngleAxis (reverse ? -90 : 90, Vector3.right) * quaternion;
            for (int plane = 0; plane < grid.GetLength (0); plane++)
            {

                for (int row = 0; row < grid.GetLength (1); row++)
                {

                    for (int col = 0; col < grid.GetLength (2); col++)
                    {

                        if (reverse)
                        {
                            newGrid[plane, row, col] = grid[height - row - 1, plane, col];
                            newVoxels[plane, row, col] = Voxels[height - row - 1, plane, col];
                        }
                        else
                        {
                            newGrid[height - row - 1, plane, col] = grid[plane, row, col];
                            newVoxels[height - row - 1, plane, col] = Voxels[plane, row, col];
                        }

                    }

                }
            }
        }

        grid = newGrid;
        Voxels = newVoxels;
        updateRendererFace ();
    }

    [ContextMenu ("Regenerate")]
    public void Regenerate ()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate (transform.GetChild (0).gameObject);
        }
        grid = Util.Create3DIntArrayFromString (gridSource);
        width = grid.GetLength (2);
        height = grid.GetLength (1);
        depth = grid.GetLength (0);
        Voxels = new GameObject[depth, height, width]; //, grid.GetLength(1), grid.GetLength(2)];
        for (int plane = 0; plane < grid.GetLength (0); plane++)
        {
            for (int row = 0; row < grid.GetLength (1); row++)
            {
                for (int col = 0; col < grid.GetLength (2); col++)
                {
                    if (grid[plane, row, col] == 1)
                    {
                        GameObject voxel = (GameObject) Instantiate (voxelPrefab);
                        voxel.transform.SetParent (gameObject.transform);
                        voxel.transform.localPosition = new Vector3 (col - 1, 1 - row, plane - 1);
                        voxel.transform.localScale = new Vector3 (0.95f, 0.95f, 0.95f);
                        Voxels[plane, row, col] = voxel;
                        voxel.name = string.Format ("{0},{1},{2}", plane, row, col);
                    }
                }
            }
        }
        quaternion = Quaternion.identity;
        targetPosition = transform.localPosition;
    }

    float smooth = 5.0f;
    [SerializeField]
    private bool _freezePosition;

    void Update ()
    {
        if (!FreezeRotation)
        {
            transform.rotation = Quaternion.Slerp (transform.rotation, quaternion, Time.deltaTime * smooth);
        }
        if (!FreezePosition)
        {
            transform.parent.localPosition = Vector3.Lerp (transform.parent.localPosition, targetPosition, Time.deltaTime * smooth * 5);
        }
    }

    public void DragToPosition (Vector3 point)
    {
        transform.parent.position = point;
    }
    public Vector3 CenterPoint
    {
        get
        {
            return transform.parent.localPosition;
        }
    }

}