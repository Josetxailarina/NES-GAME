using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour, IDeflectable
{
    [SerializeField] private float resetDistance = 15;
    [SerializeField] private float fireVelocity = 9;

    private Rigidbody2D rb;
    private GameObject samurai;
    private Vector3 initialPosition;
    private bool isDeflected;

    public bool isLaunched;


    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        samurai = GameObject.FindGameObjectWithTag("Samurai");
    }
    private void Update()
    {
        if (Vector3.Distance(samurai.transform.position, transform.position) > resetDistance)
        {
            ResetFireBall();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDeflected)
        {
            if (collision.gameObject.CompareTag("NinjaM"))
            {
                collision.GetComponent<EnemyScript>().TakeDamage(rb.velocity.normalized);
                ResetFireBall();
            }
            else if (collision.gameObject.CompareTag("ShieldBoy"))
            {
                collision.GetComponent<ShieldBoy>().GoRomperGuardia();
                ResetFireBall();
            } 
        }
    }

    public void ResetFireBall()
    {
        transform.position = initialPosition;
        rb.velocity = Vector2.zero;
        isLaunched = false;
        isDeflected = false;
        int layerDamage = LayerMask.NameToLayer("DamageCollider");
        rb.excludeLayers &= ~(1 << layerDamage);
    }
    public void Deflect(Vector2 direccionReflejo)
    {
        isDeflected = true;
        rb.velocity = direccionReflejo * 10;
        int layerDamage = LayerMask.NameToLayer("DamageCollider");
        rb.excludeLayers |= (1 << layerDamage);
    }

    public void LanzarFuego(Vector3 posicionSuriken, Vector2 Direccion)
    {
        int layerSlash = LayerMask.NameToLayer("Slash");
        rb.excludeLayers |= (1 << layerSlash);
        isLaunched = true;
        transform.position = posicionSuriken;
        rb.velocity = Direccion * fireVelocity;
        StartCoroutine(TiempoInrreflejable());
    }
    IEnumerator TiempoInrreflejable()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        int layerSamurai = LayerMask.NameToLayer("Slash");
        rb.excludeLayers &= ~(1 << layerSamurai);
    }

}
