using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGroup : MonoBehaviour
{
    public List<Portal> Portals;

    // Start is called before the first frame update
    public void PortalEntered (Portal p)
    {
        if (p.PortalActive)
        {
            Portal otherP = Portals.Find ((other) => other != p && other.gameObject.activeSelf && other.PortalActive);
            // otherP.PortalActive = false;
            (FindObjectOfType (typeof (Player)) as Player).gameObject.transform.position = (otherP.gameObject.transform.position + new Vector3 (0, 1f, 0));
            (FindObjectOfType (typeof (CharacterController2D)) as CharacterController2D).velocity = Vector3.zero;
        }
    }
    void Start ()
    {
        Portals = new List<Portal> (GetComponentsInChildren<Portal> ());
    }

    // Update is called once per frame
    void Update ()
    {

    }
}