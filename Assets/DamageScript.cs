using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public PlayerController playerController;
    public Animator hitAnimator;
    private int multiplicadorKnockUp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerController.parpadeando)
        {
            hitAnimator.transform.position = playerController.transform.position;
            hitAnimator.SetTrigger("Hit");
            if (collision.CompareTag("ShieldBoy"))
            {
                multiplicadorKnockUp = 2;
            }
            else
            {
                multiplicadorKnockUp = 1;
            }
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                
                StartCoroutine(playerController.TakeDamage(new Vector2(-1*multiplicadorKnockUp,0)));
            }
            else
            {
                StartCoroutine(playerController.TakeDamage(new Vector2(1 * multiplicadorKnockUp, 0)));

            }

        }
    }
}
