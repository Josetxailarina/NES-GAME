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
        if (!collision.CompareTag("NinjaM"))
        {
            hitSound.Play();
        }

        if (playerController.direccionAtaque == Vector2.down&& !collision.CompareTag("ShieldBoy"))
        {
            playerController.PerformJumpAttack();
            animHit.transform.position = collision.transform.position;
            animHit.transform.rotation = transform.rotation;
            animHit.SetTrigger("Hit");

        }
        if (collision.CompareTag("Suriken"))
        {
            animHit.transform.position = collision.transform.position;
            animHit.transform.rotation = transform.rotation;
            animHit.SetTrigger("Hit");

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
                if (playerController.direccionAtaque != Vector2.down)
                {
                    playerController.StartKnockUp(Vector2.right);

                }
                print("llamo a corrutina knokup rigt");

            }
            else
            {
                collision.GetComponent<EnemyScript>().TakeDamage(Vector2.right);
                if (playerController.direccionAtaque != Vector2.down)
                {
                    playerController.StartKnockUp(Vector2.left);

                }
                print("llamo a corrutina knokup left");


            }

        }
        else if (collision.CompareTag("ShieldBoy"))
        {
            if (playerController.transform.position.x > collision.transform.position.x)
            {
                //collision.GetComponent<EnemyScript>().TakeDamage(Vector2.left);
                if (playerController.direccionAtaque != Vector2.down)
                {
                    playerController.StartKnockUp(Vector2.right);

                }

            }
            else
            {
                //collision.GetComponent<EnemyScript>().TakeDamage(Vector2.right);
                if (playerController.direccionAtaque != Vector2.down)
                {
                    playerController.StartKnockUp(Vector2.left);

                }


            }

        }

    }
}
