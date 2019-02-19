using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGameObject : MonoBehaviour
{

    public Shape shape;
    public GameObject voxelPrefab;
    public Material materialRed;
    public Material materialGreen;
    public Material materialWhite;
    public GameObject[, , ] Voxels;

    private bool _freezeRotation = false;
    public bool FreezeRotation
    {
        get { return _freezeRotation; }
        set { _freezeRotation = value; }
    }

    public bool FreezePosition { get => _freezePosition; set => _freezePosition = value; }

    public Vector3 targetPosition;
    public Vector3 targetOffsetPosition;

    private Color clearColor = new Color (1f, 1f, 1f, 200f / 255f);
    public void ClearColorVoxels ()
    {
        for (int row = 0; row < shape.grid.GetLength (1); row++)
        {

            for (int col = 0; col < shape.grid.GetLength (2); col++)
            {
                for (int plane = 0; plane < shape.grid.GetLength (0); plane++)
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
    public void ColorVoxels (int[, ] slots)
    {

        for (int row = slots.GetLength (0) - 1; row >= 0; row--)
        {
            for (int col = 0; col < slots.GetLength (1); col++)
            {
                for (int plane = 0; plane < shape.grid.GetLength (0); plane++)
                {
                    if (Voxels[plane, row, col] == null)
                    {
                        continue;
                    }
                    if (slots[(slots.GetLength (0) - 1) - row, (slots.GetLength (1) - 1) - col] == 1)
                    {
                        Voxels[plane, row, col].GetComponent<Renderer> ().materials = new Material[1] { materialGreen };
                    }
                    else
                    {
                        Voxels[plane, row, col].GetComponent<Renderer> ().materials = new Material[1] { materialRed };
                    }
                    Color oldColor = Voxels[plane, row, col].GetComponent<Renderer> ().material.color;
                    Voxels[plane, row, col].GetComponent<Renderer> ().material.color = new Color (oldColor.r, oldColor.g, oldColor.b, 1);

                }
            }
        }
    }
    public Quaternion quaternion;

    private void UpdateDimensionOffsets ()
    {
        Vector3 geometricCenter = Util.GeometricCenter (shape.Face ());
        shape.isOddX = (geometricCenter.x - Math.Truncate (geometricCenter.x) >= 0.4f);
        shape.isOddY = (geometricCenter.y - Math.Truncate (geometricCenter.y) >= 0.4f);

        shape.OffsetPoint = geometricCenter.ToPoint ();

        targetOffsetPosition = Vector3.Scale (new Vector3 (1.0f, -1.0f, 0), shape.CenterPoint.ToVector3 () - geometricCenter);
    }
    public void rotate (Util.ROTATE_DIRECTION dir, bool reverse)
    {
        Voxels = Util.RotateArray (Voxels, dir, reverse); //Util.CreateNulled3DGameObjectArray (Voxels); //, Voxels[0].Length, Voxels[0,0].Length];
        if (dir == Util.ROTATE_DIRECTION.ROLL)
        {
            quaternion = Quaternion.AngleAxis (reverse ? -90 : 90, Vector3.forward) * quaternion;
        }
        else if (dir == Util.ROTATE_DIRECTION.YAW)
        {
            quaternion = Quaternion.AngleAxis (reverse ? 90 : -90, Vector3.up) * quaternion;
        }
        else if (dir == Util.ROTATE_DIRECTION.PITCH)
        {
            quaternion = Quaternion.AngleAxis (reverse ? -90 : 90, Vector3.right) * quaternion;
        }

        UpdateDimensionOffsets ();
    }

    public void Regenerate ()
    {
        shape = GetComponent<Shape> ();
        while (transform.childCount != 0)
        {
            DestroyImmediate (transform.GetChild (0).gameObject);
        }
        UpdateDimensionOffsets ();
        Voxels = new GameObject[shape.grid.GetLength (0), shape.grid.GetLength (1), shape.grid.GetLength (2)]; //, shape.grid.GetLength(1), shape.grid.GetLength(2)];
        for (int plane = 0; plane < shape.grid.GetLength (0); plane++)
        {
            for (int row = 0; row < shape.grid.GetLength (1); row++)
            {
                for (int col = 0; col < shape.grid.GetLength (2); col++)
                {
                    if (shape.grid[plane, row, col] == 1)
                    {
                        GameObject voxel = (GameObject) Instantiate (voxelPrefab);
                        voxel.transform.SetParent (gameObject.transform);
                        voxel.transform.localPosition = new Vector3 (col - (shape.width - 1) / 2.0f, (shape.height - 1) / 2.0f - row, plane - (shape.depth - 1) / 2.0f);
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
    private DebugState positionDebugState;
    void Awake ()
    {
        shape = GetComponent<Shape> ();

    }
    void Start ()
    {
        positionDebugState = this.CreateStateObject (3, KeyCode.Y);
        // DebugPanel.StartChecking ("targetOffsetPosition", () => targetOffsetPosition.ToString ());
        // DebugPanel.StartChecking ("centerPoint", () => shape?.CenterPoint?.ToVector3 ().ToString ());
    }
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
        transform.localPosition = Vector3.Lerp (transform.localPosition, targetOffsetPosition, Time.deltaTime * smooth);

    }

    public void DragToPosition (Vector3 point)
    {
        transform.parent.position = point;
    }

}