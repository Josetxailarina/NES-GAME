using System.Collections;
using UnityEngine;

public class BossOrb : EnemyScript
{
    public static int brokenOrbCount = 0;

    [SerializeField] private Sprite brokenOrbSprite;
    [SerializeField] private Sprite intactOrbSprite;
    [SerializeField] private GameObject[] explosionObjects;
    [SerializeField] private GameOverMenu gameOverScript;
    [SerializeField] private float shotCooldown = 3f;
    [SerializeField] private float currentShotCooldown = 0f;
    [SerializeField] private ProjectileLauncher launcherScript;
    

    private bool isBroken;
    private Vector3 shotDirection = Vector3.right;
    private Collider2D bossCollider;
    private float initialCooldown;

    void Start()
    {
        initialCooldown = currentShotCooldown;
        bossCollider = GetComponent<Collider2D>();
    }
    public override void Update()
    {
        CheckVisibility();
        HandleOrbAttack();
    }
    public override void ResetEnemy()
    {
        base.ResetEnemy();
        enemySpriteRenderer.sprite = intactOrbSprite;
        brokenOrbCount = 0;
        bossCollider.enabled = true;
        isBroken = false;
        currentShotCooldown = initialCooldown;
    }
    public override void Die()
    {
        EffectsManager.Instance.PlayExplosion(transform.position);
        enemySpriteRenderer.sprite = brokenOrbSprite;
        brokenOrbCount++;
        bossCollider.enabled = false;
        isBroken = true;
        if (brokenOrbCount == 3)
        {
            StartCoroutine(ExecuteBossDestruction());
        }
    }

    private void HandleOrbAttack()
    {
        if (isVisible && !isBroken)
        {
            currentShotCooldown += Time.deltaTime;

            if (currentShotCooldown >= shotCooldown)
            {
                currentShotCooldown = currentShotCooldown % shotCooldown;
                shotDirection = samurai.transform.position - transform.position;
                launcherScript.ThrowProjectile(transform.position, shotDirection.normalized);
                SoundsManager.Instance.fireShotSound.Play();
            }
        }
    }

    IEnumerator ExecuteBossDestruction()
    {
        Time.timeScale = 0;
        GameManagerScript.gameMode = GameMode.GameOver;
        SoundsManager.Instance.backgroundMusic.Stop();
        explosionObjects[0].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosionObjects[1].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosionObjects[2].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosionObjects[3].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosionObjects[4].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosionObjects[5].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosionObjects[6].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosionObjects[7].SetActive(true);
        gameOverScript.gameObject.SetActive(true);
        gameOverScript.ShowEndMenu();
        SoundsManager.Instance.gameOverMusic.Play();
    }
}
