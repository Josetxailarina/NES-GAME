using System.Collections;
using UnityEngine;

public class Ninja : EnemyScript
{
    [SerializeField] private float shotCooldown = 1f;
    [SerializeField] private ProjectileLauncher lanzadorSurikens;

    private Rigidbody2D rb;
    private float currentShotCooldown = 0;
    [HideInInspector] public Vector3 startPosition;
    private EnemyMovement enemyMovement;

    public override void Awake()
    {
        base.Awake();
        enemyMovement = GetComponent<EnemyMovement>();
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }
    public override void Update()
    {
        base.Update();
        if (!isVisible) return;
        HandleShotCooldown();
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        rb.velocity = Vector3.zero;
        transform.position = startPosition;
        currentShotCooldown = 0;
    }
    private void ThrowSuriken()
    {
        Vector2 launchDirection = isFacingRight ? Vector2.right : Vector2.left;
        lanzadorSurikens.ThrowProjectile(transform.position, launchDirection);
    }


    private void HandleShotCooldown()
    {
        currentShotCooldown += Time.deltaTime;
        if (currentShotCooldown >= shotCooldown)
        {
            ThrowSuriken();
            currentShotCooldown = 0;
        }
    }

  
    public override IEnumerator DamageFeedback(Vector2 direccionHit)
    {
        StartCoroutine(base.DamageFeedback(direccionHit));
        if (enemyMovement != null)
        {
            enemyMovement.StartCoroutine(enemyMovement.ApplyKnockbackEffect(direccionHit));
        }
        yield break;
    }
}
