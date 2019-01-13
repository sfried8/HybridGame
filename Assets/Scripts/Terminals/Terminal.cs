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
	[ContextMenu ("Create Texture")]
	public void CreateTexture ()
	{
		grid = GetComponent<TerminalGrid> ();
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