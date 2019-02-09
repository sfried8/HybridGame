using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public GameObject inventory;

    public TerminalGrid terminalGravityGrid;
    public void CollectCoin (Shape shape)
    {

    }
    public void AddToInventory (GameObject shape)
    {
        inventory.GetComponent<Inventory> ().AddShape (shape);
    }
    public float gravity = -25f;
    private float gravityBase = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;

    public bool controlsActive = true;

    public void OnTerminalComplete (EventInfo info)
    {
        TerminalPuzzleInfo tpi = (TerminalPuzzleInfo) info;
        if (tpi?.terminalGrid != terminalGravityGrid)
        {
            return;
        }
        gravity = gravityBase / 2;
    }

    public void OnTerminalIncomplete (EventInfo info)
    {
        TerminalPuzzleInfo tpi = (TerminalPuzzleInfo) info;
        if (tpi?.terminalGrid != terminalGravityGrid)
        {
            return;
        }
        gravity = gravityBase;
    }
    void Awake ()
    {
        inventory = (FindObjectOfType (typeof (Inventory)) as Inventory).gameObject;
        _controller = GetComponent<CharacterController2D> ();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_ACTIVATED, (_) => controlsActive = false);
        EventManager.StartListening (EventManager.EVENT_TYPE.HEART_COLLECTED, (_) => controlsActive = false);
        EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_DEACTIVATED, (_) => controlsActive = true);

        EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_COMPLETE, OnTerminalComplete);
        EventManager.StartListening (EventManager.EVENT_TYPE.TERMINAL_INCOMPLETE, OnTerminalIncomplete);
    }

    #region Event Listeners

    void onControllerCollider (RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }

    void onTriggerEnterEvent (Collider2D col) { }

    void onTriggerExitEvent (Collider2D col) { }

    #endregion

    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.R))
        {
            StartCoroutine (HoldButtonForSeconds (KeyCode.R, 2f, () => SceneManager.LoadScene (SceneManager.GetActiveScene ().name)));
        }
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            StartCoroutine (HoldButtonForSeconds (KeyCode.Escape, 2f, () => SceneManager.LoadScene ("MainMenu")));
        }
        if (controlsActive)
        {
            MoveCharacter ();
        }
    }
    void MoveCharacter ()
    {
        if (_controller.isGrounded)
            _velocity.y = 0;
        if (_controller.isGrounded && Input.GetKeyDown (KeyCode.Space))
        {
            _velocity.y = Mathf.Sqrt (2f * jumpHeight * -gravityBase);

        }
        if (Input.GetKey (KeyCode.D))
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        else if (Input.GetKey (KeyCode.A))
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        else
        {
            normalizedHorizontalSpeed = 0;

        }

        // we can only jump whilst grounded

        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        _velocity.x = Mathf.Lerp (_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        _velocity.y += gravity * Time.deltaTime;

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (_controller.isGrounded && Input.GetKey (KeyCode.S))
        {
            _velocity.y *= 3f;
            _controller.ignoreOneWayPlatformsThisFrame = true;
        }

        _controller.move (_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;
    }
    IEnumerator HoldButtonForSeconds (KeyCode key, float seconds, Util.VoidFunction action)
    {
        float endTime = Time.time + seconds;
        while (Time.time <= endTime)
        {
            if (!Input.GetKey (key))
            {
                yield break;
            }
            yield return null;
        }
        action ();
    }
}