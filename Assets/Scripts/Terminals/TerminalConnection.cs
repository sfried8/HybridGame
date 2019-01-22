using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalConnection : MonoBehaviour
{
    // Start is called before the first frame update
    public TerminalGrid terminalGrid;
    private EventManager.EventResponse OnComplete;
    private EventManager.EventResponse OnIncomplete;
    public void RegisterForTerminalEvents (EventManager.EventResponse onComplete, EventManager.EventResponse onIncomplete)
    {
        OnComplete = onComplete;
        OnIncomplete = onIncomplete;
        if (OnComplete != null)
        {
            EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_COMPLETE, internalOnComplete);
        }
        if (OnIncomplete != null)
        {
            EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_INCOMPLETE, internalOnIncomplete);
        }
    }
    private void internalOnComplete (EventInfo info)
    {
        TerminalPuzzleInfo tpi = (TerminalPuzzleInfo) info;
        if (tpi?.terminalGrid != terminalGrid)
        {
            return;
        }
        OnComplete (info);
    }
    private void internalOnIncomplete (EventInfo info)
    {
        TerminalPuzzleInfo tpi = (TerminalPuzzleInfo) info;
        if (tpi?.terminalGrid != terminalGrid)
        {
            return;
        }
        OnIncomplete (info);
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine (transform.position, terminalGrid.transform.position);
    }
}