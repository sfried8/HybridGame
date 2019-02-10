using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    // Start is called before the first frame update
    private bool initialized = false;
    public bool UnlockAllLevels;
    public List<int> levels = new List<int> ();
    public List<int> unlocked = new List<int> () { 1 };
    private static LevelSelection levelSelection;

    public RectTransform panel;
    public GameObject mainMenuButtonPrefab;

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
                    Debug.LogError ("There needs to be one active LevelSelection script on a GameObject in your scene.");
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
        if (initialized)
        {
            return;
        }
        initialized = true;
        DontDestroyOnLoad (this);
        EventManager.StartListening (EventManager.EVENT_TYPE.HEART_COLLECTED, LevelComplete);
        levels = new List<int> ();
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex (i);
            int lastSlash = scenePath.LastIndexOf ("/");
            string sceneName = (scenePath.Substring (lastSlash + 1, scenePath.LastIndexOf (".") - lastSlash - 1));
            int level;
            int.TryParse (sceneName.Replace ("Platformer", ""), out level);
            if (level != 0)
            {
                levels.Add (level);

            }
        }
        levels.Sort ();

        foreach (int level in levels)
        {
            GameObject button = Instantiate (mainMenuButtonPrefab);
            button.transform.SetParent (panel);
            button.GetComponent<MainMenuButton> ().level = level;
            button.GetComponentInChildren<Text> ().text = level.ToString ();
        }
    }
    void Start ()
    {
        if (GameObject.FindObjectsOfType (typeof (LevelSelection)).Length > 1)
        {
            Destroy (this.gameObject);
        }
        DontDestroyOnLoad (this.gameObject);
        Init ();
    }
    public static bool IsLevelUnlocked (int level)
    {
        return instance.UnlockAllLevels || instance.unlocked.Contains (level);
    }
    public static void OnClick (int level)
    {
        Debug.Log ("hello?");
        if (!IsLevelUnlocked (level))
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
    public void CloseApplication ()
    {
        Application.Quit ();
    }
}