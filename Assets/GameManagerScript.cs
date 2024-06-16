using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Play,
    Menu,
    GameOver,


}
public class GameManagerScript : MonoBehaviour
{
    public static GameMode modoJuego;
    // Start is called before the first frame update
    void Start()
    {
        modoJuego = GameMode.Play;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
