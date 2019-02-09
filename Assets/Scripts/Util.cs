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
}