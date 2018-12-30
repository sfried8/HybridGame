using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public int level;
    public SineWaveMovement swm;
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
            EventManager.TriggerEvent(EventManager.EVENT_TYPE.HEART_COLLECTED,null);
			StartCoroutine(EndLevel());
			// EventManager.TriggerEvent (EventManager.EVENT_TYPE.COIN_COLLECTED);
		}

	}

    IEnumerator EndLevel() {
            swm = GetComponent<SineWaveMovement>();
        while (swm.RotationVelocityY < 1500) {
            swm.RotationVelocityY += 5;
            yield return null;
        }
        yield return new WaitForSeconds(3);
        int nextLevel = (level%3)+1;
        SceneManager.LoadScene("Platformer"+nextLevel);
        yield return null;
    }
}
