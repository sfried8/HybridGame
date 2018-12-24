using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

  public float smoothSpeed;
  public Transform target;

  public Vector3 offset;
  void Update ()
  {
    Vector3 specificVector = new Vector3 (target.position.x + offset.x, target.position.y + offset.y, transform.position.z + offset.z);
    transform.position = Vector3.Lerp (transform.position, specificVector, smoothSpeed * Time.deltaTime);

  }
}