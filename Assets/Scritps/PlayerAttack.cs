
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector] public Vector2 attackDirection { get; private set; }
    [HideInInspector] public bool isAttackLaunched;

    private float currentAttackCD;
    private PlayerController playerController;
    private Animator playerAnimator;
    [SerializeField] private float attackCD = 0.5f;



    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentAttackCD > 0)
        {
            currentAttackCD -= Time.deltaTime;
        }
    }
    public void Attack(InputAction.CallbackContext callBack)
    {
        if (callBack.performed && GameManagerScript.modoJuego == GameMode.Play && currentAttackCD <= 0)
        {
            HandleAttackDirection();
            playerAnimator.SetTrigger("Attack");
            currentAttackCD = attackCD;
            SoundsManager.Instance.playerAttackSound.Play();
        }
    }

    private void HandleAttackDirection()
    {
        if (playerController.moveInput.y > 0)
        {
            playerAnimator.SetFloat("AttackDirection", 1f);
            attackDirection = Vector2.up;
        }
        else if (playerController.moveInput.y < 0 && !playerController.isTouchingGround)
        {
            playerAnimator.SetFloat("AttackDirection", -1f);
            attackDirection = Vector2.down;
        }
        else
        {
            playerAnimator.SetFloat("AttackDirection", 0);
            if (playerController.isFacingLeft)
            {
                attackDirection = Vector2.left;
            }
            else
            {
                attackDirection = Vector2.right;
            }
        }
    }
}
