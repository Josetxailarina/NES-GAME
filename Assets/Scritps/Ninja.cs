using System.Collections;
using UnityEngine;

public class Ninja : EnemyScript
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float jumpForce = 2.0f;
    [SerializeField] private float throwInterval = 2.0f;
    [SerializeField] private float jumpInterval = 2.0f; 
    [SerializeField] private float moveDistance = 5.0f;
    [SerializeField] private float knockbackForce = 3.0f;

    private Rigidbody2D rb;
    private float direction = 1.0f;
    private float jumpTimer;
    private float throwTimer;
    private LanzadorSurikens lanzadorSurikens;
    private Vector3 startPosition;
    private Vector3 initialPosition;

    public override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
        lanzadorSurikens = FindObjectOfType<LanzadorSurikens>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }
    void OnEnable()
    {
        jumpTimer = Time.time;
        throwTimer = Time.time;
    }

    public override void Update()
    {
        base.Update();

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
        if (isMoving)
        {
            rb.velocity = new Vector3(speed * direction, rb.velocity.y);

        }


    }
    public override void ResetEnemy()
    {
        base.ResetEnemy();
        rb.velocity = Vector3.zero;
        transform.position = initialPosition;
        isMoving = true;

    }
    private void ThrowSuriken()
    {
        lanzadorSurikens.LanzarSuriken(transform.position, isFacingRight);

    }
    public override IEnumerator DamageFeedback(Vector2 direccionHit)
    {
        StartCoroutine(base.DamageFeedback(direccionHit));
        isMoving = false;
        rb.velocity = direccionHit * knockbackForce;
        yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(0.6f);
        isMoving = true;

    }
}
