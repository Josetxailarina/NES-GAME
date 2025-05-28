

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveInput { get; private set; }
    public bool isFacingLeft { get; private set; }


    [HideInInspector] public Rigidbody2D rb;
    public float moveForce = 6f;
    public float jumpForce = 10.8f;
    public float multiplierFall = 3f;
    public float jumpBufferTime = 0.2f;
    public float maxFallSpeed = -10f;
    public float coyoteTime = 0.1f;
    private float jumpBufferCounter;
    public float coyoteTimeCounter;
    private bool jumpButtonHeld;
    public bool isTouchingGround { get; set; }
    public Animator playerAnimator;
    public Vector3 posicionLastCheckpoint;
    private Vector2 gravityVector;
    public bool isBeingKnockedBack;
    [HideInInspector] public PlayerLives playerHealth;
    private PlayerAttack playerAttack;



    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerLives>();
        isTouchingGround = true;
        rb = GetComponent<Rigidbody2D>();
        gravityVector = new Vector2(0, -Physics2D.gravity.y);
        playerAnimator = GetComponent<Animator>();
        posicionLastCheckpoint = transform.position;
    }

    public void Move(InputAction.CallbackContext callBack)
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
   private void UpdateFacingDirection()
    {
        if (!isBeingKnockedBack && GameManagerScript.modoJuego == GameMode.Play)
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
    private void Update()
    {
        UpdateFacingDirection();

        if (!isTouchingGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = 0;
        }

        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        playerAnimator.SetFloat("Yvelocity", rb.velocity.y);

        if (!isBeingKnockedBack)
        {
            rb.velocity = new Vector2(moveInput.x * moveForce, rb.velocity.y);
        }

        // Aplicar gravedad adicional para una caída más rápida
        if (rb.velocity.y < 0 || (!jumpButtonHeld && !playerAttack.isAttackLaunched))
        {
            rb.velocity -= gravityVector * multiplierFall * Time.deltaTime;
        }

        // Realizar el salto si hay un buffer de salto activo y está dentro del tiempo de coyote
        if (jumpBufferCounter > 0 && (isTouchingGround || coyoteTimeCounter < coyoteTime))
        {
            PerformJump();
            jumpBufferCounter = 0; // Resetear el buffer de salto después de saltar
        }

    }

    public void ResetSamurai()
    {
        transform.position = posicionLastCheckpoint;
        playerAnimator.SetTrigger("Reset");
        playerHealth.ResetLives();
    }
    public void Jump(InputAction.CallbackContext callBack)
    {
        if (callBack.performed && !isBeingKnockedBack)
        {
            if (GameManagerScript.modoJuego == GameMode.Play)
            {
                jumpButtonHeld = true;
                jumpBufferCounter = jumpBufferTime;
            }
        }

        if (callBack.canceled)
        {
            jumpButtonHeld = false; // Indicar que el botón de salto se ha soltado
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.7f); // Reducir la altura del salto si el botón se suelta temprano
            }
        }
    }

    public void PerformJump()
    {
        if (!isBeingKnockedBack && GameManagerScript.modoJuego == GameMode.Play)
        {
            float actualJumpForce = jumpButtonHeld ? jumpForce : jumpForce * 0.7f;
            rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
            isTouchingGround = false;
            coyoteTimeCounter = coyoteTime; // Resetear el tiempo de coyote
            playerAttack.isAttackLaunched = false;
            SoundsManager.Instance.jumpSound.Play();
            playerAnimator.SetBool("Jumping", true);
        }

    }
    public void PerformJumpAttack()
    {

        if (!isBeingKnockedBack && GameManagerScript.modoJuego == GameMode.Play)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isTouchingGround = false;
            coyoteTimeCounter = coyoteTime; // Resetear el tiempo de coyote
            playerAttack.isAttackLaunched = true;
            SoundsManager.Instance.jumpSound.Play();
            playerAnimator.SetBool("Jumping", true);
        }
    }

    public void ResetJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.7f);
        playerAttack.isAttackLaunched = true;
    }

    public void StartKnockUp(Vector2 direction)
    {
        StartCoroutine(KnockUp(direction));
    }
    public IEnumerator KnockUp(Vector2 direction)
    {
        isBeingKnockedBack = true;
        rb.velocity = direction * 10;
        yield return new WaitForSecondsRealtime(0.08f);
        isBeingKnockedBack = false;
    }

}
