using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    public Animator animHit;
    public AudioSource hitSound;
    public PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (playerController.direccionAtaque == Vector2.down)
        {
            playerController.PerformJumpAttack();


        }
        if (collision.CompareTag("Suriken"))
        {
            hitSound.Play();

            if (playerController.direccionAtaque == Vector2.down)
            {
                collision.GetComponent<Suriken>().Reflejar(playerController.direccionAtaque);

            }
            else if (playerController.direccionAtaque == Vector2.up)
            {
                collision.GetComponent<Suriken>().Reflejar(playerController.direccionAtaque);

            }
            else 
            {
                if (playerController.mirandoIzqui)
                {
                    collision.GetComponent<Suriken>().Reflejar(Vector2.left);

                }
                else
                {
                    collision.GetComponent<Suriken>().Reflejar(Vector2.right);

                }

            }


        }
        else if (collision.CompareTag("NinjaM"))
        {
            if (playerController.transform.position.x > collision.transform.position.x)
            {
                collision.GetComponent<EnemyScript>().TakeDamage(Vector2.left);

            }
            else
            {
                collision.GetComponent<EnemyScript>().TakeDamage(Vector2.right);

            }
        }
    }
}
