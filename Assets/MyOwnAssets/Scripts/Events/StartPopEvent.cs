using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StartPopEvent : MonoBehaviour
{
    [Header("Audio explicando lo que sigue luego de apilar")]
    public AudioSource audioSource;

    [Header("Objeto más alto en la pila que se volverá a activar")]
    public GameObject topCube;
    public GameObject midCube;
    public GameObject bottomCube;
    public GameObject nextSocket; // Ahora es innecesario
    public GameObject thisSocket; // Ahora es innecesario
    public MaterialManager avatar;

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            StartCoroutine(WaitForAudioToEnd());
        }
        else
        {
            Debug.LogWarning("AudioSource no asignado en StartPopEvent");
        }
    }

    private IEnumerator WaitForAudioToEnd()
    {
        // Esperar a que el audio empiece (por si hay un pequeño retraso)
        while (!audioSource.isPlaying)
        {
            yield return null;
        }

        // Esperar a que termine de reproducirse
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        if (topCube != null)
        {
            nextSocket.SetActive(true); // Ahora es innecesario
            topCube.GetComponent<XRGrabInteractable>().enabled = true;
            topCube.GetComponent<Rigidbody>().isKinematic = false;
            midCube.GetComponent<XRGrabInteractable>().enabled = true;
            midCube.GetComponent<Rigidbody>().isKinematic = false;
            bottomCube.GetComponent<XRGrabInteractable>().enabled = true;
            bottomCube.GetComponent<Rigidbody>().isKinematic = false;
            avatar.setDefaultMaterial();
            thisSocket.SetActive(false); // Ahora es innecesario
        }
    }
}
