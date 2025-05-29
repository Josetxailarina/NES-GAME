using System.Collections;
using UnityEngine;

public class ShieldBoy : EnemyScript
{
    [SerializeField] private float shotCooldown = 3f;
    [SerializeField] private ProjectileLauncher fireBallLauncher;

    private bool isGuardBroken;
    private float currentShotCooldown = 2f;
    private Vector2 fireDirection = Vector2.right;
    private Animator anim;


    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        HandleShotCooldown();
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        isGuardBroken = false;
        anim.SetBool("Charging", false);
        anim.SetBool("GuardBroken", false);
        isVisible = false;
    }
    protected override void UpdateFacingDirection()
    {
        if (!isGuardBroken)
        {
            base.UpdateFacingDirection();
        }
    }
    public override void TakeDamage(Vector2 direccionHit)
    {
        if (isGuardBroken)
        {
            base.TakeDamage(direccionHit);
        }
        else
        {
            SoundsManager.Instance.armorHitSound.Play();
        }
    }
    private void HandleShotCooldown()
    {
        if (isVisible && !isGuardBroken)
        {
            currentShotCooldown += Time.deltaTime;

            if (currentShotCooldown >= shotCooldown)
            {
                currentShotCooldown = currentShotCooldown % shotCooldown;
                StartCharging();
            }
        }
    }

    public void StartCharging()
    {
        anim.SetBool("Charging", true);
        SoundsManager.Instance.chargingFireSound.Play();
    }
    public void BreakGuard()
    {
        StartCoroutine(BreakGuardCoroutine());
        TakeDamage(Vector2.left);
    }
    IEnumerator BreakGuardCoroutine()
    {
        isGuardBroken = true;
        anim.SetBool("GuardBroken", true);
        anim.SetBool("Charging", false);
        yield return new WaitForSecondsRealtime(2);
        isGuardBroken = false;
        anim.SetBool("GuardBroken", false);

    }

    public void ShootFireball() // called from the animation event
    {
        SoundsManager.Instance.fireShotSound.Play();
        anim.SetBool("Charging", false);
        if (isFacingRight)
        {
            fireDirection = Vector2.right;
        }
        else
        {
            fireDirection = Vector2.left;
        }
        fireBallLauncher.ThrowProjectile(transform.position - new Vector3(0.1f, 0.3f, 0), fireDirection);
    }
}
