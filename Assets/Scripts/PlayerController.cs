using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls inputActions;
    private Rigidbody2D theRB;

    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float dashSpeed = 15f;
    public float maxHoverTime = 3f;
    public float hoverCooldown = 7f;

    private Vector2 moveInput;
    private bool canDoubleJump;
    private bool canDash = true;
    private bool isHovering;
    private bool isHoverCooldown;
    private float hoverTimeRemaining;
    private float hoverHeight;

    public bool canMove = true;

    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;

    public Animator animPlayer;
    public BulletController shotToFire;
    public Transform shotPoint;
    public SpriteRenderer theSR, afterImage;
    public float afterImageLifetime, timeBetweenAfterImages;
    private float afterImageCounter;
    public Color afterImageColor;

    private void Awake()
    {
        inputActions = new PlayerControls();
        theRB = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        // Subscribe to input events
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMoveStop;
        inputActions.Player.Jump.performed += OnJump;
        //inputActions.Player.Dash.performed += OnDash;
        inputActions.Player.Attack.performed += OnShoot;
        inputActions.Player.Hover.performed += OnHoverStart;
        inputActions.Player.Hover.canceled += OnHoverEnd;
    }

    private void OnDisable()
    {
        inputActions.Disable();

        // Unsubscribe from input events
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMoveStop;
        inputActions.Player.Jump.performed -= OnJump;
        //inputActions.Player.Dash.performed -= OnDash;
        inputActions.Player.Attack.performed -= OnShoot;
        inputActions.Player.Hover.performed -= OnHoverStart;
        inputActions.Player.Hover.canceled -= OnHoverEnd;
    }

    private void Update()
    {
        if (canMove)
        {
            theRB.velocity = new Vector2(moveInput.x * moveSpeed, theRB.velocity.y);

            if (theRB.velocity.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
            else if (theRB.velocity.x > 0)
                transform.localScale = Vector3.one;
        }
        else
        {
            theRB.velocity = Vector2.zero; // Stop movement when canMove is false
        }

        // Check if on the ground
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);

        if (isHovering)
        {
            hoverTimeRemaining -= Time.deltaTime;
            theRB.gravityScale = 0;
            theRB.velocity = new Vector2(theRB.velocity.x, 0);

            if (hoverTimeRemaining <= 0)
            {
                StopHover();
            }
        }

        // Update animator
        animPlayer.SetBool("isOnGround", isOnGround);
        animPlayer.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }

    private void OnMoveStop(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (canMove && (isOnGround || canDoubleJump))
        {
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);

            if (!isOnGround)
            {
                canDoubleJump = false;
                animPlayer.SetTrigger("doubleJump");
            }
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (canMove && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        float originalGravity = theRB.gravityScale;
        theRB.gravityScale = 0;
        theRB.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);

        yield return new WaitForSeconds(0.3f);

        theRB.gravityScale = originalGravity;
        canDash = true;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);
            animPlayer.SetTrigger("shotFired");
        }
    }

    private void OnHoverStart(InputAction.CallbackContext context)
    {
        if (canMove && !isHovering && !isHoverCooldown)
        {
            isHovering = true;
            hoverTimeRemaining = maxHoverTime;
            hoverHeight = transform.position.y;
            animPlayer.SetBool("hover", true);
        }
    }

    private void OnHoverEnd(InputAction.CallbackContext context)
    {
        StopHover();
    }

    private void StopHover()
    {
        isHovering = false;
        isHoverCooldown = true;
        animPlayer.SetBool("hover", false);
        theRB.gravityScale = 1;

        StartCoroutine(HoverCooldown());
    }

    private IEnumerator HoverCooldown()
    {
        yield return new WaitForSeconds(hoverCooldown);
        isHoverCooldown = false;
    }

    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;
        Destroy(image.gameObject, afterImageLifetime);
        afterImageCounter = timeBetweenAfterImages;
    }
}
