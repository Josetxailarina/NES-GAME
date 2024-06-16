

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region OTRAS
    [Header("STATS")]
    public int vidas = 3;
    public Sprite[] vidasImgs;
    public SpriteRenderer vidaSpriteRenderer;
    #endregion
    #region MOVIMIENTO
    [Header("MOVIMIENTO")]
    public Rigidbody2D rb;
    private PlayerInput playerInput;
    public float moveForce = 10f;
    private float moveInput;
    private float previousMoveInput = 0;
    public bool mirandoIzqui;
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
    public AudioSource jumpSound;
    public bool tocandoSuelo { get; set; }
    #endregion

    #region ATAQUE
    [Header("ATAQUE")]

    public Animator anim;
    public Vector2 direccionAtaque;
    public bool impulsoAttack;
    public AudioSource attackSound;
    public float attackCD = 0.5f;
    private float currentAttackCD;
    private bool upPressed;
    private bool downPressed;
    #endregion

    #region OTRAS
    [Header("OTRAS")]

    private Vector3 posicionLastCheckpoint;
    private Vector2 gravityVector;
    public AnimatorOverrideController[] animatorOverrideController;
    public AudioSource spawnSound;
    public bool damaged;
    public bool parpadeando;
    public SpriteRenderer sprite;
    public AudioSource damageSound;
    public AudioSource musicSound;
    public AudioSource deadMusicSound;
    public GameObject gameOverPanel;
    public GameOverMenu gameOverScript;
    #endregion


    private void Start()
    {
        tocandoSuelo = true;
        rb = GetComponent<Rigidbody2D>();
        gravityVector = new Vector2(0, -Physics2D.gravity.y);
        playerInput = GetComponent<PlayerInput>();
       anim = GetComponent<Animator>();
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
            if (moveInput != previousMoveInput&&!damaged&&GameManagerScript.modoJuego == GameMode.Play)
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
        anim.SetFloat("Yvelocity",rb.velocity.y);
        // Movimiento horizontal
        if (!damaged)
        {
            rb.velocity = new Vector2(moveInput * moveForce, rb.velocity.y);

        }

        // Aplicar gravedad adicional para una caída más rápida
        if (rb.velocity.y < 0 || (!jumpButtonHeld && !impulsoAttack))
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
    
    public void ResetSamurai()
    {
        transform.position = posicionLastCheckpoint;
        anim.SetTrigger("Reset");
        vidas = 3;
        vidaSpriteRenderer.sprite = vidasImgs[vidas];
        musicSound.Play();
        deadMusicSound.Stop();


    }
    public void Jump(InputAction.CallbackContext callBack)
    {
        if (callBack.performed&&!damaged)
        {
            if (GameManagerScript.modoJuego == GameMode.Play)
            {
                jumpButtonHeld = true; // Indicar que el botón de salto está presionado
                                       //if (doubleJumpReady)
                                       //{
                                       //    PerformJump();
                                       //}
                                       //else
                                       //{
                jumpBufferCounter = jumpBufferTime;
                //}
            }
            else if (GameManagerScript.modoJuego == GameMode.Menu)
            {
                gameOverScript.Aceptar();
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
        if (!damaged && GameManagerScript.modoJuego == GameMode.Play)
        {
            float actualJumpForce = jumpButtonHeld ? jumpForce : jumpForce * 0.7f;
            rb.velocity = new Vector2(rb.velocity.x, actualJumpForce);
            tocandoSuelo = false;
            coyoteTimeCounter = coyoteTime; // Resetear el tiempo de coyote
            impulsoAttack = false;
            jumpSound.Play();
            anim.SetBool("Jumping", true); 
        }
        
    }
    public void PerformJumpAttack()
    {

        if (!damaged && GameManagerScript.modoJuego == GameMode.Play)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            tocandoSuelo = false;
            coyoteTimeCounter = coyoteTime; // Resetear el tiempo de coyote
            impulsoAttack = true;
            jumpSound.Play();
            anim.SetBool("Jumping", true); 
        }

    }
  
    public void ResetJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.7f);
        impulsoAttack = true;
        
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
        if (callBack.performed && !damaged && GameManagerScript.modoJuego == GameMode.Play)
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
            if (GameManagerScript.modoJuego == GameMode.Menu)
            {
                gameOverScript.Subir();
            }
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
            if (GameManagerScript.modoJuego == GameMode.Menu)
            {
                gameOverScript.Bajar();
            }
        }
        if (callBack.canceled)
        {
            downPressed = false;

        }
    }
   
    public IEnumerator TakeDamage(Vector2 direccion)
    {
        if (vidas>0)
        {
            vidas--;
            vidaSpriteRenderer.sprite = vidasImgs[vidas];
        }
        else
        {
            damageSound.Play();

            anim.SetTrigger("Dead");
            Time.timeScale = 0;
            musicSound.Stop();
            deadMusicSound.Play();
            GameManagerScript.modoJuego = GameMode.GameOver;
            yield return new WaitForSecondsRealtime(2);
            gameOverPanel.SetActive(true);
            GameManagerScript.modoJuego = GameMode.Menu;

            yield break;
        }
        
        damageSound.Play(); 
        damaged = true;
        parpadeando = true;
        rb.velocity = direccion*10;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(0.2f);
        sprite.enabled = false;
        damaged = false;

        yield return new WaitForSecondsRealtime(0.1f);

        sprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        parpadeando = false;

    }

}
