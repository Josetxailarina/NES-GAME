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

    void Start()
    {
        modoJuego = GameMode.Play;
    }
}
