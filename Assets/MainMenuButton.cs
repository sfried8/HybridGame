using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    public int level;
    void Awake ()
    {
        if (!LevelSelection.instance.unlocked.Contains (level))
        {
            GetComponent<Button> ().interactable = false;
            GetComponentInChildren<Text> ().color = Color.gray;
        }
    }
    public void OnClick ()
    {
        LevelSelection.OnClick (level);
    }
}