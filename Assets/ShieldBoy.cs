using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBoy : EnemyScript
{
    public AudioSource reboteSound;
    private bool tumbado;
    public override void Awake()
    {
        base.Awake();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
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
