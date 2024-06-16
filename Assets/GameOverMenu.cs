using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverMenu : MonoBehaviour
{
    public GameObject suriken;
    public Transform surikenEnd;
    public Vector3 posicionInicialSuriken;
    public PlayerController playerController;
    public bool arriba=true;
    public AudioSource moverMenuSound;
    public AudioSource aceptarSound;
    public EnemyScript[] enemys;

    private void Start()
    {
        posicionInicialSuriken = suriken.transform.localPosition;
        enemys = FindObjectsOfType<EnemyScript>();
        gameObject.SetActive(false);
    }
    public void Subir()
    {
        if (!arriba)
        {
            suriken.transform.localPosition = posicionInicialSuriken;
            moverMenuSound.Play();
            arriba = true;
        }
    }
    public void Bajar()
    {
        if (arriba)
        {
            suriken.transform.position = surikenEnd.transform.position;
            moverMenuSound.Play();
            arriba=false;

        }
    }
    public void Aceptar()
    {
        if (arriba)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1.0f;
            playerController.ResetSamurai(); 
            aceptarSound.Play();
            foreach (EnemyScript script in enemys)
            {
                print("reiniciando a "+ script.gameObject.name);
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
