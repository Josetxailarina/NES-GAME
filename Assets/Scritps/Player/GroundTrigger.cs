using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    [SerializeField] private PlayerController controllerScript;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private AudioSource groundContactSound;
    private Vector3 lastSafePosition;

    private void Update()
    {
        CheckIfFalling();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        controllerScript.isTouchingGround = true;
        controllerScript.playerAnimator.SetBool("Jumping",false);
        lastSafePosition = transform.position;
        groundContactSound.Play();
        playerAttack.isPerformingAttackJump = false;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        controllerScript.isTouchingGround = false;
        controllerScript.playerAnimator.SetBool("Jumping", true);
    }
    private void CheckIfFalling()
    {
        if (transform.position.y < -10)
        {
            controllerScript.rb.velocity = Vector3.zero;
            controllerScript.transform.position = lastSafePosition;
            controllerScript.playerLivesScript.TakeDamage(Vector2.zero);
        }
    }
}
