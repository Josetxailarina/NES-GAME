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
            animHit.transform.position = collision.transform.position;
            animHit.transform.rotation = transform.rotation;
            animHit.SetTrigger("Hit");
            collision.GetComponent<Suriken>().Reflejar(playerController.direccionAtaque);
            hitSound.Play();

        }
        else if (collision.CompareTag("NinjaM"))
        {
            collision.GetComponent<EnemyScript>().TakeDamage(playerController.direccionAtaque);
        }
    }
}
