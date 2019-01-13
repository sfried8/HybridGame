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
    public int[][][] grid;

    public Material materialRed;
    public Material materialGreen;
    public Material materialWhite;
    public GameObject[][][] Voxels;
    public Point location;

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
        int[][] face = Face ();
        List<Point> ret = new List<Point> ();
        for (int row = 0; row < face.Length; row++)
        {
            for (int col = 0; col < face[0].Length; col++)
            {
                if (face[row][col] == 1)
                {
                    ret.Add (Point.GridCoord (row + _location.Row - 1, col + _location.Col - 1));
                }
            }
        }
        return ret;
    }

    private List<GameObject>[][] rendererFace;
    private void updateRendererFace ()
    {
        rendererFace = new List<GameObject>[grid[0].Length][];

        for (int plane = 0; plane < grid.Length; plane++)
        {
            for (int row = 0; row < grid[0].Length; row++)
            {
                rendererFace[row] = new List<GameObject>[grid[0][0].Length];
                for (int col = 0; col < grid[0][0].Length; col++)
                {
                    if (grid[plane][row][col] == 1)
                    {
                        if (rendererFace[row][col] == null)
                        {
                            rendererFace[row][col] = new List<GameObject> ();
                        }
                        rendererFace[row][col].Add (Voxels[plane][row][col]);
                    }
                }

            }
        }
    }
    public int[][] Face ()
    {
        int[][] ret = Util.CreateZeroed2DIntArray (grid[0][0].Length, grid[0].Length);
        for (int plane = 0; plane < grid.Length; plane++)
        {
            for (int row = 0; row < grid[0].Length; row++)
            {
                for (int col = 0; col < grid[0][0].Length; col++)
                {
                    if (grid[plane][row][col] == 1)
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
        for (int row = 0; row < grid[0].Length; row++)
        {

            for (int col = 0; col < grid[0][0].Length; col++)
            {
                for (int plane = 0; plane < grid.Length; plane++)
                {
                    if (Voxels[plane][row][col] != null)
                    {
                        Voxels[plane][row][col].GetComponent<Renderer> ().materials = new Material[1] { materialGreen };
                        Voxels[plane][row][col].GetComponent<Renderer> ().material.color = clearColor;
                    }
                }
            }
        }
    }
    private Color greenColor = new Color (0.5f, 1.0f, 0.5f, 0.8f);
    private Color redColor = new Color (1.0f, 0.5f, 0.5f, 0.8f);
    public void ColorVoxels (int[][] slots)
    {
        // return;
        updateRendererFace ();
        // Debug.Log (slots.PrettyPrint ());
        // Debug.Log (rendererFace.PrettyPrint ());
        for (int row = slots.Length - 1; row >= 0; row--)
        {

            for (int col = 0; col < slots[0].Length; col++)
            {
                for (int plane = 0; plane < grid.Length; plane++)
                {
                    if (Voxels[plane][row][col] != null)
                    {
                        if (slots[2 - row][2 - col] == 1)
                        {
                            // Voxels[plane][row][col].GetComponent<Renderer> ().materials.color = greenColor;
                            Voxels[plane][row][col].GetComponent<Renderer> ().materials = new Material[1] { materialGreen };

                        }
                        else
                        {
                            Voxels[plane][row][col].GetComponent<Renderer> ().materials = new Material[1] { materialRed };
                            // Voxels[plane][row][col].GetComponent<Renderer> ().material.color = redColor;

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
        int[][][] newGrid = Util.CreateZeroed3DIntArray (grid); // grid[0].Length, grid[0][0].Length];
        GameObject[][][] newVoxels = Util.CreateNulled3DGameObjectArray (Voxels); //, Voxels[0].Length, Voxels[0][0].Length];
        if (dir == ROTATE_DIRECTION.ROLL)
        {

            quaternion = Quaternion.AngleAxis (reverse ? -90 : 90, Vector3.forward) * quaternion;

            for (int plane = 0; plane < grid.Length; plane++)
            {
                for (int row = 0; row < grid[0].Length; row++)
                {
                    for (int col = 0; col < grid[0][0].Length; col++)
                    {
                        if (reverse)
                        {
                            newGrid[plane][row][col] = grid[plane][2 - col][row];
                            newVoxels[plane][row][col] = Voxels[plane][2 - col][row];
                        }
                        else
                        {
                            newGrid[plane][2 - col][row] = grid[plane][row][col];
                            newVoxels[plane][2 - col][row] = Voxels[plane][row][col];
                        }
                    }

                }
            }
        }
        else if (dir == ROTATE_DIRECTION.YAW)
        {
            quaternion = Quaternion.AngleAxis (reverse ? 90 : -90, Vector3.up) * quaternion;
            for (int plane = 0; plane < grid.Length; plane++)
            {

                for (int row = 0; row < grid[0].Length; row++)
                {

                    for (int col = 0; col < grid[0][0].Length; col++)
                    {

                        if (reverse)
                        {
                            newGrid[plane][row][col] = grid[col][row][2 - plane];
                            newVoxels[plane][row][col] = Voxels[col][row][2 - plane];
                        }
                        else
                        {
                            newGrid[col][row][2 - plane] = grid[plane][row][col];
                            newVoxels[col][row][2 - plane] = Voxels[plane][row][col];
                        }

                    }

                }
            }

        }
        else if (dir == ROTATE_DIRECTION.PITCH)
        {
            quaternion = Quaternion.AngleAxis (reverse ? -90 : 90, Vector3.right) * quaternion;
            for (int plane = 0; plane < grid.Length; plane++)
            {

                for (int row = 0; row < grid[0].Length; row++)
                {

                    for (int col = 0; col < grid[0][0].Length; col++)
                    {

                        if (reverse)
                        {
                            newGrid[plane][row][col] = grid[2 - row][plane][col];
                            newVoxels[plane][row][col] = Voxels[2 - row][plane][col];
                        }
                        else
                        {
                            newGrid[2 - row][plane][col] = grid[plane][row][col];
                            newVoxels[2 - row][plane][col] = Voxels[plane][row][col];
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
        grid = Util.Create3DIntArrayFromString (gridSource, 3, 3, 3);
        Voxels = Util.CreateNulled3DGameObjectArray (grid[0][0].Length, grid[0].Length, grid.Length); //, grid[0].Length, grid[0][0].Length];
        for (int plane = 0; plane < grid.Length; plane++)
        {
            for (int row = 0; row < grid[0].Length; row++)
            {
                for (int col = 0; col < grid[0][0].Length; col++)
                {
                    if (grid[plane][row][col] == 1)
                    {
                        GameObject voxel = (GameObject) Instantiate (voxelPrefab);
                        voxel.transform.SetParent (gameObject.transform);
                        voxel.transform.localPosition = new Vector3 (col - 1, 1 - row, plane - 1);
                        voxel.transform.localScale = new Vector3 (0.95f, 0.95f, 0.95f);
                        Voxels[plane][row][col] = voxel;
                        voxel.name = string.Format ("{0},{1},{2}", plane, row, col);
                    }
                }
            }
        }
        quaternion = Quaternion.identity;
        targetPosition = transform.localPosition;
    }

    public void Scale (Vector3 scale)
    {
        foreach (GameObject[][] x in Voxels)
        {
            foreach (GameObject[] y in x)
            {
                foreach (GameObject z in y)
                {
                    if (z != null)
                    {
                        z.transform.localScale = scale;
                    }
                }
            }
        }
        // gameObject.transform.localScale = new Vector3(scale.x/2,scale.y/2,scale.z/2);
        gameObject.transform.localScale = scale;
    }
    float smooth = 5.0f;
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

}