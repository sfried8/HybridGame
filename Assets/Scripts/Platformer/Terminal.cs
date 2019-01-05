using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Terminal : MonoBehaviour
{

	// Use this for initialization
	public Camera terminalCamera;
	public GameObject screenModel;
	private SpriteRenderer keyPrompt;
	private bool playerIsNear = false;
	private TerminalGrid grid;
	void Start ()
	{
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_COMPLETE, OnComplete);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_INCOMPLETE, OnIncomplete);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_BACK_PRESSED, OnBackPressed);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_RESTART_PRESSED, OnRestartPressed);
		keyPrompt = GetComponentInChildren<SpriteRenderer> (true);
		grid = GetComponent<TerminalGrid> ();
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
			keyPrompt.gameObject.SetActive (true);
		}
	}
	void OnTriggerExit2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			playerIsNear = false;
			keyPrompt.gameObject.SetActive (false);
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