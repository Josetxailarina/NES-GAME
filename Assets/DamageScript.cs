using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerController.parpadeando)
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
            {

                StartCoroutine(playerController.TakeDamage(new Vector2(-1,0)));
            }
            else
            {
                StartCoroutine(playerController.TakeDamage(new Vector2(1, 0)));

            }

        }
    }
}
