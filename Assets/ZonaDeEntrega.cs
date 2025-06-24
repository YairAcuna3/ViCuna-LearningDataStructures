using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ZonaDeEntrega : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip audioSiguientePaso;
    public AudioClip sonidoCajaIncorrecta; // Nuevo sonido para caja incorrecta
    private AudioSource audioSource;
    private bool audioReproducido = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"🔍 Estado actual de la cola: {string.Join(", ", GameManager.instancia.ObtenerEstadoColaPeek())}");
        if (!other.CompareTag("Caja"))
        {
            Debug.Log("⛔ Objeto ignorado: " + other.name);
            return;
        }

        Caja caja = other.GetComponent<Caja>();
        if (caja == null)
        {
            Debug.LogError("⚠️ El objeto con tag 'Caja' no tiene componente Caja");
            return;
        }

        int idCaja = caja.id;
        int idEsperado = GameManager.instancia.VerIDEsperado();
        Debug.Log($"🧪 Validando caja con ID {idCaja}, se espera ID {idEsperado}");

        if (idCaja == idEsperado)
        {
            // !ACIERTOUWU
            Debug.Log($"✅ Caja entregada correctamente: {idCaja}");
            caja.CambiarColor(Color.green);
            GameManager.instancia.ConfirmarCajaCorrecta();

            // Efecto visual (puedes cambiarlo por animación o desactivación)
            Destroy(caja.gameObject, 1f);

            if (!GameManager.instancia.HayIDsRestantes())
            {
                if (audioSiguientePaso != null && !audioReproducido)
                {
                    audioSource.PlayOneShot(audioSiguientePaso);
                    audioReproducido = true;
                    Debug.Log("🔊 COMPLETADO!");
                    //! TIEMPO FINAL
                }
            }
        }
        else
        {
            Debug.Log($"❌ Caja incorrecta entregada: {idCaja}. Se esperaba: {idEsperado}");
            // TODO:ERRORUWU

            // Reproducir sonido de error
            if (sonidoCajaIncorrecta != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoCajaIncorrecta);
            }

            // Obtener el EvaluadorSlot original de la caja
            EvaluadorSlot slotOriginal = caja.ObtenerSlotOriginal();
            if (slotOriginal != null)
            {
                // Devolver la caja a su posición original
                DevolverCajaASlot(caja, slotOriginal);
            }
            else
            {
                Debug.LogWarning("⚠️ No se encontró el slot original de la caja");
                caja.CambiarColor(Color.red);
            }
        }
    }

    private void DevolverCajaASlot(Caja caja, EvaluadorSlot slot)
    {
        // Mover la caja de vuelta al slot
        StartCoroutine(MoverCajaASlot(caja, slot));
    }

    private IEnumerator MoverCajaASlot(Caja caja, EvaluadorSlot slot)
    {
        Vector3 posicionInicial = caja.transform.position;
        Vector3 posicionFinal = slot.transform.position;
        float duracion = 0.5f; // Duración de la animación
        float tiempoTranscurrido = 0f;

        // Desactivar temporalmente la interacción XR durante el movimiento
        var interactable = caja.GetComponent<IXRSelectInteractable>();
        if (interactable != null)
        {
            // Desactivar temporalmente el interactable
            var xrInteractable = caja.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
            if (xrInteractable != null)
            {
                xrInteractable.enabled = false;
            }
        }

        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / duracion;

            // Interpolación suave
            caja.transform.position = Vector3.Lerp(posicionInicial, posicionFinal, t);

            yield return null;
        }

        // Asegurar que la caja esté exactamente en la posición final
        caja.transform.position = posicionFinal;

        // Reactivar la interacción XR
        if (interactable != null)
        {
            var xrInteractable = caja.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
            if (xrInteractable != null)
            {
                xrInteractable.enabled = true;
            }
        }

        // Habilitar el socket para que pueda recibir la caja
        slot.HabilitarSocket();

        Debug.Log("✅ Caja devuelta a su posición original");
    }
}
