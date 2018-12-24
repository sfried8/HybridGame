using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, ITerminalListener
{
    // Start is called before the first frame update
    public Grid terminalGrid;
    private Vector3 spawnPoint;
    private Quaternion spawnRotation;
    void Start ()
    {
        terminalGrid.AddTerminalListener (this);
        spawnPoint = transform.position;
        spawnRotation = transform.rotation;
    }
    public void OnComplete ()
    {
        gameObject.SetActive (false);
        Application.LoadLevel ("Platformer1");
    }
    public void OnIncomplete ()
    {
        transform.position = spawnPoint;
        transform.rotation = spawnRotation;
        gameObject.SetActive (true);
    }
}