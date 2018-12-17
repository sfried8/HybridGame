using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PlatformController))]
public class PlatformerControllerEditor : Editor {

	// Use this for initialization
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		PlatformController myController = (PlatformController)target;

		if (GUILayout.Button("Restart")) {
			myController.Restart();
		}
	}
}
