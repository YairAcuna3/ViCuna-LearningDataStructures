using UnityEngine;

public class StartEndEvent : MonoBehaviour
{
    [Header("Audio hablando del final del juego")]
    public AudioSource audioSource;

    [Header("Objeto más alto en la pila que se volverá a activar")]
    public GameObject lastCube;
    public GameObject thisSocket;

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Destroy(lastCube);
            thisSocket.SetActive(false);
            // Agregar el display con las estadísticas
        }
    }
}
