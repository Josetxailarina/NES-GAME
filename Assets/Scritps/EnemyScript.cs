using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private int currentHealth = 3;
    [SerializeField] private Animator damageEffectAnim;
    [SerializeField] private Color damageTintColor = Color.white;
    [SerializeField] protected Animator explosionEffect;
    [SerializeField] protected AudioSource dieSound;
    [SerializeField] protected AudioSource hitSound;

    protected GameObject samurai;
    protected bool isFacingRight;
    protected bool isMoving = true;
    protected SpriteRenderer sprite;
    protected int initialHealth;

    public virtual void Awake()
    {
        initialHealth = currentHealth;
        sprite = GetComponent<SpriteRenderer>();
        samurai = GameObject.FindGameObjectWithTag("Samurai");
    }

    public virtual void Update()
    {
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
            PlayEffect(damageEffectAnim, hitSound);
            StartCoroutine(DamageFeedback(hitDirection));
        }
    }

    public virtual void ResetEnemy()
    {
        currentHealth = initialHealth;
        sprite.material.SetColor("_Tint", new Color(damageTintColor.r, damageTintColor.g, damageTintColor.b, 0));
    }


    public virtual void Die()
    {
        PlayEffect(explosionEffect,dieSound);
        gameObject.SetActive(false);
    }
    private void CheckIfFalling()
    {
        if (transform.position.y < -10)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateFacingDirection()
    {
        if (isMoving)
        {
            bool shouldFaceRight = samurai.transform.position.x > transform.position.x;

            if (shouldFaceRight != isFacingRight)
            {
                isFacingRight = shouldFaceRight;
                float yRotation = isFacingRight ? 180 : 0;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);
            }
        }
    }
    
    protected void PlayEffect(Animator effectAnimator, AudioSource audio)
    {
        effectAnimator.transform.position = transform.position;
        effectAnimator.transform.rotation = transform.rotation;
        effectAnimator.SetTrigger("Hit");
        audio.Play();
    }

    public virtual IEnumerator DamageFeedback(Vector2 direccionHit)
    {
        sprite.material.SetColor("_Tint", damageTintColor);
        yield return new WaitForSeconds(0.1f);
        sprite.material.SetColor("_Tint", new Color(damageTintColor.r, damageTintColor.g, damageTintColor.b, 0));
    }

}
