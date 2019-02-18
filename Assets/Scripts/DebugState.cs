using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugState : MonoBehaviour
{

    // Start is called before the first frame update
    public int State = 0;
    public int numStates = 1;
    public KeyCode toggleKey = KeyCode.None;
    private bool initialized = false;
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        if (!initialized)
        {
            initialized = true;
            DebugPanel.StartChecking ("State", () => (State + 1) + "/" + numStates);
        }
        if (Input.GetKeyDown (toggleKey))
        {
            State = (State + 1) % (numStates);
        }
    }
}