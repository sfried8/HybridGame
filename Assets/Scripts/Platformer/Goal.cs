using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public int level;
    public SineWaveMovement swm;
    void Start ()
    {
        level = int.Parse (SceneManager.GetActiveScene ().name.Replace ("Platformer", ""));
    }
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag ("Player"))
        {
            EventManager.TriggerEvent (EventManager.EVENT_TYPE.HEART_COLLECTED, new CollectGoalInfo () { level = level });
            StartCoroutine (EndLevel ());
            // EventManager.TriggerEvent (EventManager.EVENT_TYPE.COIN_COLLECTED);
        }

    }

    IEnumerator EndLevel ()
    {
        swm = GetComponent<SineWaveMovement> ();
        while (swm.RotationVelocityY < 1500)
        {
            swm.RotationVelocityY += 5;
            yield return null;
        }
        yield return new WaitForSeconds (3);
        SceneManager.LoadScene ("MainMenu");
        yield return null;
    }
}