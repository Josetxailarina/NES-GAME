using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerLives = 3;
    [SerializeField] private Sprite[] livesNumberSprites;
    [SerializeField] private SpriteRenderer livesDisplayRenderer;
    [SerializeField] private GameOverMenu gameOverScript;
    [HideInInspector] public bool isFlashing;
    private SpriteRenderer sprite;
    private PlayerController playerController;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(Vector2 direccion)
    {
        playerLives--;
        livesDisplayRenderer.sprite = livesNumberSprites[playerLives];
        SoundsManager.Instance.playerDamageSound.Play();

        if (playerLives > 0)
        {
            StartCoroutine(FlashDamageEffect(direccion));
        }
        else
        {
            StartCoroutine(ShowGameOverPanel());
        }
        
    }
    public void ResetLives()
    {
        playerLives = 3;
        livesDisplayRenderer.sprite = livesNumberSprites[playerLives];
        SoundsManager.Instance.backgroundMusic.Play();
        SoundsManager.Instance.gameOverMusic.Stop();
    }

    IEnumerator ShowGameOverPanel()
    {
        playerController.playerAnimator.SetTrigger("Dead");
        Time.timeScale = 0;
        SoundsManager.Instance.backgroundMusic.Stop();
        SoundsManager.Instance.gameOverMusic.Play();
        GameManagerScript.modoJuego = GameMode.GameOver;

        yield return new WaitForSecondsRealtime(1.7f);

        gameOverScript.gameObject.SetActive(true);
        GameManagerScript.modoJuego = GameMode.Menu;
    }

    public IEnumerator FlashDamageEffect(Vector2 direccion)
    {
        isFlashing = true;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
        playerController.StartKnockUp(direccion);
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.enabled = true;
        isFlashing = false;
    }
}
