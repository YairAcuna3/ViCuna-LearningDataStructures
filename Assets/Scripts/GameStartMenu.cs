using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartMenu : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    public Button colasButton;
    public Button pilasButton;
    public Button quitButton;

    void Start()
    {
        colasButton.onClick.AddListener(LoadColasScene);
        pilasButton.onClick.AddListener(LoadPilasScene);
        quitButton.onClick.AddListener(QuitGame);

        // Solo iniciar el tiempo si aún no se ha iniciado
        // if (string.IsNullOrEmpty(GameSession.startTime))
        // {
        //     GameSession.StartTimer();
        // }
    }

    public void LoadColasScene()
    {
        SceneManager.LoadScene("ColasScene");
    }

    public void LoadPilasScene()
    {
        SceneManager.LoadScene("PilesScene");
    }

    public void QuitGame()
    {
        // GameSession.EndTimer();
        GameSession.GenerateStatsLog();
        Application.Quit();
        Debug.Log("Juego cerrado.");
    }
}
