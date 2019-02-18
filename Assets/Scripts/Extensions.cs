using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Extensions
{

	public static DebugState CreateStateObject (this MonoBehaviour mb, int numStates, KeyCode keyCode)
	{
		DebugState ds = mb.gameObject.AddComponent<DebugState> ();
		ds.numStates = numStates;
		ds.toggleKey = keyCode;
		return ds;
	}
	public static void SetTimeout (this MonoBehaviour mb, Util.VoidFunction action, float seconds)
	{
		mb.StartCoroutine (Util.setTimeoutHelper (action, seconds));
	}
	public static Point ToPoint (this Vector3 vector)
	{
		return new Point ((int) vector.x, (int) vector.y, (int) vector.z);
	}
	public static string PrettyPrint (this int[, ] grid)
	{

		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < grid.GetLength (0); i++)
		{
			for (int j = 0; j < grid.GetLength (1); j++)
			{
				sb.Append (grid[i, j] + " ");

			}
			sb.AppendLine ();
		}
		return sb.ToString ();
	}
	public static string PrettyPrint (this List<Renderer>[, ] grid)
	{
		int[, ] intgrid = new int[grid.GetLength (0), grid.GetLength (1)];
		for (int i = 0; i < grid.GetLength (0); i++)
		{
			for (int j = 0; j < grid.GetLength (1); j++)
			{
				if (grid[i, j] != null)
				{

					intgrid[i, j] = grid[i, j].Count;
				}
			}
		}
		return intgrid.PrettyPrint ();
	}
	public static void SetLayerRecursively (this GameObject obj, int newLayer)
	{

		if (null == obj)
		{
			return;
		}

		obj.layer = newLayer;

		foreach (Transform child in obj.transform)
		{
			if (null == child)
			{
				continue;
			}
			child.gameObject.SetLayerRecursively (newLayer);
		}

	}

	public static T[] GetColumn<T> (this T[, ] matrix, int columnNumber)
	{
		return Enumerable.Range (0, matrix.GetLength (0))
			.Select (x => matrix[x, columnNumber])
			.ToArray ();
	}

	public static T[] GetRow<T> (this T[, ] matrix, int rowNumber)
	{
		return Enumerable.Range (0, matrix.GetLength (1))
			.Select (x => matrix[rowNumber, x])
			.ToArray ();
	}

}