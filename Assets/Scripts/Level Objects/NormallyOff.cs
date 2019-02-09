using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (TerminalConnection))]
public class NormallyOff : MonoBehaviour
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
        foreach (Renderer renderer in GetComponentsInChildren<Renderer> ())
        {

            startingOpacity = renderer.material.color.a;
            renderer.material.color = new Color (renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0f);
        }
        gameObject.SetActive (false);
        if (FadeTime == 0f)
        {
            FadeTime = 1f;
        }
    }
    public void OnComplete (EventInfo info)
    {
        transform.position = spawnPoint;
        transform.rotation = spawnRotation;
        Fade (true);
    }
    public void OnIncomplete (EventInfo info)
    {
        Fade (false);
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
        Renderer[] renderers = GetComponentsInChildren<Renderer> ();
        Color color = renderers[0].material.color;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color (color.r, color.g, color.b, Mathf.Lerp (color.a, aValue, t));
            foreach (Renderer renderer in GetComponentsInChildren<Renderer> ())
            {

                renderer.material.color = newColor;
            }
            yield return null;
        }
        if (renderers[0].material.color.a < 0.01f)
        {
            gameObject.SetActive (false);
        }
        yield return null;
    }
}