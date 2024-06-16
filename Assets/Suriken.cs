using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Suriken : MonoBehaviour
{
    private Rigidbody2D rb;
    public float fuerzaSuriken;
    private GameObject samurai;
    public bool lanzado;
    public float distanciaReset;
    private Vector3 posicionInicial;
    private bool reflejado;
    private Vector3 direccionReflejo;
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
            ResetSuriken();

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NinjaM")&&reflejado)
        {
            collision.GetComponent<EnemyScript>().TakeDamage(rb.velocity.normalized);
        }
    }

    public void ResetSuriken()
    {
        transform.position = posicionInicial;
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        lanzado = false;
        rb.gravityScale = 1;
        reflejado = false;
        int layerSamurai = LayerMask.NameToLayer("DamageCollider");
        rb.excludeLayers &= ~(1 << layerSamurai);

    }
    public void Reflejar(Vector2 direccionReflejo)
    {
        reflejado = true;
        rb.gravityScale = 0;
        rb.velocity = direccionReflejo * 10;
        int layerSamurai = LayerMask.NameToLayer("DamageCollider");
        rb.excludeLayers |= (1 << layerSamurai);
    }

    public void LanzarSuriken(Vector3 posicionSuriken, bool derecha)
    {
        int layerSamurai = LayerMask.NameToLayer("Slash");
        rb.excludeLayers |= (1 << layerSamurai);
        lanzado = true;
        transform.position = posicionSuriken;
        rb.simulated = true;
        rb.velocity = Vector2.zero;
        if (derecha)
        {
            rb.AddForce(new Vector2(fuerzaSuriken * Random.Range(0.8f, 1), fuerzaSuriken * 2 * Random.Range(0.2f, 0.5f)), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2((fuerzaSuriken * -1) * Random.Range(0.5f, 1), fuerzaSuriken * 2 * Random.Range(0.5f, 1)), ForceMode2D.Impulse);

        }
        StartCoroutine(TiempoInreflejable());
    }
    IEnumerator TiempoInreflejable()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        int layerSamurai = LayerMask.NameToLayer("Slash");
        rb.excludeLayers &= ~(1 << layerSamurai);
    }

}
