using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Data", menuName = "Data/Shape", order = 2)]
public class ShapeData : ScriptableObject
{
    [TextArea (11, 11)]
    public string ShapeString;
    private int[][][] grid;
    public int[][][] Grid
    {
        get
        {
            if (grid == null)
            {
                grid = Util.Create3DIntArrayFromString (ShapeString, 3, 3, 3);
            }
            return grid;
        }
    }
}