using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPrompt : MonoBehaviour
{
    private SpriteRenderer keyPrompt;
    // Start is called before the first frame update
    void Start ()
    {
        keyPrompt = GetComponentInChildren<SpriteRenderer> (true);

    }
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag ("Player"))
        {
            keyPrompt.gameObject.SetActive (true);
        }
    }
    void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.CompareTag ("Player"))
        {
            keyPrompt.gameObject.SetActive (false);
        }
    }
}