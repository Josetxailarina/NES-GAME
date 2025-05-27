using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Animator hitEffectAnim;
    private int knockUpMultiplier;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerHealth.isFlashing)
        {
            hitEffectAnim.transform.position = playerHealth.transform.position;
            hitEffectAnim.SetTrigger("Hit");
            HandlePlayerDamage(collision);
        }
    }

    private void HandlePlayerDamage(Collider2D collision)
    {
        if (collision.CompareTag("ShieldBoy"))
        {
            knockUpMultiplier = 2;
        }
        else
        {
            knockUpMultiplier = 1;
        }
        if (collision.gameObject.transform.position.x > transform.position.x)
        {
            playerHealth.TakeDamage(new Vector2(-1 * knockUpMultiplier, 0));
        }
        else
        {
            playerHealth.TakeDamage(new Vector2(1 * knockUpMultiplier, 0));
        }
    }
}
