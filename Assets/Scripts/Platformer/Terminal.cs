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
	private Grid grid;
	void Start ()
	{
		EventManager.StartListening (EventManager.EVENT_TYPE.COIN_COLLECTED, OnCoinCollected);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_COMPLETE, OnComplete);
		EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_INCOMPLETE, OnIncomplete);
		keyPrompt = GetComponentInChildren<SpriteRenderer> (true);
		grid = GetComponent<Grid> ();
	}

	void OnCoinCollected (EventInfo info)
	{
		// transform.localScale *= 1.1f;
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
		if (playerIsNear && Input.GetKeyDown (KeyCode.F))
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
		        TerminalPuzzleInfo tpi = (TerminalPuzzleInfo)info;
        if (tpi?.terminalGrid != grid) {
            return;
        }
		ToggleActive ();
		screenModel.GetComponent<Renderer> ().material.color = Color.green;
	}

	public void OnIncomplete (EventInfo info)
	{
		        TerminalPuzzleInfo tpi = (TerminalPuzzleInfo)info;
        if (tpi?.terminalGrid != grid) {
            return;
        }
		screenModel.GetComponent<Renderer> ().material.color = Color.white;
	}

}