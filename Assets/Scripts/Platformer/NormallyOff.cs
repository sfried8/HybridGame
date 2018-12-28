using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormallyOff : MonoBehaviour
{
    // Start is called before the first frame update
    public Grid terminalGrid;
    private Vector3 spawnPoint;
    private Quaternion spawnRotation;
    void Start ()
    {
        EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_COMPLETE, OnComplete);
        EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_INCOMPLETE, OnIncomplete);
        spawnPoint = transform.position;
        spawnRotation = transform.rotation;
        gameObject.SetActive (false);
    }
    public void OnComplete (EventInfo info)
    {
        TerminalPuzzleInfo tpi = (TerminalPuzzleInfo)info;
        if (tpi?.terminalGrid != terminalGrid) {
            return;
        }
        transform.position = spawnPoint;
        transform.rotation = spawnRotation;
        gameObject.SetActive (true);
    }
    public void OnIncomplete (EventInfo info)
    {
        TerminalPuzzleInfo tpi = (TerminalPuzzleInfo)info;
        if (tpi?.terminalGrid != terminalGrid) {
            return;
        }
        gameObject.SetActive (false);
    }
}