using UnityEngine;

public class Suriken : Projectile
{
    public override void ResetProjectile()
    {
        base.ResetProjectile();
        rb.simulated = false;
        rb.gravityScale = 1;
    }
    public override void Deflect(Vector2 deflectDirection)
    {
        base.Deflect(deflectDirection);
        rb.gravityScale = 0;
    }

    public override void ThrowProjectile(Vector3 surikenPosition, Vector2 throwDirection)
    {
        base.ThrowProjectile(surikenPosition, throwDirection);
        rb.simulated = true;
        if (throwDirection.x >= 0)
        {
            rb.AddForce(new Vector2(throwForce * Random.Range(0.8f, 1), throwForce * 3 * Random.Range(0.2f, 0.5f)), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2((throwForce * -1) * Random.Range(0.5f, 1), throwForce * 3 * Random.Range(0.5f, 1)), ForceMode2D.Impulse);
        }
    }
}
