using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPrompt : MonoBehaviour
{
    private GameObject keyPrompt;
    public Vector3 offsetPosition;
    public string key;
    // Start is called before the first frame update
    void Start ()
    {
        keyPrompt = new GameObject ("Key Prompt");
        keyPrompt.transform.SetParent (transform);
        keyPrompt.transform.localPosition = offsetPosition;
        keyPrompt.transform.localScale = new Vector3 (0.92f, 0.92f, 0.92f);
        SpriteRenderer sr = keyPrompt.AddComponent<SpriteRenderer> ();
        sr.sprite = Resources.Load<Sprite> ("Sprites/Light/Keyboard_White_" + key.ToUpper ());
        keyPrompt.SetActive (false);
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