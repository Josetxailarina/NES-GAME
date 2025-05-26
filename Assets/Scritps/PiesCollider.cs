using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiesCollider : MonoBehaviour
{
    public PlayerController controllerScript;
    private Vector3 ultimaPosicionSegura;
    public AudioSource aterrizajeSound;
    private void Update()
    {
        if (transform.position.y < -10)
        {
            print("Limite alcanzado, ultima posicion segura: " + ultimaPosicionSegura);
            controllerScript.rb.velocity = Vector3.zero;
            controllerScript.transform.position = ultimaPosicionSegura;
            StartCoroutine(controllerScript.TakeDamage(Vector2.zero));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        controllerScript.tocandoSuelo = true;
        controllerScript.anim.SetBool("Jumping",false);
        ultimaPosicionSegura = transform.position;
        aterrizajeSound.Play();
        controllerScript.impulsoAttack = false;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        controllerScript.tocandoSuelo = false;
        controllerScript.anim.SetBool("Jumping", true);

    }
}
