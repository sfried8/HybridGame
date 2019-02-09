using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalGridBackButton : MonoBehaviour
{
    public Button backButton;
    public Button restartButton;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(()=>EventManager.TriggerEvent(EventManager.EVENT_TYPE.TERMINAL_BACK_PRESSED,null));
        restartButton.onClick.AddListener(()=>EventManager.TriggerEvent(EventManager.EVENT_TYPE.TERMINAL_RESTART_PRESSED,null));
    }

}
