using Cinemachine;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    public static Vector3 lastCheckpointPosition;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private AudioSource audioCheck;
    private bool isRaised;
    private Animator anim;
    private Vector3 checkpointOffSet = new Vector3(0.5f, 0.5f, 0);


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        audioCheck = GetComponent<AudioSource>();   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isRaised)
        {
            anim.SetBool("Up", true);
            audioCheck.Play();
            lastCheckpointPosition = transform.position - checkpointOffSet;
            isRaised = true;
            if (virtualCamera != null)
            {
                virtualCamera.Priority = 11;
            }
        }
    }
}
