using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Puzzle", order = 1)]
public class GridPuzzle : ScriptableObject {

	 [TextArea(8,8)]
	public string PuzzleString;
	private int[,] grid;
	public int[, ] Grid {
		get {
			if (grid == null) {
				string[] splitString = PuzzleString.Split('\n');

				grid = new int[splitString.Length,splitString[0].Length];
				for (int i = 0; i < splitString.Length; i++) {
					for (int j = 0; j < splitString[i].Length; j++) {
						grid[i,j] = int.Parse(splitString[i].Substring(j,1));
					}
				}
			}
			return grid;
		}
	}

}
