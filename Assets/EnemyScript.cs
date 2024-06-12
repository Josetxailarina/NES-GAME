using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 2.0f;
    public float jumpForce = 2.0f;
    public float throwInterval = 2.0f;
    public int vidaTotal = 3;
    public float jumpInterval = 2.0f; // Tiempo entre saltos
    public float moveDistance = 5.0f; // Distancia que recorre antes de cambiar de dirección
    public Animator damageEffectAnim;
    private Rigidbody2D rb;
    private float direction = 1.0f;
    private float jumpTimer;
    private float throwTimer;
    private GameObject samurai;
    private bool isFacingRight;
    private LanzadorSurikens lanzadorSurikens;
    private Vector3 startPosition; // Posición inicial
    private bool moviendo=true;
    public float fuerzaEmpuje;
    public AudioSource hitSound;
    void Start()
    {
        samurai = GameObject.FindGameObjectWithTag("Samurai");
        lanzadorSurikens = FindObjectOfType<LanzadorSurikens>();
        rb = GetComponent<Rigidbody2D>();
        jumpTimer = Time.time;
        throwTimer = Time.time;
        startPosition = transform.position; // Guardar la posición inicial
    }

    void Update()
    {
        if (moviendo)
        {
            //mirar al player
            bool shouldFaceRight = samurai.transform.position.x > transform.position.x;

            if (shouldFaceRight != isFacingRight)
            {
                isFacingRight = shouldFaceRight;
                float yRotation = isFacingRight ? 180 : 0;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);
            }

            // Mover el enemigo hacia adelante y hacia atrás

            rb.velocity = new Vector3(speed * direction, rb.velocity.y);


            // Hacer que el enemigo salte cada X segundos
            if (Time.time - jumpTimer >= jumpInterval)
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode2D.Impulse);
                jumpTimer = Time.time;
            }

            // Cambiar la dirección cada X metros
            if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
            {
                direction *= -1; // Cambiar la dirección
                startPosition = transform.position; // Actualizar la posición inicial
            }
        }
            // Hacer que el enemigo lance un suriken cada X segundos
            if (Time.time - throwTimer >= throwInterval)
            {
                ThrowSuriken();
                throwTimer = Time.time;
            }
        
    }

    void ThrowSuriken()
    {
        lanzadorSurikens.LanzarSuriken(transform.position, isFacingRight);

    }

    public void TakeDamage(Vector2 direccionHit)
    {
        print("Damage");
        vidaTotal -= 1;
        damageEffectAnim.transform.position = transform.position;
        damageEffectAnim.SetTrigger("Hit");
        hitSound.Play();

        StopAllCoroutines();
        StartCoroutine(Damage(direccionHit));
        if (vidaTotal <= 0)
        {
            EnemyDie();
        }
    }
    IEnumerator Damage(Vector2 direccionHit)
    {
        moviendo = false;
        rb.velocity = direccionHit * fuerzaEmpuje;
        yield return new WaitForSeconds(0.6f);
        moviendo = true;

    }

    void EnemyDie()
    {
        // Implementa aquí la lógica para cuando el enemigo muere
    }

}
