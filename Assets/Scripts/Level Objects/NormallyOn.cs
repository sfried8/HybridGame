using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (TerminalConnection))]
public class NormallyOn : MonoBehaviour
{
    // Start is called before the first frame update
    public float FadeTime;
    private Vector3 spawnPoint;
    private Quaternion spawnRotation;
    float startingOpacity;
    void Start ()
    {
        GetComponent<TerminalConnection> ().RegisterForTerminalEvents (OnComplete, OnIncomplete);
        spawnPoint = transform.position;
        spawnRotation = transform.rotation;
        startingOpacity = GetComponentInChildren<Renderer> ().material.color.a;
        if (FadeTime == 0f)
        {
            FadeTime = 1f;
        }
    }
    public void OnComplete (EventInfo info)
    {
        Fade (false);
    }
    public void OnIncomplete (EventInfo info)
    {
        transform.position = spawnPoint;
        transform.rotation = spawnRotation;
        Fade (true);
    }
    void Fade (bool fadeIn)
    {
        if (fadeIn)
        {
            gameObject.SetActive (true);
            StartCoroutine (FadeTo (startingOpacity, FadeTime));
        }
        else
        {
            StartCoroutine (FadeTo (0f, FadeTime));
        }
    }
    IEnumerator FadeTo (float aValue, float aTime)
    {
        Renderer renderer = GetComponentInChildren<Renderer> ();
        Color color = renderer.material.color;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color (color.r, color.g, color.b, Mathf.Lerp (color.a, aValue, t));
            renderer.material.color = newColor;
            yield return null;
        }
        if (renderer.material.color.a < 0.01f)
        {
            gameObject.SetActive (false);
        }
        yield return null;
    }
}