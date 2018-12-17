using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Extensions
{

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

}