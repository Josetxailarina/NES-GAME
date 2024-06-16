using System;
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
        if (!collision.CompareTag("NinjaM")&& !collision.CompareTag("ShieldBoy"))
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
        if (collision.CompareTag("Suriken") || collision.CompareTag("BolaFuego"))
        {
            animHit.transform.position = collision.transform.position;
            animHit.transform.rotation = transform.rotation;
            animHit.SetTrigger("Hit");

            Vector2 direccionReflejo;

            if (playerController.direccionAtaque == Vector2.down || playerController.direccionAtaque == Vector2.up)
            {
                direccionReflejo = playerController.direccionAtaque;
            }
            else
            {
                direccionReflejo = playerController.mirandoIzqui ? Vector2.left : Vector2.right;
            }

            if (collision.CompareTag("Suriken"))
            {
                collision.GetComponent<Suriken>().Reflejar(direccionReflejo);
            }
            else if (collision.CompareTag("BolaFuego"))
            {
                collision.GetComponent<BolaFuego>().Reflejar(direccionReflejo);
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
                collision.GetComponent<EnemyScript>().TakeDamage(Vector2.left);
                if (playerController.direccionAtaque != Vector2.down)
                {
                    playerController.StartKnockUp(Vector2.right);

                }

            }
            else
            {
                collision.GetComponent<EnemyScript>().TakeDamage(Vector2.right);
                if (playerController.direccionAtaque != Vector2.down)
                {
                    playerController.StartKnockUp(Vector2.left);

                }


            }

        }

    }
    
}
