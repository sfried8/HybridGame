using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Terminal : MonoBehaviour
{

	// Use this for initialization
	public Camera terminalCamera;
	public GameObject screenModel;
	private bool playerIsNear = false;
	private TerminalGrid grid;
	void Start ()
	{
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_COMPLETE, OnComplete);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_INCOMPLETE, OnIncomplete);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_BACK_PRESSED, OnBackPressed);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_RESTART_PRESSED, OnRestartPressed);
		grid = GetComponent<TerminalGrid> ();
		var texture = new Texture2D (8, 8, TextureFormat.ARGB32, false);
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				if (grid.puzzles[0].Grid[7 - j, i] == 1)
				{
					texture.SetPixel (i, j, Color.white);
				}
				else
				{
					texture.SetPixel (i, j, Color.black);
				}
			}
		}
		// set the pixel values
		texture.filterMode = FilterMode.Point;
		// Apply all SetPixel calls
		texture.Apply ();
		screenModel.GetComponent<Renderer> ().material.mainTexture = texture;
		// connect texture to material of GameObject this script is attached to
	}

	void OnBackPressed (EventInfo info)
	{
		if (grid.isActive)
		{
			ToggleActive ();
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
	void ToggleActive ()
	{
		bool wasActive = terminalCamera.gameObject.activeSelf;
		if (wasActive)
		{
			grid.OnDeactivated ();
			EventManager.TriggerEvent (EventManager.EVENT_TYPE.TERMINAL_DEACTIVATED, null);
		}
		else
		{
			grid.OnActivated ();
			EventManager.TriggerEvent (EventManager.EVENT_TYPE.TERMINAL_ACTIVATED, null);
		}
		terminalCamera.gameObject.SetActive (!wasActive);
	}
	void Update ()
	{
		if (playerIsNear && !grid.isActive && Input.GetKeyDown (KeyCode.F))
		{
			ToggleActive ();
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
		ToggleActive ();
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

}