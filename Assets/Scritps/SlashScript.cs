using UnityEngine;

public class SlashScript : MonoBehaviour
{
    [SerializeField] private Animator hitEffectAnimator;
    [SerializeField] private PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundsManager.Instance.hitSound.Play();
        IDamagable damagable = collision.GetComponent<IDamagable>();

        
        if (collision.CompareTag("Suriken") || collision.CompareTag("FireBall"))
        {
            PlayHitEffect(collision);
            Vector2 deflectDirection;
            deflectDirection = GetDeflectDirection();
            collision.GetComponent<IDeflectable>().Deflect(deflectDirection);
        }

        else if ( damagable != null)
        {
            if (playerController.transform.position.x > collision.transform.position.x)
            {
                damagable.TakeDamage(Vector2.left);
                if (playerController.attackDirection != Vector2.down && playerController.attackDirection != Vector2.up)
                {
                    playerController.StartKnockUp(Vector2.right);
                }
            }
            else
            {
                damagable.TakeDamage(Vector2.right);
                if (playerController.attackDirection != Vector2.down && playerController.attackDirection != Vector2.up)
                {
                    playerController.StartKnockUp(Vector2.left);
                }
            }
        }

        if (playerController.attackDirection == Vector2.down)
        {
            playerController.PerformJumpAttack();
            PlayHitEffect(collision);
        }
    }

    private Vector2 GetDeflectDirection()
    {
        Vector2 deflectDirection;
        if (playerController.attackDirection == Vector2.down || playerController.attackDirection == Vector2.up)
        {
            deflectDirection = playerController.attackDirection;
        }
        else
        {
            deflectDirection = playerController.mirandoIzqui ? Vector2.left : Vector2.right;
        }

        return deflectDirection;
    }

    private void PlayHitEffect(Collider2D collision)
    {
        hitEffectAnimator.transform.position = collision.transform.position;
        hitEffectAnimator.transform.rotation = transform.rotation;
        hitEffectAnimator.SetTrigger("Hit");
    }
}
