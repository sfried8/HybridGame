using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shape : MonoBehaviour
{

    public Material materialGreen;
    public Material materialRed;
    public GameObject voxelPrefab;
    public ShapeGameObject shapeGameObject
    {
        get
        {
            if (_shapeGameObject == null)
            {
                _shapeGameObject = GetComponent<ShapeGameObject> () ?? gameObject.AddComponent (typeof (ShapeGameObject)) as ShapeGameObject;
                _shapeGameObject.voxelPrefab = voxelPrefab;
                _shapeGameObject.materialGreen = materialGreen;
                _shapeGameObject.materialRed = materialRed;
            }
            return _shapeGameObject;
        }
    }
    private ShapeGameObject _shapeGameObject;
    public string gridSource;
    public int[, , ] grid;

    public Point location;

    public int width { get => grid.GetLength (2); }
    public int height { get => grid.GetLength (1); }
    public int depth { get => grid.GetLength (0); }

    public int FaceWidth ()
    {
        int[, ] trimmedFace = Util.TrimArray (Face ());
        return trimmedFace.GetLength (1);
    }

    public int FaceHeight ()
    {
        int[, ] trimmedFace = Util.TrimArray (Face ());
        return trimmedFace.GetLength (0);
    }
    public int FaceLeftMargin ()
    {
        return Util.NumLeftZeroCols (Face ());
    }

    public int FaceTopMargin ()
    {
        return Util.NumTopZeroRows (Face ());
    }

    [SerializeField]
    private Point _centerPoint;
    public bool FreezeRotation { get => shapeGameObject.FreezeRotation; set => shapeGameObject.FreezeRotation = value; }
    public bool FreezePosition { get => shapeGameObject.FreezePosition; set => shapeGameObject.FreezePosition = value; }
    public Point CenterPoint { get { if (_centerPoint == null) { _centerPoint = Point.ONE (); } return _centerPoint; } set => _centerPoint = value; }

    [SerializeField]
    private Point offsetPoint;
    public Vector3 targetPosition { get => shapeGameObject.targetPosition; set => shapeGameObject.targetPosition = value; }
    public Point OffsetPoint { get => offsetPoint; set => offsetPoint = value; }

    public float buttonGridXOffset;
    public float buttonGridYOffset;
    public bool isOddX;
    public bool isOddY;
    public List<Point> GetTileLocations (Point _location)
    {

        int[, ] face = Face ();
        List<Point> ret = new List<Point> ();
        for (int row = 0; row < face.GetLength (0); row++)
        {

            for (int col = 0; col < face.GetLength (1); col++)
            {
                if (face[row, col] == 1)
                {
                    ret.Add (Point.GridCoord (row + _location.Row - OffsetPoint.Y, col + _location.Col - OffsetPoint.X));
                }
            }
        }
        return ret;
    }

    public int[, ] Face ()
    {
        int[, ] ret = new int[height, width];
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                for (int plane = 0; plane < depth; plane++)
                {
                    if (grid[plane, row, col] == 1)
                    {
                        ret[row, col] = 1;
                    }
                }

            }
        }

        return ret;
    }

    public void ClearColorVoxels ()
    {
        shapeGameObject.ClearColorVoxels ();
    }
    public void ColorVoxels (int[, ] slots)
    {
        shapeGameObject.ColorVoxels (slots);
    }

    public void rotate (Util.ROTATE_DIRECTION dir, bool reverse)
    {
        grid = Util.RotateArray (grid, dir, reverse);
        shapeGameObject.rotate (dir, reverse);
        // CenterOffset = new Point ((FaceWidth () - 1) / 2, (FaceHeight () - 1) / 2);
    }

    [ContextMenu ("Regenerate")]
    public void Regenerate ()
    {
        grid = Util.Create3DIntArrayFromString (gridSource);
        CenterPoint = new Point ((width - 1) / 2, (height - 1) / 2, (depth - 1) / 2);
        shapeGameObject.Regenerate ();
        OffsetPoint = Util.GeometricCenter (Face ()).ToPoint ();
    }
    public void DragToPosition (Vector3 point)
    {
        shapeGameObject.DragToPosition (point);
    }
}