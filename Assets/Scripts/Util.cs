using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Util
{
	public delegate void VoidFunction ();
	public static IEnumerator setTimeoutHelper (VoidFunction action, float seconds)
	{
		yield return new WaitForSecondsRealtime (seconds);
		action ();
		yield return null;
	}
	public static int[, , ] Create3DIntArrayFromString (string source)
	{

		string[] planes = source.Split (new string[] { "\n\r\n", "\n\n", "||" }, System.StringSplitOptions.None);
		char[][] rows0 = planes[0].Split (new char[] { '\n' }).Select ((r) => r.Trim ().ToCharArray ()).ToArray ();
		int depth = planes.Length;
		int height = rows0.Length;
		int width = rows0[0].Length;
		int[, , ] ret = new int[depth, height, width];

		for (int i = 0; i < depth; i++)
		{
			char[][] rows = planes[i].Split (new char[] { '\n' }).Select ((r) => r.Trim ().ToCharArray ()).ToArray ();
			for (int j = 0; j < height; j++)
			{
				for (int k = 0; k < width; k++)
				{

					ret[i, j, k] = int.Parse (rows[j][k].ToString ());
				}
			}
		}
		return ret;
	}
	public enum ROTATE_DIRECTION
	{
		ROLL,
		PITCH,
		YAW
	}
	public static T[, , ] RotateArray<T> (T[, , ] arr, ROTATE_DIRECTION dir, bool reverse)
	{
		T[, , ] newArr = null; // arr.GetLength(1), arr.GetLength(2)];
		if (dir == ROTATE_DIRECTION.ROLL)
		{
			newArr = new T[arr.GetLength (0), arr.GetLength (2), arr.GetLength (1)];
			for (int plane = 0; plane < arr.GetLength (0); plane++)
			{
				for (int row = 0; row < arr.GetLength (1); row++)
				{
					for (int col = 0; col < arr.GetLength (2); col++)
					{
						if (reverse)
						{
							newArr[plane, row, col] = arr[plane, arr.GetLength (2) - col - 1, row];
						}
						else
						{
							newArr[plane, arr.GetLength (2) - col - 1, row] = arr[plane, row, col];
						}
					}

				}
			}
		}
		else if (dir == ROTATE_DIRECTION.YAW)
		{
			newArr = new T[arr.GetLength (2), arr.GetLength (1), arr.GetLength (0)];
			for (int plane = 0; plane < arr.GetLength (0); plane++)
			{

				for (int row = 0; row < arr.GetLength (1); row++)
				{

					for (int col = 0; col < arr.GetLength (2); col++)
					{

						if (reverse)
						{
							newArr[plane, row, col] = arr[col, row, arr.GetLength (0) - plane - 1];
						}
						else
						{
							newArr[col, row, arr.GetLength (0) - plane - 1] = arr[plane, row, col];
						}

					}

				}
			}

		}
		else if (dir == ROTATE_DIRECTION.PITCH)
		{
			newArr = new T[arr.GetLength (1), arr.GetLength (0), arr.GetLength (2)];
			for (int plane = 0; plane < arr.GetLength (0); plane++)
			{

				for (int row = 0; row < arr.GetLength (1); row++)
				{

					for (int col = 0; col < arr.GetLength (2); col++)
					{

						if (reverse)
						{
							newArr[plane, row, col] = arr[arr.GetLength (1) - row - 1, plane, col];
						}
						else
						{
							newArr[arr.GetLength (1) - row - 1, plane, col] = arr[plane, row, col];
						}

					}

				}
			}
		}

		return newArr;

	}
	public static int[, ] RemoveRowsFromArray (int[, ] arr, int removeLeft, int removeTop, int removeRight, int removeBottom)
	{
		if (removeTop == 0 && removeLeft == 0 && removeRight == 0 && removeBottom == 0)
		{
			return arr;
		}
		int[, ] newArr = new int[arr.GetLength (0) - removeTop - removeBottom, arr.GetLength (1) - removeLeft - removeRight];
		for (int i = removeTop; i < arr.GetLength (0) - removeBottom; i++)
		{
			for (int j = removeLeft; j < arr.GetLength (1) - removeRight; j++)
			{
				newArr[i - removeTop, j - removeLeft] = arr[i, j];
			}
		}
		return newArr;
	}

	private static bool allZeroes (int[] arr)
	{
		foreach (int i in arr)
		{
			if (i != 0)
			{
				return false;
			}
		}
		return true;
	}
	public static int NumTopZeroRows (int[, ] arr)
	{
		for (int i = 0; i < arr.GetLength (0); i++)
		{
			if (!allZeroes (arr.GetRow (i)))
			{
				return i;
			}
		}
		return 0;
	}
	public static int NumLeftZeroCols (int[, ] arr)
	{
		for (int i = 0; i < arr.GetLength (1); i++)
		{
			if (!allZeroes (arr.GetColumn (i)))
			{
				return i;
			}

		}
		return 0;
	}
	public static int[, ] TrimArray (int[, ] arr)
	{
		int top = -1;
		int bottom = -1;
		int left = -1;
		int right = -1;

		for (int i = 0; i < arr.GetLength (0); i++)
		{
			if (top < 0 && !allZeroes (arr.GetRow (i)))
			{
				top = i;
			}
			if (bottom < 0 && !allZeroes (arr.GetRow (arr.GetLength (0) - i - 1)))
			{
				bottom = i;
			}
		}

		for (int i = 0; i < arr.GetLength (1); i++)
		{
			if (left < 0 && !allZeroes (arr.GetColumn (i)))
			{
				left = i;
			}
			if (right < 0 && !allZeroes (arr.GetColumn (arr.GetLength (1) - i - 1)))
			{
				right = i;
			}
		}

		return RemoveRowsFromArray (arr, left, top, right, bottom);
	}
	public static Vector3 GeometricCenter (int[, ] arr)
	{
		int[, ] trimmedArr = TrimArray (arr);
		int trimmedHeight = trimmedArr.GetLength (0);
		int trimmedWidth = trimmedArr.GetLength (1);
		float trimmedCenterX = (trimmedWidth - 1) / 2.0f;
		float trimmedCenterY = (trimmedHeight - 1) / 2.0f;
		float marginX = NumLeftZeroCols (arr);
		float marginY = NumTopZeroRows (arr);
		return new Vector3 (marginX + trimmedCenterX, marginY + trimmedCenterY, 0);

	}
	public static bool IsInRange (int x, int lowInclusive, int highExclusive)
	{
		return x >= lowInclusive && x < highExclusive;
	}

}