using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLayoutGroup : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 origin;
[HideInInspector]
    public float verticalSpacing;

    public void UpdateSpacing()
    {
        float spacing = 0f;
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf) {
                t.localPosition = new Vector3 (origin.x, origin.y + spacing, origin.z);
                spacing += verticalSpacing;
            }
        }
    }
}
