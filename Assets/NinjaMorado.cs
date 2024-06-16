using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaMorado : EnemyScript
{
    public float speed = 2.0f;
    public float jumpForce = 2.0f;
    public float throwInterval = 2.0f;
    public float jumpInterval = 2.0f; // Tiempo entre saltos
    public float moveDistance = 5.0f; // Distancia que recorre antes de cambiar de dirección
    private Rigidbody2D rb;
    private float direction = 1.0f;
    private float jumpTimer;
    private float throwTimer;
    private LanzadorSurikens lanzadorSurikens;
    private Vector3 startPosition; // Posición inicial
    public float fuerzaEmpuje;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
        lanzadorSurikens = FindObjectOfType<LanzadorSurikens>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position; // Guardar la posición inicial

    }
    void OnEnable()
    {

        jumpTimer = Time.time;
        throwTimer = Time.time;
    }

    // Update is called once per frame
   public override void Update()
    {
        base.Update();
        
            //mirar al player
            
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

            // Hacer que el enemigo lance un suriken cada X segundos
            if (Time.time - throwTimer >= throwInterval)
            {
                ThrowSuriken();
                throwTimer = Time.time;
            }

        // Mover el enemigo hacia adelante y hacia atrás
        if (moviendo)
        {
            rb.velocity = new Vector3(speed * direction, rb.velocity.y);

        }


    }
    public override void ResetEnemy()
    {
        base.ResetEnemy();
        rb.velocity = Vector3.zero;
        transform.position = initialPosition;
        moviendo = true;

    }
    private void ThrowSuriken()
    {
        lanzadorSurikens.LanzarSuriken(transform.position, isFacingRight);

    }
    public override IEnumerator Damage(Vector2 direccionHit)
    {
        StartCoroutine(base.Damage(direccionHit));
        moviendo = false;
        rb.velocity = direccionHit * fuerzaEmpuje;
        yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(0.6f);
        moviendo = true;

    }
}
