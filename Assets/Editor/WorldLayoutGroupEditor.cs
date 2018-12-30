using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldLayoutGroup))]
public class WorldLayoutGroupEditor : Editor {


	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		WorldLayoutGroup myWorldLayoutGroup = (WorldLayoutGroup)target;
        myWorldLayoutGroup.verticalSpacing = EditorGUILayout.Slider(myWorldLayoutGroup.verticalSpacing,0f,10f);
		myWorldLayoutGroup.UpdateSpacing();
	}
}