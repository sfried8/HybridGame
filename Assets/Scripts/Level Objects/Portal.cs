using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public PortalGroup portalGroup;
    public bool PortalActive = true;
    private bool playerIsNear = false;

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
            PortalActive = true;
            // StartCoroutine (WaitForPlayerToLeave (5f, () => PortalActive = true));

        }
    }
    // Start is called before the first frame update
    void Start ()
    {
        portalGroup = GetComponentInParent<PortalGroup> ();
    }
    void Update ()
    {
        if (playerIsNear)
        {
            if (Input.GetKeyDown (KeyCode.F))
            {
                portalGroup.PortalEntered (this);
            }
        }
    }
    IEnumerator WaitForPlayerToLeave (float seconds, Util.VoidFunction action)
    {
        float endTime = Time.time + seconds;
        while (endTime < Time.time)
        {
            if (playerIsNear)
            {
                endTime = Time.time + seconds;
            }
            yield return null;
        }
        action ();
    }
}