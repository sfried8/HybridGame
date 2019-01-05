using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    // Start is called before the first frame update
    int[] levels = new int[] { 1, 2, 3 };
    public List<int> unlocked = new List<int> () { 1 };
    private static LevelSelection levelSelection;

    public static LevelSelection instance
    {
        get
        {
            if (!levelSelection)
            {
                levelSelection = FindObjectOfType (typeof (LevelSelection)) as LevelSelection;
                DontDestroyOnLoad (levelSelection);
                if (!levelSelection)
                {
                    Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    levelSelection.Init ();
                }
            }

            return levelSelection;
        }
    }

    void Init ()
    {
        DontDestroyOnLoad (this);
        EventManager.StartListening (EventManager.EVENT_TYPE.HEART_COLLECTED, LevelComplete);
    }
    void Start ()
    {

        Object[] objs = GameObject.FindObjectsOfType (typeof (LevelSelection));

        if (objs.Length > 1)
        {
            Destroy (this.gameObject);
        }

        DontDestroyOnLoad (this.gameObject);

    }
    public static void OnClick (int level)
    {
        if (!instance.unlocked.Contains (level))
        {
            return;
        }
        EventManager.ClearEvents ();
        EventManager.StartListening (EventManager.EVENT_TYPE.HEART_COLLECTED, instance.LevelComplete);
        SceneManager.LoadScene ("Platformer" + level);
    }
    public void LevelComplete (EventInfo info)
    {
        CollectGoalInfo cgi = (CollectGoalInfo) info;
        if (!unlocked.Contains (cgi.level + 1))
        {
            unlocked.Add (cgi.level + 1);

        }
    }
}