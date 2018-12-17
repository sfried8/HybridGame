using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{

	// Use this for initialization
	public Camera terminalCamera;
	private SpriteRenderer keyPrompt;
	private bool playerIsNear = false;
	private Grid grid;
	void Start ()
	{
		EventManager.StartListening (EventManager.Event.COIN_COLLECTED, new UnityEngine.Events.UnityAction (OnCoinCollected));
		keyPrompt = GetComponentInChildren<SpriteRenderer> (true);
		grid = GetComponent<Grid> ();
	}

	void OnCoinCollected ()
	{
		// transform.localScale *= 1.1f;
	}

	void Update ()
	{
		if (playerIsNear && Input.GetKeyDown (KeyCode.F))
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
}