using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject suriken;
    [SerializeField] private Transform endPosition;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject theEndMenu;

    private EnemyScript[] enemyScripts;
    private Projectile[] proyectiles;
    private bool isUp = true;
    private Vector3 continuePosition;

    private void Start()
    {
        continuePosition = suriken.transform.localPosition;
        enemyScripts = FindObjectsOfType<EnemyScript>();
        proyectiles = FindObjectsOfType<Projectile>();
    }

    public void MenuMove(InputAction.CallbackContext callBack)
    {
        if (callBack.performed && GameManagerScript.gameMode == GameMode.Menu)
        {
            if (isUp && callBack.ReadValue<Vector2>().y == -1)
            {
                GoDown();
            }
            else if(!isUp && callBack.ReadValue<Vector2>().y == 1)
            {
                GoUp();
            }
        }
    }
    public void MenuConfirm(InputAction.CallbackContext callBack)
    {
        if (callBack.performed && GameManagerScript.gameMode == GameMode.Menu)
        {
            ConfirmAction();
        }
    }
    public void GoUp()
    {
        if (!isUp)
        {
            suriken.transform.localPosition = continuePosition;
            SoundsManager.Instance.moveMenuSound.Play();
            isUp = true;
        }
    }
    public void GoDown()
    {
        if (isUp)
        {
            suriken.transform.position = endPosition.transform.position;
            SoundsManager.Instance.moveMenuSound.Play();
            isUp =false;
        }
    }
    public void ConfirmAction()
    {
        if (isUp)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1.0f;
            playerController.ResetPlayer(); 
            SoundsManager.Instance.confirmMenuSound.Play();
            foreach (EnemyScript script in enemyScripts)
            {
                script.gameObject.SetActive(true);
                script.ResetEnemy();
            }
            foreach (Projectile projectile in proyectiles)
            {
                projectile.ResetProjectile();
            }
            GameManagerScript.gameMode = GameMode.Play;
        }
        else
        {
            Application.Quit();
        }
    }
    public void ShowEndMenu() 
    {
        gameOverMenu.SetActive(false);
        theEndMenu.SetActive(true);
    }

}
