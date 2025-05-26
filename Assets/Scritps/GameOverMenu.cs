using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject suriken;
    [SerializeField] private Transform surikenEnd;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AudioSource menuMoveSound;
    [SerializeField] private AudioSource confirmSound;

    private EnemyScript[] enemys;
    private bool isUp = true;
    private Vector3 initialSurikenPosition;

    private void Start()
    {
        initialSurikenPosition = suriken.transform.localPosition;
        enemys = FindObjectsOfType<EnemyScript>();
        gameObject.SetActive(false);
    }
    public void GoUp()
    {
        if (!isUp)
        {
            suriken.transform.localPosition = initialSurikenPosition;
            menuMoveSound.Play();
            isUp = true;
        }
    }
    public void GoDown()
    {
        if (isUp)
        {
            suriken.transform.position = surikenEnd.transform.position;
            menuMoveSound.Play();
            isUp=false;
        }
    }
    public void ConfirmAction()
    {
        if (isUp)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1.0f;
            playerController.ResetSamurai(); 
            confirmSound.Play();
            foreach (EnemyScript script in enemys)
            {
                script.gameObject.SetActive(true);
                script.ResetEnemy();
            }
            GameManagerScript.modoJuego = GameMode.Play;
        }
        else
        {
            Application.Quit();
        }

    }
}
