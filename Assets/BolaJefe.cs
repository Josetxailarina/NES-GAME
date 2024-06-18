using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class BolaJefe : EnemyScript
{
    public Sprite bolaRota;
    public Sprite bolaBien;
    public static int bolasRotas=0;
    public GameObject[] explosiones;
    public AudioSource music;
    public GameObject credits;
    public AudioSource creditsMusic;
    private bool onScreen;
    public float intervaloDisparos = 5f;
    public float tiempoPasado = 0f;
    public LanzadorFuego lanzadorScript;
    private Vector3 direccionFuego= Vector3.right;
    public SpriteRenderer gameoverRender;
    public Sprite endSprite;
    public GameObject menu;
    private Collider2D collider;
    public AudioSource sonidoLanzarFuego;
    private bool muerto;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
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
           
            tiempoPasado += Time.deltaTime;

            if (tiempoPasado >= intervaloDisparos)
            {
                tiempoPasado = tiempoPasado % intervaloDisparos;
                direccionFuego = samurai.transform.position - transform.position;
                lanzadorScript.LanzarFuego(transform.position, direccionFuego.normalized);
                sonidoLanzarFuego.Play();
            }
        }
    }
    public override void ResetEnemy()
    {
        base.ResetEnemy();
        sprite.sprite = bolaBien;
        bolasRotas = 0;
        collider.enabled = true;
        muerto = false;
    }
    public override void EnemyDie()
    {
        dieEffect.transform.position = transform.position;
        dieEffect.transform.rotation = transform.rotation;
        dieEffect.SetTrigger("Hit");
        dieSound.Play();
        sprite.sprite = bolaRota;
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
        explosiones[0].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosiones[1].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosiones[2].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosiones[3].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosiones[4].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosiones[5].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosiones[6].SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        explosiones[7].SetActive(true);
        credits.SetActive(true);
        creditsMusic.Play();
        gameoverRender.sprite = endSprite;
        menu.SetActive(false);
    }
}
