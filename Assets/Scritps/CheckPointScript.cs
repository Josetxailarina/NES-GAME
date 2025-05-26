using Cinemachine;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    private PlayerController playerController;
    private AudioSource audioCheck;
    private bool isRaised;
    private Animator anim;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        audioCheck = GetComponent<AudioSource>();   
        playerController = GameObject.FindGameObjectWithTag("Samurai").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Samurai")&&!isRaised)
        {
            anim.SetBool("Up", true);
            audioCheck.Play();
            playerController.posicionLastCheckpoint = transform.position - new Vector3(0.5f, 0.5f, 0);
            isRaised = true;
            if (virtualCamera != null)
            {
                virtualCamera.Priority = 11;
            }
        }
    }
}
