using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    
    public int actualHealth = 3;
    
    public Animator damageEffectAnim;
    public Animator dieEffect;
    public AudioSource dieSound;
    
    protected GameObject samurai;
    protected bool isFacingRight;
    protected bool moviendo = true;

    public AudioSource hitSound;
    protected SpriteRenderer sprite;
    public Color colorDamage;
    protected int initialHealth;
    public virtual void Awake()
    {
        initialHealth = actualHealth;
        sprite = GetComponent<SpriteRenderer>();
        samurai = GameObject.FindGameObjectWithTag("Samurai");
        
    }
    

    public virtual void Update()
    {
        //DESACTIVAR SI SE CAE
        if (transform.position.y < -10)
        {
            gameObject.SetActive(false);
        }

        if (moviendo)
        {
            ////MIRAR AL PLAYER
            bool shouldFaceRight = samurai.transform.position.x > transform.position.x;

            if (shouldFaceRight != isFacingRight)
            {
                isFacingRight = shouldFaceRight;
                float yRotation = isFacingRight ? 180 : 0;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);
            }
        }
        


    }

    

    public virtual void TakeDamage(Vector2 direccionHit)
    {
        
        if (actualHealth < 0)
        {
            EnemyDie();
        }
        else
        {
            actualHealth -= 1;
            damageEffectAnim.transform.position = transform.position;
            damageEffectAnim.transform.rotation = transform.rotation;
            damageEffectAnim.SetTrigger("Hit");
            hitSound.Play();

            StopAllCoroutines();
            StartCoroutine(Damage(direccionHit));
        }

    }
    public virtual void ResetEnemy()
    {

        actualHealth = initialHealth;
        sprite.material.SetColor("_Tint", new Color(colorDamage.r, colorDamage.g, colorDamage.b, 0));

    }
    public virtual IEnumerator Damage(Vector2 direccionHit)
    {
        sprite.material.SetColor("_Tint", colorDamage);
        print("tintado");
        yield return new WaitForSeconds(0.1f);
        sprite.material.SetColor("_Tint", new Color(colorDamage.r, colorDamage.g, colorDamage.b, 0));
        print("destintado");

    }

    void EnemyDie()
    {
        dieEffect.transform.position = transform.position;
        dieEffect.transform.rotation = transform.rotation;
        dieEffect.SetTrigger("Hit");
        dieSound.Play();
        gameObject.SetActive(false);
    }

}
