using UnityEngine;

public class FireBall : Projectile
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (isDeflected && collision.gameObject.CompareTag("ShieldBoy"))
        {
            collision.GetComponent<ShieldBoy>().BreakGuard();
            ResetProjectile();
        }
    }
}
