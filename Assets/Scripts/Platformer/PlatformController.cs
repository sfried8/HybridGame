using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class PlatformController : MonoBehaviour
{

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;
    [HideInInspector] public bool wallJump = false;

    public Vector3 spawnPoint;
    public float moveForce = 365f;
    public float maxHorizontalSpeed = 5f;
    public float maxVerticalSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;
    public Transform frontWallCheck;
    public Transform backWallCheck;
    public float dy;
    private int wallJumpTimer = 0;
    private static int WALL_JUMP_DELAY = 30;

    public bool wallJumpWasFacingRight = false;
    public bool grounded = false;
    public bool againstFrontWall = false;
    public bool againstBackWall = false;

    public float wallMultiplier;
    private Rigidbody2D rb2d;

    private Quaternion backwardsQuaternion = Quaternion.Euler (0, -180, 0);
    public float rotationVelocity;

    private bool controlsActive = true;

    // Use this for initialization
    void Awake ()
    {
        spawnPoint = transform.position;
        rb2d = GetComponent<Rigidbody2D> ();
        EventManager.StartListening (EventManager.Event.TERMINAL_ACTIVATED, new UnityAction (() => controlsActive = false));
        EventManager.StartListening (EventManager.Event.TERMINAL_DEACTIVATED, new UnityAction (() => controlsActive = true));
    }

    // Update is called once per frame
    void Update ()
    {
        grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
        againstFrontWall = Physics2D.Linecast (transform.position, frontWallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
        againstBackWall = Physics2D.Linecast (transform.position, backWallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
        if (controlsActive)
        {

            if (Input.GetKeyDown (KeyCode.Space) && grounded)
            {
                jump = true;
            }
            else if (Input.GetKeyDown (KeyCode.Space) && (againstFrontWall || againstBackWall))
            {
                wallJump = true;
            }
        }
        dy = rb2d.velocity.y;
        transform.rotation = Quaternion.Slerp (transform.rotation, facingRight ? Quaternion.identity : backwardsQuaternion, Time.deltaTime * rotationVelocity);
        if (transform.position.y < -25)
        {
            Restart ();
        }

    }

    void FixedUpdate ()
    {
        if (controlsActive)
        {

            float h = Input.GetAxis ("Horizontal");
            wallMultiplier = 1.0f - (wallJumpTimer / (float) WALL_JUMP_DELAY);
            if (facingRight != wallJumpWasFacingRight)
            {
                wallMultiplier = 1.0f;
            }

            if (h * rb2d.velocity.x < maxHorizontalSpeed)
                rb2d.AddForce (Vector2.right * h * moveForce * wallMultiplier * (againstFrontWall || againstBackWall? 0.5f : 1.0f));

        }
        wallJumpTimer = wallJumpTimer <= 1 ? 0 : wallJumpTimer - 1;
        if (Mathf.Abs (rb2d.velocity.x) > maxHorizontalSpeed)
            rb2d.velocity = new Vector2 (Mathf.Sign (rb2d.velocity.x) * maxHorizontalSpeed, rb2d.velocity.y);

        if (rb2d.velocity.x > 0.5 && !facingRight)
            Flip ();
        else if (rb2d.velocity.x < -0.5 && facingRight)
            Flip ();

        if (jump)
        {
            rb2d.AddForce (new Vector2 (0f, jumpForce));
            jump = false;
        }
        if (wallJump)
        {

            rb2d.AddForce ((facingRight ? Vector2.left : Vector2.right) * 8 * moveForce);
            rb2d.AddForce (new Vector2 (0f, jumpForce));

            Flip ();
            wallJumpTimer = WALL_JUMP_DELAY;
            wallJumpWasFacingRight = facingRight;
            wallJump = false;
        }
        if (Mathf.Abs (rb2d.velocity.y) > maxVerticalSpeed)
            rb2d.velocity = new Vector2 (rb2d.velocity.x, Mathf.Sign (rb2d.velocity.y) * maxVerticalSpeed);
        dy = rb2d.velocity.y;
    }

    void Flip ()
    {
        facingRight = !facingRight;

    }
    public void Restart ()
    {
        transform.position = spawnPoint;
        transform.rotation = Quaternion.identity;
        rb2d.velocity = Vector3.zero;
        facingRight = true;
        Debug.Log ("restarted");
    }
}