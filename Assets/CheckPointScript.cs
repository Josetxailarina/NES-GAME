using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    private PlayerController playerController;
    private AudioSource audioCheck;
    private bool levantada;
    private Animator anim;
    public CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        audioCheck = GetComponent<AudioSource>();   
        playerController = GameObject.FindGameObjectWithTag("Samurai").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Samurai")&&!levantada)
        {
            anim.SetBool("Up", true);
            audioCheck.Play();
            playerController.posicionLastCheckpoint = transform.position - new Vector3(0.5f, 0.5f, 0);
            levantada = true;
            if (virtualCamera != null)
            {
                virtualCamera.Priority = 11;
            }
        }
    }
}
