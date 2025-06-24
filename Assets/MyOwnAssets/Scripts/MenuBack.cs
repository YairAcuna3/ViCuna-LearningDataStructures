using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBack : MonoBehaviour
{
    public void volverMenuPrincipal()
    {
        SceneManager.LoadScene("principalMenu");
        GameSession.EndTimer();
        GameManagerPiles.GenerateStatsLog();
    }
}