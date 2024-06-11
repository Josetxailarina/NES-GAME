using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerController.damaged)
        {

            StartCoroutine(playerController.TakeDamage());
        }
    }
}
