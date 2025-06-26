using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBack : MonoBehaviour
{
    public void volverMenuPrincipal()
    {
        GameSession.EndTimer();
        GameManagerPiles.GenerateStatsLog();
        Invoke(nameof(FirebaseNMenu), 0.1f);
    }

    private void FirebaseNMenu()
    {
        FirebaseController.instancia.EnviarDatos();
        SceneManager.LoadScene("principalMenu");
    }
}