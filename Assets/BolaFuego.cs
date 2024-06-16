using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaFuego : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject samurai;
    public bool lanzado;
    public float distanciaReset;
    private Vector3 posicionInicial;
    private bool reflejado;
    public float fireVelocity;
    private void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
        samurai = GameObject.FindGameObjectWithTag("Samurai");
    }
    private void Update()
    {
        if (Vector3.Distance(samurai.transform.position, transform.position) > distanciaReset)
        {
            ResetFireBall();

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("NinjaM")) && reflejado)
        {
            collision.GetComponent<EnemyScript>().TakeDamage(rb.velocity.normalized);
            ResetFireBall();

        }
        else if (collision.gameObject.CompareTag("ShieldBoy") && reflejado)
        {
            collision.GetComponent<ShieldBoy>().GoRomperGuardia();
            ResetFireBall();

        }

    }

    public void ResetFireBall()
    {
        transform.position = posicionInicial;
        rb.velocity = Vector2.zero;
        lanzado = false;
        reflejado = false;
        int layerDamage = LayerMask.NameToLayer("DamageCollider");
        rb.excludeLayers &= ~(1 << layerDamage);

    }
    public void Reflejar(Vector2 direccionReflejo)
    {
        reflejado = true;
        rb.velocity = direccionReflejo * 10;
        int layerDamage = LayerMask.NameToLayer("DamageCollider");
        rb.excludeLayers |= (1 << layerDamage);
    }

    public void LanzarFuego(Vector3 posicionSuriken, bool derecha)
    {
        int layerSlash = LayerMask.NameToLayer("Slash");
        rb.excludeLayers |= (1 << layerSlash);
        lanzado = true;
        transform.position = posicionSuriken;

        if (derecha)
        {
            rb.velocity = new Vector2(1,0) * fireVelocity;
        }
        else
        {
            rb.velocity = new Vector2(-1, 0) * fireVelocity;

        }
        StartCoroutine(TiempoInrreflejable());
    }
    IEnumerator TiempoInrreflejable()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        int layerSamurai = LayerMask.NameToLayer("Slash");
        rb.excludeLayers &= ~(1 << layerSamurai);
    }

}
