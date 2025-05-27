using System.Collections;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] private int currentLives = 3;
    [SerializeField] private Sprite[] livesNumberSprites;
    [SerializeField] private SpriteRenderer livesDisplayRenderer;
    [SerializeField] private GameOverMenu gameOverScript;
    [HideInInspector] public bool isFlashing;
    private SpriteRenderer playerSprite;
    private PlayerController playerController;

    private void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(Vector2 direccion)
    {
        currentLives--;
        livesDisplayRenderer.sprite = livesNumberSprites[currentLives];
        SoundsManager.Instance.playerDamageSound.Play();

        if (currentLives > 0)
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
        currentLives = 3;
        livesDisplayRenderer.sprite = livesNumberSprites[currentLives];
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
        playerSprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        playerSprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        playerSprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        playerSprite.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        playerSprite.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        playerSprite.enabled = true;
        isFlashing = false;
    }
}
