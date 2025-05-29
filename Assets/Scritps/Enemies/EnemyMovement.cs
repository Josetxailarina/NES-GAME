using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float jumpCooldown = 1.2f;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float knockbackForce = 3f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float jumpForce = 4f;
    private float direction = 1.0f;
    private float currentJumpCooldown = 0;
    private Vector3 pathStartPosition;
    private Vector3 startPosition;
    private bool isMoving = true;
    private Rigidbody2D rb;
    private EnemyScript enemyScript;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pathStartPosition = transform.position;
        startPosition = transform.position;
        enemyScript = GetComponent<EnemyScript>();
    }
    private void OnDisable()
    {
        isMoving = true;
        currentJumpCooldown = 0;
        pathStartPosition = startPosition;
    }
    private void FixedUpdate()
    {
        if (!enemyScript.isVisible) return;
        HandleMovement();
        HandleJumpCooldown();
    }


    private void HandleMovement()
    {
        if (isMoving)
        {
            rb.velocity = new Vector3(speed * direction, rb.velocity.y);
        }

        if (transform.position.x - pathStartPosition.x >= moveDistance)
        {
            direction *= -1;
            pathStartPosition = transform.position;
        }
    }
    private void HandleJumpCooldown()
    {
        currentJumpCooldown += Time.deltaTime;
        if (currentJumpCooldown >= jumpCooldown)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode2D.Impulse);
            currentJumpCooldown = 0;
        }
    }
    public IEnumerator ApplyKnockbackEffect(Vector2 direccionHit)
    {
        isMoving = false;
        rb.velocity = direccionHit * knockbackForce;

        yield return new WaitForSeconds(0.7f);

        isMoving = true;
    }
}

