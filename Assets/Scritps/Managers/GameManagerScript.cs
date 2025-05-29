using UnityEngine;

public enum GameMode
{
    Play,
    Menu,
    GameOver,
}
public class GameManagerScript : MonoBehaviour
{
    public static GameMode gameMode;

    void Start()
    {
        gameMode = GameMode.Play;
    }
}
