using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterController2D _controller;
    EdgeCollider2D _collider;
    Vector3 lastPosition = Vector3.zero;
    private bool playerIsOn = false;
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
            playerIsOn = true;

            // _controller.move (new Vector3 (_controller.velocity.x, 17f, 0f) * Time.deltaTime);
            // StartCoroutine (Compress ());
            // _controller.velocity.y = 5f;
        }
    }

    void Update ()
    {
        if (playerIsOn)
        {
            if (lastPosition != Vector3.zero)
            {
                _controller.move (new Vector3 (0f, _controller.velocity.y + transform.position.y - lastPosition.y, 0f));
            }
            playerIsOn = false;
        }
        lastPosition = transform.position;
    }

}