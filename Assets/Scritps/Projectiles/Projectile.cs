using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IDeflectable
{
    [SerializeField] private float resetDistance = 15;
    [SerializeField] protected float throwForce = 15;

    [HideInInspector] public bool isLaunched;

    protected bool isDeflected;
    protected Rigidbody2D rb;
    private GameObject samurai;
    private Vector3 initialPosition;

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
            ResetProjectile();
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDeflected && collision.gameObject.CompareTag("NinjaM"))
        {
            collision.GetComponent<EnemyScript>().TakeDamage(rb.velocity.normalized);
            ResetProjectile();
        }
    }
    public virtual void ThrowProjectile(Vector3 shurikenPosition, Vector2 throwDirection)
    {
        int layerSlash = LayerMask.NameToLayer("Slash");
        rb.excludeLayers |= (1 << layerSlash);
        isLaunched = true;
        transform.position = shurikenPosition;
        rb.velocity = throwDirection * throwForce;
        StartCoroutine(TemporarilyIgnoreSlashLayer());
    }
    public virtual void ResetProjectile()
    {
        transform.position = initialPosition;
        rb.velocity = Vector2.zero;
        isLaunched = false;
        isDeflected = false;
        int layerDamage = LayerMask.NameToLayer("DamageCollider");
        rb.excludeLayers &= ~(1 << layerDamage);
    }
    public virtual void Deflect(Vector2 deflectDirection)
    {
        isDeflected = true;
        rb.velocity = deflectDirection * 10;
        int layerDamage = LayerMask.NameToLayer("DamageCollider");
        rb.excludeLayers |= (1 << layerDamage);
    }
    IEnumerator TemporarilyIgnoreSlashLayer()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        int layerSamurai = LayerMask.NameToLayer("Slash");
        rb.excludeLayers &= ~(1 << layerSamurai);
    }
}
