using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    // Start is called before the first frame update
    private Text textField;
    public delegate string StringFunction ();
    public Dictionary<string, StringFunction> debugFuncs;
    void Awake ()
    {
        debugFuncs = new Dictionary<string, StringFunction> ();
        textField = GetComponentInChildren<Text> ();
    }

    // Update is called once per frame
    void Update ()
    {
        string result = "";
        foreach (string key in debugFuncs.Keys)
        {
            result += key + ": " + debugFuncs[key] () + "\n";
        }

        textField.text = result;

    }
    private static DebugPanel _instance;
    public static DebugPanel instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType (typeof (DebugPanel)) as DebugPanel;
            }
            return _instance;
        }
    }
    public static void StartChecking (string name, StringFunction value)
    {
        if (!instance)
        {
            return;
        }
        int x = 0;
        while (instance.debugFuncs.ContainsKey (name + x))
        {
            x += 1;
        }
        instance.debugFuncs.Add (name + x, value);
    }
    public static void StopChecking (string name)
    {
        if (!instance)
        {
            return;
        }
        instance.debugFuncs.Remove (name);
    }
}