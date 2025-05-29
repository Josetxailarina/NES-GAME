using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }

    [SerializeField] private Animator hitEffect;
    [SerializeField] private Animator redHitEffect;
    [SerializeField] private Animator explosion;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHitEffect(Vector2 position, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        hitEffect.transform.position = position;
        hitEffect.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        hitEffect.SetTrigger("Hit");
        SoundsManager.Instance.hitSound.Play();
    }
    public void PlayRedHitEffect(Vector2 position, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        redHitEffect.transform.position = position;
        redHitEffect.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        redHitEffect.SetTrigger("Hit");
        SoundsManager.Instance.hitSound.Play();
    }
    public void PlayExplosion(Vector2 position)
    {
        explosion.transform.position = position;
        explosion.SetTrigger("Hit");
        SoundsManager.Instance.explosionSound.Play();
    }
}
