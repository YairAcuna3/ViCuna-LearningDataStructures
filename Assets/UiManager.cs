using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    // Llama a este m�todo desde el bot�n "Salir"
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo de la escena...");
        GameSession.QueueEndTimer();
        SceneManager.LoadScene("principalMenu");

        // En el editor de Unity, esto no cerrar� el juego, pero en el build s�.
        // #if UNITY_EDITOR
        //         UnityEditor.EditorApplication.isPlaying = false;
        // #endif
    }

    // Llama a este m�todo desde el bot�n "Resetear escena"
    public void ReiniciarEscena()
    {
        Debug.Log("Reiniciando escena...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
