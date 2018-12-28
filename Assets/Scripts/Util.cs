using System.Collections;
using System.Linq;
using UnityEngine;

public class Util
{

	public static int[][] CreateZeroed2DIntArray (int width, int height)
	{
		int[][] ret = new int[height][];
		for (int i = 0; i < height; i++)
		{
			ret[i] = new int[width];
			for (int j = 0; j < width; j++)
			{
				ret[i][j] = 0;
			}
		}
		return ret;
	}
	public static int[][] CreateZeroed2DIntArray (int[][] basedOn)
	{
		return CreateZeroed2DIntArray (basedOn[0].Length, basedOn.Length);
	}
	public static int[][][] CreateZeroed3DIntArray (int width, int height, int depth)
	{
		int[][][] ret = new int[depth][][];
		for (int i = 0; i < depth; i++)
		{
			ret[i] = CreateZeroed2DIntArray (width, height);
		}
		return ret;
	}
	public static int[][][] CreateZeroed3DIntArray (int[][][] basedOn)
	{
		return CreateZeroed3DIntArray (basedOn[0][0].Length, basedOn[0].Length, basedOn.Length);
	}
	public static GameObject[][] CreateNulled2DGameObjectArray (int width, int height)
	{
		GameObject[][] ret = new GameObject[height][];
		for (int i = 0; i < height; i++)
		{
			ret[i] = new GameObject[width];
			for (int j = 0; j < width; j++)
			{
				ret[i][j] = null;
			}
		}
		return ret;
	}
	public static GameObject[][] CreateNulled2DGameObjectArray (GameObject[][] basedOn)
	{
		return CreateNulled2DGameObjectArray (basedOn[0].Length, basedOn.Length);
	}
	public static GameObject[][][] CreateNulled3DGameObjectArray (int width, int height, int depth)
	{
		GameObject[][][] ret = new GameObject[depth][][];
		for (int i = 0; i < depth; i++)
		{
			ret[i] = CreateNulled2DGameObjectArray (width, height);
		}
		return ret;
	}
	public static GameObject[][][] CreateNulled3DGameObjectArray (GameObject[][][] basedOn)
	{
		return CreateNulled3DGameObjectArray (basedOn[0][0].Length, basedOn[0].Length, basedOn.Length);
	}

	public static int[][] Create2DIntArrayFromString (string source, int height, int width)
	{
		int[][] ret = CreateZeroed2DIntArray (width, height);
		char[][] rows = source.Split (new char[] { '\n' }).Select ((r) => r.Trim ().ToCharArray ()).ToArray ();
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				ret[i][j] = int.Parse (rows[i][j].ToString ());
			}
		}
		return ret;
	}

	public static int[][][] Create3DIntArrayFromString (string source, int depth, int width, int height)
	{
		int[][][] ret = CreateZeroed3DIntArray (width, height, depth);
		string[] planes = source.Split (new string[] { "\n\r\n", "\n\n", "||" }, System.StringSplitOptions.None);
		for (int i = 0; i < depth; i++)
		{
			ret[i] = Create2DIntArrayFromString (planes[i], height, width);
		}
		return ret;
	}
}