using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamagable
{
    [SerializeField] private int currentHealth = 3;
    [SerializeField] private Color damageTintColor = Color.white;
    [SerializeField] private PlayerAttack playerAttack;

    protected GameObject samurai;
    protected bool isFacingRight;
    protected SpriteRenderer enemySpriteRenderer;
    protected int maxHealth;
    [HideInInspector] public bool isVisible = true;

    public virtual void Awake()
    {
        maxHealth = currentHealth;
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        samurai = GameObject.FindGameObjectWithTag("Samurai");
    }

    public virtual void Update()
    {
        CheckVisibility();
        CheckIfFalling();
        UpdateFacingDirection();
    }

    public virtual void TakeDamage(Vector2 hitDirection)
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            EffectsManager.Instance.PlayHitEffect(transform.position, playerAttack.attackDirection);
            StartCoroutine(DamageFeedback(hitDirection));
        }
    }

    public virtual void ResetEnemy()
    {
        currentHealth = maxHealth;
        enemySpriteRenderer.material.SetColor("_Tint", new Color(damageTintColor.r, damageTintColor.g, damageTintColor.b, 0));
    }


    public virtual void Die()
    {
        EffectsManager.Instance.PlayExplosion(transform.position);
        gameObject.SetActive(false);
    }
    private void CheckIfFalling()
    {
        if (transform.position.y < -10)
        {
            gameObject.SetActive(false);
        }
    }
    protected void CheckVisibility()
    {
        if (Vector3.Distance(samurai.transform.position, transform.position) > 10)
        {
            isVisible = false;
        }
        else
        {
            isVisible = true;
        }
    }

    protected virtual void UpdateFacingDirection()
    {

        bool shouldFaceRight = samurai.transform.position.x > transform.position.x;

        if (shouldFaceRight != isFacingRight)
        {
            isFacingRight = shouldFaceRight;
            float yRotation = isFacingRight ? 180 : 0;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);
        }

    }
    public virtual IEnumerator DamageFeedback(Vector2 direccionHit)
    {
        enemySpriteRenderer.material.SetColor("_Tint", damageTintColor);
        yield return new WaitForSeconds(0.1f);
        enemySpriteRenderer.material.SetColor("_Tint", new Color(damageTintColor.r, damageTintColor.g, damageTintColor.b, 0));
    }
}
