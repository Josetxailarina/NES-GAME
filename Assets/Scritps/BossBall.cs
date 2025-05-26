using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class BossBall : EnemyScript
{
    public static int bolasRotas = 0;

    [SerializeField] private Sprite brokenBall;
    [SerializeField] private Sprite intactBall;
    [SerializeField] private GameObject[] explosionObjects;
    [SerializeField] private AudioSource music;
    [SerializeField] private GameObject credits;
    [SerializeField] private AudioSource creditsMusic;
    [SerializeField] private float shotCooldown = 3f;
    [SerializeField] private float currentCooldown = 0f;
    [SerializeField] private LanzadorFuego lanzadorScript;
    [SerializeField] private SpriteRenderer gameoverRender;
    [SerializeField] private Sprite endSprite;
    [SerializeField] private GameObject menu;
    [SerializeField] private AudioSource fireShotSound;

    private bool muerto;
    private bool onScreen;
    private Vector3 direccionFuego = Vector3.right;
    private Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    public override void Update()
    {
        if (Vector3.Distance(samurai.transform.position, transform.position) > 15)
        {
            onScreen = false;
        }
        else
        {
            onScreen = true;
        }
        if (onScreen&&!muerto)
        {
           
            currentCooldown += Time.deltaTime;

            if (currentCooldown >= shotCooldown)
            {
                currentCooldown = currentCooldown % shotCooldown;
                direccionFuego = samurai.transform.position - transform.position;
                lanzadorScript.LanzarFuego(transform.position, direccionFuego.normalized);
                fireShotSound.Play();
            }
        }
    }
    public override void ResetEnemy()
    {
        base.ResetEnemy();
        sprite.sprite = intactBall;
        bolasRotas = 0;
        collider.enabled = true;
        muerto = false;
    }
    public override void Die()
    {
        PlayEffect(explosionEffect, dieSound);
        sprite.sprite = brokenBall;
        bolasRotas++;
        collider.enabled = false;
        muerto = true;
        if (bolasRotas==3)
        {
            StartCoroutine(DestruccionBoss());
        }
        
    }
    IEnumerator DestruccionBoss()
    {
        Time.timeScale = 0;
        GameManagerScript.modoJuego = GameMode.GameOver;
        music.Stop();
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
        credits.SetActive(true);
        creditsMusic.Play();
        gameoverRender.sprite = endSprite;
        menu.SetActive(false);
    }
}
