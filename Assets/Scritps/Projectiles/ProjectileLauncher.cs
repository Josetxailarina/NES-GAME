using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Projectile[] projectiles;

    public void ThrowProjectile(Vector3 position, Vector2 direction)
    {
        foreach (Projectile script in projectiles)
        {
            if (!script.isLaunched)
            {
                script.ThrowProjectile(position, direction);
                break;
            }
        }
    }
}
