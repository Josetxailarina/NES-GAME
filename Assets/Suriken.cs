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
    private void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
        samurai = GameObject.FindGameObjectWithTag("Samurai");
    }
   
    public void ResetSuriken()
    {
        transform.position = posicionInicial;
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        lanzado = false;

    }

    public void LanzarSuriken(Vector3 posicionSuriken,bool derecha)
    {
        lanzado = true;
        transform.position = posicionSuriken;
        rb.simulated = true;
        rb.velocity = Vector2.zero;
        if (derecha)
        {
            rb.AddForce(new Vector2(fuerzaSuriken, fuerzaSuriken*2),ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(fuerzaSuriken*-1, fuerzaSuriken*2), ForceMode2D.Impulse);

        }
    }
    private void OnBecameInvisible()
    {
        ResetSuriken();
    }
}
