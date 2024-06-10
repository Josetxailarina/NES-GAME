

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private PlayerInput playerInput;
    private Vector2 gravityVector;
    public Animator anim;
    public ParticleSystem slash;
    public AudioSource jumpSound;
    public GameObject cameraTarget;
    private AudioSource playerAudio;
    public AudioClip[] jumpScreams;
    public GameObject skull;
    public ParticleSystem jumpParticles;
    public ParticleSystem particulasMuerte;
    public GameObject panelStart;
    public GameObject panelFinal;
    public GameObject leaderboardObject;
    public AnimatorOverrideController[] animatorOverrideController;
    public Vector2 direccionAtaque;

    public float jumpForce = 250f;
    public float moveForce = 10f;
    public float multiplierFall = 4f;
    public float jumpBufferTime = 0.2f;
    public float maxFallSpeed = -20f;
    public float coyoteTime = 0.1f;

    private float moveInput;
    private float jumpBufferCounter;
    public float coyoteTimeCounter;
    private bool jumpButtonHeld;
    private float previousMoveInput = 0;
    public bool doubleJumpReady;
    public bool mirandoIzqui;
    public AudioSource spawnSound;
    public AudioSource attackSound;
    public float attackCD=0.5f;
    private float currentAttackCD;
    private bool upPressed;
    private bool downPressed;

    public bool tocandoSuelo { get; set; }

    private void Start()
    {
        tocandoSuelo = true;
        rb = GetComponent<Rigidbody2D>();
        gravityVector = new Vector2(0, -Physics2D.gravity.y);
        playerInput = GetComponent<PlayerInput>();
       anim = GetComponent<Animator>();
        //playerAudio = GetComponent<AudioSource>();
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
            if (moveInput != previousMoveInput)
            {
                anim.SetBool("Running", moveInput != 0);
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
            if (!tocandoSuelo)
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
        
            // Movimiento horizontal
            rb.velocity = new Vector2(moveInput * moveForce, rb.velocity.y);

            // Aplicar gravedad adicional para una caída más rápida
            if (rb.velocity.y < 0 || (!jumpButtonHeld && !doubleJumpReady))
            {
                rb.velocity -= gravityVector * multiplierFall * Time.deltaTime;
            }

            // Limitar velocidad de caída
            if (rb.velocity.y < maxFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
            }

            // Realizar el salto si hay un buffer de salto activo y está dentro del tiempo de coyote
            if (jumpBufferCounter > 0 && (tocandoSuelo || coyoteTimeCounter < coyoteTime))
            {
                PerformJump();
                jumpBufferCounter = 0; // Resetear el buffer de salto después de saltar
            }
        
    }
    private void PlayRandomJumpAudio()
    {
        playerAudio.pitch = Random.Range(0.9f, 1.1f);
        playerAudio.PlayOneShot(jumpScreams[Random.Range(0, jumpScreams.Length)]);
    }
    public void Jump(InputAction.CallbackContext callBack)
    {
        if (callBack.performed)
        {
            print("pulsado X");
            jumpButtonHeld = true; // Indicar que el botón de salto está presionado
            if (doubleJumpReady)
            {
                PerformJump();
            }
            else
            {
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
        float actualJumpForce = jumpButtonHeld ? jumpForce : jumpForce * 0.7f;
        rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
        tocandoSuelo = false;
        coyoteTimeCounter = coyoteTime; // Resetear el tiempo de coyote
        doubleJumpReady = false;
        jumpSound.Play();
        anim.SetBool("Jumping", true);
        
    }
    public void PerformJumpFull()
    {
        
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        tocandoSuelo = false;
        coyoteTimeCounter = coyoteTime; // Resetear el tiempo de coyote
        doubleJumpReady = false;
        jumpSound.Play();
        anim.SetBool("Jumping", true);

    }
    public void SpawnPlayer(Vector3 spawnPoint)
    {
        particulasMuerte.Play();
        transform.position = spawnPoint;
        particulasMuerte.Play();
        spawnSound.Play();
        rb.velocity = Vector3.zero;
    }
    public void ResetJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.7f);
        doubleJumpReady = true;
        
        //jumpParticles.Play();


    }

    public void StartButton(InputAction.CallbackContext callBack)
    {
        if (callBack.started)
        {
            panelStart.SetActive(true);
            leaderboardObject.transform.SetParent(panelStart.transform);
            Time.timeScale = 0;

        }
        else if (callBack.started  )
        {
            ReanudarGame();


        }

    }
    public void ReanudarGame()
    {
        
        panelStart.SetActive(false);
        panelFinal.SetActive(false);

        Time.timeScale = 1;
    }
    public void ReiniciarScene()
    {
        
        Time.timeScale = 1;

        SceneManager.LoadScene(1);
    }
    public void CerrarJuego()
    {
        Application.Quit();
    }

    public void Attack(InputAction.CallbackContext callBack)
    {
        if (callBack.performed)
        {
            if (currentAttackCD <= 0)
            {
                if (upPressed)
                {
                    anim.runtimeAnimatorController = animatorOverrideController[1];
                    direccionAtaque = Vector2.up;
                }
                else if (downPressed&&!tocandoSuelo)
                {
                    anim.runtimeAnimatorController = animatorOverrideController[2];
                    direccionAtaque = Vector2.down;


                }
                else
                {
                    anim.runtimeAnimatorController = animatorOverrideController[0];
                    if (mirandoIzqui)
                    {
                        direccionAtaque = Vector2.left;

                    }
                    else
                    {
                        direccionAtaque = Vector2.right;

                    }

                }
                anim.SetTrigger("Attack");
                currentAttackCD = attackCD;
                attackSound.Play();

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

}
