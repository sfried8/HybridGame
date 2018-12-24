using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour, ITerminalListener
{

	// Use this for initialization
	public Camera terminalCamera;
	public GameObject screenModel;
	private SpriteRenderer keyPrompt;
	private bool playerIsNear = false;
	private Grid grid;
	void Start ()
	{
		EventManager.StartListening (EventManager.Event.COIN_COLLECTED, new UnityEngine.Events.UnityAction (OnCoinCollected));
		keyPrompt = GetComponentInChildren<SpriteRenderer> (true);
		grid = GetComponent<Grid> ();
		grid.AddTerminalListener (this);
	}

	void OnCoinCollected ()
	{
		// transform.localScale *= 1.1f;
	}

	void ToggleActive ()
	{
		bool wasActive = terminalCamera.gameObject.activeSelf;
		if (wasActive)
		{
			grid.OnDeactivated ();
			EventManager.TriggerEvent (EventManager.Event.TERMINAL_DEACTIVATED);
		}
		else
		{
			grid.OnActivated ();
			EventManager.TriggerEvent (EventManager.Event.TERMINAL_ACTIVATED);
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

	public void OnComplete ()
	{
		ToggleActive ();
		screenModel.GetComponent<Renderer> ().material.color = Color.green;
	}

	public void OnIncomplete ()
	{
		screenModel.GetComponent<Renderer> ().material.color = Color.white;
	}

}