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
        animHit.transform.position = collision.transform.position;
        animHit.transform.rotation = transform.rotation;
        animHit.SetTrigger("Hit");
        hitSound.Play();
        if (playerController.direccionAtaque == Vector2.down)
        {
            playerController.PerformJumpAttack();
        }
        if (collision.CompareTag("Suriken"))
        {
            collision.GetComponent<Suriken>().Reflejar(playerController.direccionAtaque);
        }
        else if (collision.CompareTag("NinjaM"))
        {
            collision.GetComponent<EnemyScript>().TakeDamage(playerController.direccionAtaque);
        }
    }
}
