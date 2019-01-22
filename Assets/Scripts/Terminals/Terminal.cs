using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Terminal : MonoBehaviour
{

	// Use this for initialization
	public Camera terminalCamera;
	public GameObject screenModel;
	private bool playerIsNear = false;
	public TerminalGrid grid;
	[ContextMenu ("Create Texture")]
	public void CreateTexture ()
	{
		if (grid == null)
		{
			grid = GetComponent<TerminalGrid> ();
		}
		var texture = new Texture2D (12, 8, TextureFormat.ARGB32, false);
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 12; j++)
			{
				if (j < 2 || j > 9)
				{
					texture.SetPixel (j, i, Color.black);
				}
				else
				{

					if (grid.puzzles[0].Grid[7 - i, j - 2] == 1)
					{
						texture.SetPixel (j, i, Color.white);
					}
					else
					{
						texture.SetPixel (j, i, Color.black);
					}
				}
			}
		}
		// set the pixel values
		texture.filterMode = FilterMode.Point;
		// Apply all SetPixel calls
		texture.Apply ();
		screenModel.GetComponent<Renderer> ().sharedMaterial.mainTexture = texture;
	}
	void Start ()
	{
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_COMPLETE, OnComplete);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_INCOMPLETE, OnIncomplete);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_BACK_PRESSED, OnBackPressed);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_RESTART_PRESSED, OnRestartPressed);
		CreateTexture ();
		// connect texture to material of GameObject this script is attached to
	}

	void OnBackPressed (EventInfo info)
	{
		if (grid.isActive)
		{
			ToggleActive (false);
		}
	}
	void OnRestartPressed (EventInfo info)
	{
		if (grid.isActive)
		{
			grid.ReturnAllToInventory ();
			// this.SetTimeout (ToggleActive, 1.5f);
		}
	}
	void ToggleActive (bool activate)
	{

		if (activate && !grid.isActive)
		{
			grid.OnActivated ();
			EventManager.TriggerEvent (EventManager.EVENT_TYPE.TERMINAL_ACTIVATED, null);

		}
		else if (!activate && grid.isActive)
		{
			grid.OnDeactivated ();
			EventManager.TriggerEvent (EventManager.EVENT_TYPE.TERMINAL_DEACTIVATED, null);
		}
		terminalCamera.gameObject.SetActive (activate);
	}
	void Update ()
	{
		if (playerIsNear && !grid.isActive && Input.GetKeyDown (KeyCode.F))
		{
			ToggleActive (true);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			playerIsNear = true;
		}
	}
	void OnTriggerExit2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			playerIsNear = false;
		}
	}

	public void OnComplete (EventInfo info)
	{
		TerminalPuzzleInfo tpi = (TerminalPuzzleInfo) info;
		if (tpi?.terminalGrid != grid)
		{
			return;
		}
		ToggleActive (false);
		screenModel.GetComponent<Renderer> ().material.color = Color.green;
	}

	public void OnIncomplete (EventInfo info)
	{
		TerminalPuzzleInfo tpi = (TerminalPuzzleInfo) info;
		if (tpi?.terminalGrid != grid)
		{
			return;
		}
		screenModel.GetComponent<Renderer> ().material.color = Color.white;
	}

	private void OnDrawGizmos ()
	{
		if (grid?.gameObject != null && grid.gameObject != gameObject)
		{

			Gizmos.color = Color.green;
			Gizmos.DrawLine (transform.position, grid.transform.position);
		}
	}
}