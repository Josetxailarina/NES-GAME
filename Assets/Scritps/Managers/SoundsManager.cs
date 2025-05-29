using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance { get; private set; }

    public AudioSource backgroundMusic;
    public AudioSource gameOverMusic;
    public AudioSource explosionSound;
    public AudioSource playerDamageSound;
    public AudioSource playerAttackSound;
    public AudioSource jumpSound;
    public AudioSource hitSound;
    public AudioSource moveMenuSound;
    public AudioSource confirmMenuSound;
    public AudioSource fireShotSound;
    public AudioSource armorHitSound;
    public AudioSource chargingFireSound;

    private void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
