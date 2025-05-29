using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveInput { get; private set; }
    public bool isFacingLeft { get; private set; }
    public bool isTouchingGround { get; set; } = true;


    [HideInInspector] public Animator playerAnimator;
    [HideInInspector] public PlayerLives playerLivesScript;
    [HideInInspector] public Rigidbody2D rb;

    private float moveForce = 6f;
    private float jumpForce = 10.8f;
    private float multiplierFall = 3f;
    private float jumpBufferTime = 0.2f;
    private float coyoteTimeDuration = 0.1f;
    private float currentJumpBuffer;
    private float currentCoyoteTime;
    private bool isJumpButtonPressed;
    private Vector2 gravityVector;
    private bool isBeingKnockedBack;
    private PlayerAttack playerAttack;



    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerLivesScript = GetComponent<PlayerLives>();
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gravityVector = new Vector2(0, -Physics2D.gravity.y);
        CheckPointScript.lastCheckpointPosition = transform.position;
    }

    private void Update()
    {
        UpdateFacingDirection();
        UpdateCoyoteTime();
        UpdateJumpBuffer();
    }

    private void FixedUpdate()
    {
        playerAnimator.SetFloat("Yvelocity", rb.velocity.y);
        HandleMovement();
        HandleFallGravity();
        HandleJumpBuffer();
    }


    //Called by Input System
    public void PlayerMove(InputAction.CallbackContext callBack)
    {
        if (callBack.performed)
        {
            Vector2 input = callBack.ReadValue<Vector2>();
            moveInput = new Vector2(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y));
            if(moveInput.x != 0 && !isBeingKnockedBack)
            {
                playerAnimator.SetBool("Running", true);
            }
        }
        else if (callBack.canceled)
        {
            moveInput = Vector2.zero;
            playerAnimator.SetBool("Running", false);
        }
    }

    //Called by Input System
    public void Jump(InputAction.CallbackContext callBack)
    {
        if (callBack.performed && !isBeingKnockedBack)
        {
            if (GameManagerScript.gameMode == GameMode.Play)
            {
                isJumpButtonPressed = true;
                currentJumpBuffer = jumpBufferTime;
            }
        }
        else if (callBack.canceled)
        {
            isJumpButtonPressed = false;
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.7f);
            }
        }
    }

    public void ResetPlayer()
    {
        transform.position = CheckPointScript.lastCheckpointPosition;
        playerAnimator.SetTrigger("Reset");
        playerLivesScript.ResetLives();
    }
    public void PerformJump()
    {
        if (!isBeingKnockedBack && GameManagerScript.gameMode == GameMode.Play)
        {
            float actualJumpForce = isJumpButtonPressed ? jumpForce : jumpForce * 0.7f;
            rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
            isTouchingGround = false;
            currentCoyoteTime = 0; // Resetear el tiempo de coyote
            playerAttack.isPerformingAttackJump = false;
            SoundsManager.Instance.jumpSound.Play();
            playerAnimator.SetBool("Jumping", true);
        }

    }
    public void PerformJumpAttack()
    {

        if (!isBeingKnockedBack && GameManagerScript.gameMode == GameMode.Play)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isTouchingGround = false;
            currentCoyoteTime = 0; // Resetear el tiempo de coyote
            playerAttack.isPerformingAttackJump = true;
            SoundsManager.Instance.jumpSound.Play();
            playerAnimator.SetBool("Jumping", true);
        }
    }
    public void StartKnockUp(Vector2 direction)
    {
        StartCoroutine(KnockUp(direction));
    }
    private void UpdateFacingDirection()
    {
        if (!isBeingKnockedBack && GameManagerScript.gameMode == GameMode.Play)
        {
            Vector3 localScale = transform.localScale;
            if (moveInput.x < 0 && !isFacingLeft)
            {
                localScale.x = -1;
                transform.localScale = localScale;
                isFacingLeft = true;
            }
            else if (moveInput.x > 0 && isFacingLeft)
            {
                localScale.x = 1;
                transform.localScale = localScale;
                isFacingLeft = false;
            }
        }
    }
    private void UpdateJumpBuffer()
    {
        if (currentJumpBuffer > 0)
        {
            currentJumpBuffer -= Time.deltaTime;
        }
    }

    private void UpdateCoyoteTime()
    {
        if (!isTouchingGround)
        {
            if (currentCoyoteTime > 0)
            {
                currentCoyoteTime -= Time.deltaTime;
            }
        }
        else
        {
            currentCoyoteTime = coyoteTimeDuration;
        }
    }
    private void HandleMovement()
    {
        if (!isBeingKnockedBack)
        {
            rb.velocity = new Vector2(moveInput.x * moveForce, rb.velocity.y);
        }
    }

    private void HandleFallGravity()
    {
        if (rb.velocity.y < 0 || (!isJumpButtonPressed && !playerAttack.isPerformingAttackJump))
        {
            rb.velocity -= gravityVector * multiplierFall * Time.deltaTime;
        }
    }

    private void HandleJumpBuffer()
    {
        if (currentJumpBuffer > 0 && (isTouchingGround || currentCoyoteTime > 0))
        {
            PerformJump();
            currentJumpBuffer = 0;
        }
    }
    public IEnumerator KnockUp(Vector2 direction)
    {
        isBeingKnockedBack = true;
        rb.velocity = direction * 10;
        yield return new WaitForSecondsRealtime(0.08f);
        isBeingKnockedBack = false;
    }
}
