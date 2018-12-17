using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, ITerminalListener
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
        gameObject.SetActive (false);
    }
    public void OnComplete ()
    {
        transform.position = spawnPoint;
        transform.rotation = spawnRotation;
        gameObject.SetActive (true);
    }
    public void OnIncomplete ()
    {
        gameObject.SetActive (false);
    }
}