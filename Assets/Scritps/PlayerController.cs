

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   

    #region MOVIMIENTO
    [Header("MOVIMIENTO")]
    public Rigidbody2D rb;
    private PlayerInput playerInput;
    public float moveForce = 10f;
    private float moveInput;
    private float previousMoveInput = 0;
    public bool mirandoIzqui;
    [SerializeField] private PlayerHealth playerHealthScript;
    #endregion

    #region SALTO
    [Header("SALTO")]

    public float jumpForce = 250f;
    public float multiplierFall = 4f;
    public float jumpBufferTime = 0.2f;
    public float maxFallSpeed = -20f;
    public float coyoteTime = 0.1f;
    private float jumpBufferCounter;
    public float coyoteTimeCounter;
    private bool jumpButtonHeld;
    public bool isTouchingGround { get; set; }
    #endregion

    #region ATAQUE
    [Header("ATAQUE")]

    public Animator playerAnimator;
    public Vector2 direccionAtaque;
    public bool isAttackLaunched;
    public float attackCD = 0.5f;
    private float currentAttackCD;
    private bool upPressed;
    private bool downPressed;
    #endregion

    #region OTRAS
    [Header("OTRAS")]

    public Vector3 posicionLastCheckpoint;
    private Vector2 gravityVector;
    public AnimatorOverrideController[] animatorOverrideController;
    public bool isBeingKnockedBack;
    [HideInInspector] public PlayerHealth playerHealth;

    #endregion


    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        isTouchingGround = true;
        rb = GetComponent<Rigidbody2D>();
        gravityVector = new Vector2(0, -Physics2D.gravity.y);
        playerInput = GetComponent<PlayerInput>();
       playerAnimator = GetComponent<Animator>();
        posicionLastCheckpoint = transform.position;
    }

    private void Update()
    {
        
            float input = playerInput.actions["MOVE"].ReadValue<float>();
            if (input > 0)
            {
                moveInput = 1;
            }
            else if (input < 0)
            {
                moveInput = -1;
            }
            else
            {
                moveInput = 0;
            }
        if (currentAttackCD > 0)
        {
            currentAttackCD -= Time.deltaTime;
        }
            // Actualiza el animator solo si el estado de movimiento ha cambiado
            if (moveInput != previousMoveInput&&!isBeingKnockedBack && GameManagerScript.modoJuego == GameMode.Play)
            {
                playerAnimator.SetBool("Running", moveInput != 0);
                previousMoveInput = moveInput; // Actualiza el estado previo
                if (moveInput != 0)
                {
                    Vector3 newScale = transform.localScale;
                    newScale.x = moveInput; // Cambia la escala en el eje x para que el jugador mire en la dirección del movimiento
                    transform.localScale = newScale;
                    if (moveInput < 0)
                    {
                        mirandoIzqui = true;
                    }
                    else if (moveInput > 0)
                    {
                        mirandoIzqui = false;
                    }
                }
            }
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
        playerAnimator.SetFloat("Yvelocity",rb.velocity.y);

        if (!isBeingKnockedBack)
        {
            rb.velocity = new Vector2(moveInput * moveForce, rb.velocity.y);

        }

        // Aplicar gravedad adicional para una caída más rápida
        if (rb.velocity.y < 0 || (!jumpButtonHeld && !isAttackLaunched))
            {
                rb.velocity -= gravityVector * multiplierFall * Time.deltaTime;
            }

            // Limitar velocidad de caída
            if (rb.velocity.y < maxFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
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
        playerHealthScript.ResetLives();
    }
    public void Jump(InputAction.CallbackContext callBack)
    {
        if (callBack.performed&&!isBeingKnockedBack)
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
            isAttackLaunched = false;
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
            isAttackLaunched = true;
            SoundsManager.Instance.jumpSound.Play();
            playerAnimator.SetBool("Jumping", true); 
        }

    }
  
    public void ResetJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.7f);
        isAttackLaunched = true;
        
        //jumpParticles.Play();


    }

    public void StartButton(InputAction.CallbackContext callBack)
    {
        if (callBack.started && GameManagerScript.modoJuego == GameMode.Play)
        {
            

        }
        else if (callBack.started && GameManagerScript.modoJuego == GameMode.Play  )
        {


        }

    }
 
    public void CerrarJuego()
    {
        Application.Quit();
    }

    public void Attack(InputAction.CallbackContext callBack)
    {
        if (callBack.performed && !isBeingKnockedBack && GameManagerScript.modoJuego == GameMode.Play)
        {
            
            if (currentAttackCD <= 0)
            {
                if (upPressed)
                {
                    playerAnimator.runtimeAnimatorController = animatorOverrideController[1];
                    direccionAtaque = Vector2.up;
                }
                else if (downPressed&&!isTouchingGround)
                {
                    playerAnimator.runtimeAnimatorController = animatorOverrideController[2];
                    direccionAtaque = Vector2.down;


                }
                else
                {
                    playerAnimator.runtimeAnimatorController = animatorOverrideController[0];
                    if (mirandoIzqui)
                    {
                        direccionAtaque = Vector2.left;

                    }
                    else
                    {
                        direccionAtaque = Vector2.right;

                    }

                }
                playerAnimator.SetTrigger("Attack");
                currentAttackCD = attackCD;
                SoundsManager.Instance.playerAttackSound.Play();
            }

        }

        if (callBack.canceled   )
        {
         
        }
    }
    public void Up(InputAction.CallbackContext callBack)
    {
        if (callBack.started)
        {
            upPressed = true;
        }
        if (callBack.canceled)
        {
            upPressed = false;
        }
    }
    public void Down(InputAction.CallbackContext callBack)
    {
        if (callBack.started)
        {
            downPressed = true;
        }
        if (callBack.canceled)
        {
            downPressed = false;
        }
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
