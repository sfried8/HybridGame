using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterController2D _controller;
    EdgeCollider2D _collider;
    void Start ()
    {
        _collider = GetComponent<EdgeCollider2D> ();
        _controller = FindObjectOfType (typeof (CharacterController2D)) as CharacterController2D;
        _controller.onControllerCollidedEvent += onControllerCollider;
    }

    void onControllerCollider (RaycastHit2D hit)
    {
        if (hit.collider == _collider && _controller.collisionState.below && !_controller.collisionState.above)
        {
            _controller.move (new Vector3 (_controller.velocity.x, 17f, 0f) * Time.deltaTime);
            StartCoroutine (Compress ());
            // _controller.velocity.y = 5f;
        }
    }
    IEnumerator Compress ()
    {
        for (var i = 0; i < 5; i++)
        {
            transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y - 0.09f, transform.localScale.z);
            yield return null;
        }
        for (var i = 0; i < 15; i++)
        {
            transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y + 0.03f, transform.localScale.z);
            yield return null;
        }
        transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
    }
}