using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag ("Player"))
        {
            (other.gameObject.GetComponent<Player> () as Player).controlsActive = false;
            this.SetTimeout (() => SceneManager.LoadScene (SceneManager.GetActiveScene ().name), 1f);
        }
    }
}