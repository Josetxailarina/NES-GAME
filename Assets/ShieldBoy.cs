using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBoy : EnemyScript
{
    public AudioSource reboteSound;
    private bool tumbado;
    public float intervaloDisparos = 5f; // Cambia esto al número de segundos que quieras
    private float tiempoPasado = 0f;
    private Animator anim;
    public AudioSource chargingSound;
    public AudioSource fireSound;
    private LanzadorFuego fuegoScript;
    private bool onScreen;
    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        fuegoScript = FindObjectOfType<LanzadorFuego>();
    }
    public override void Update()
    {
        if (Vector3.Distance(samurai.transform.position, transform.position) > 10)
        {
            onScreen = false;
        }
        else
        {
            onScreen=true;
        }
        if (onScreen&&!tumbado)
        {
            base.Update();
            tiempoPasado += Time.deltaTime;

            if (tiempoPasado >= intervaloDisparos)
            {
                tiempoPasado = tiempoPasado % intervaloDisparos;
                PrepararDisparo();

            }
        }
    }
    public void PrepararDisparo()
    {
        anim.SetBool("Charging", true);
        chargingSound.Play();
    }
    public void GoRomperGuardia()
    {
        StartCoroutine(RomperGuardia());
        hitSound.Play();
        TakeDamage(Vector2.left);

    }
    IEnumerator RomperGuardia()
    {
        tumbado = true;
        anim.SetBool("Tumbado", true);
        anim.SetBool("Charging", false);
        yield return new WaitForSecondsRealtime(2);
        tumbado = false;
        anim.SetBool("Tumbado", false);

    }
    
    public void Disparar()
    {
        fireSound.Play();
        anim.SetBool("Charging", false);
        fuegoScript.LanzarFuego(transform.position-new Vector3(0.1f,0.3f,0), isFacingRight);
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        tumbado = false;
        anim.SetBool("Charging",false);
        anim.SetBool("Tumbado", false);
        onScreen = false;

    }
    public override void TakeDamage(Vector2 direccionHit)
    {
        if (tumbado)
        {
            base.TakeDamage(direccionHit);

        }
        else
        {
            reboteSound.Play();
        }

    }

}
