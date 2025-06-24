using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class EvaluadorSlot : MonoBehaviour
{
    public CajaSpawn cajaSpawn;
    private XRSocketInteractor socket;
    public int posicionEsperada;
    private int cajasCorrectasColocadas = 0;
    private HashSet<int> idsRegistrados = new HashSet<int>();
    private bool sonidoExitoReproducido = false;

    // --- NUEVO: Referencias de audio ---
    [Header("Audio")]
    public AudioClip sonidoExito;
    public AudioClip sonidoError;
    public AudioClip sonidoError2;
    public AudioClip sonidoOrdenNoEnviado;
    private AudioSource audioSource;


    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        // --- NUEVO: Inicializar AudioSource ---
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        // Suscribirse al evento cuando una caja entra al socket
        socket.selectEntered.AddListener(OnCajaColocada);

        socket.selectExited.AddListener(OnCajaRetirada);
    }

    private void OnDestroy()
    {
        // Es buena práctica eliminar listeners
        socket.selectEntered.RemoveListener(OnCajaColocada);
        socket.selectExited.RemoveListener(OnCajaRetirada);
    }

    private void OnCajaRetirada(SelectExitEventArgs args)
    {
        GameObject cajaGO = args.interactableObject.transform.gameObject;
        Caja caja = cajaGO.GetComponent<Caja>();

        if (caja == null)
            return;

        int idCaja = caja.id;
        int idEsperado = GameManager.instancia.VerIDEsperado();

        Debug.Log($"🔄 Caja retirada del slot - ID: {idCaja}, ID esperado actual: {idEsperado}");

        // Verificar si la caja retirada es la que debería retirarse
        if (idCaja != idEsperado)
        {
            Debug.Log($"❌ Error: Se retiró la caja {idCaja} cuando se esperaba la caja {idEsperado}");
            // TODO:ERRORUWU
            GameSession.AddQueueWrongAction();

            // Reproducir sonido de error
            if (sonidoError2 != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoError2);
                Debug.Log("🎵 Reproduciendo sonido de error - Caja retirada en orden incorrecto");
            }

            // Cambiar color a rojo para indicar error
            caja.CambiarColor(Color.red);
        }
        else
        {
            // Si es la caja correcta, cambiar a cyan
            caja.CambiarColor(Color.cyan);
            Debug.Log("✅ Caja retirada en el orden correcto");
            // !ACIERTOUWU
            GameSession.AddQueueCorrectAction();
        }
    }



    private void OnCajaColocada(SelectEnterEventArgs args)
    {
        GameObject cajaGO = args.interactableObject.transform.gameObject;
        Caja caja = cajaGO.GetComponent<Caja>();

        if (caja != null)
        {
            // Registrar este slot como el original de la caja
            caja.EstablecerSlotOriginal(this);
            EvaluarCaja(caja.id);
            Debug.Log("📦 Evento OnCajaColocada disparado");
        }
    }

    private void EvaluarCaja(int idColocado)
    {
        int idEsperado = GameManager.instancia.ObtenerIDEsperadoEnPosicion(posicionEsperada);
        Caja caja = null;

        if (socket.interactablesSelected.Count > 0)
        {
            caja = socket.interactablesSelected[0].transform.GetComponent<Caja>();
        }

        if (idEsperado == -1)
        {
            Debug.LogWarning("❌ El orden no ha sido enviado");
            // TODO:ERRORUWU
            GameSession.AddQueueWrongAction();
            if (sonidoOrdenNoEnviado != null && audioSource != null)
                audioSource.PlayOneShot(sonidoOrdenNoEnviado);

            socket.enabled = false;

            if (caja != null && socket != null)
            {
                socket.interactionManager.SelectExit(socket, caja.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable>());
            }
            return;
        }

        // Verificar si la caja colocada corresponde a la posición esperada
        if (idColocado == idEsperado)
        {
            // !ACIERTOUWU
            GameSession.AddQueueCorrectAction();
            Debug.Log($"✅ Caja correcta en posición: {posicionEsperada}. ID esperado: {idEsperado}, ID colocado: {idColocado}");

            if (caja != null)
            {
                caja.CambiarColor(Color.green);
            }

            // Solo contar si no se ha registrado antes este ID
            if (!idsRegistrados.Contains(idColocado))
            {
                cajasCorrectasColocadas++;
                idsRegistrados.Add(idColocado);
                GameManager.instancia.IncrementarCajasCorrectas();
                Debug.Log($"📊 Cajas correctas colocadas en slot {posicionEsperada}: {cajasCorrectasColocadas}");
            }

            // Verificar si es la última caja (ID 3) en la posición correcta (posición 2)
            if (idColocado == 3 && posicionEsperada == 2)
            {
                // Obtener el total de cajas correctas de todos los slots
                int totalCajasCorrectas = GameManager.instancia.ObtenerTotalCajasCorrectas();
                Debug.Log($"🎯 Última caja (ID 3) colocada en posición {posicionEsperada}");
                Debug.Log($"🎯 Total de cajas correctas en todos los slots: {totalCajasCorrectas}");
                Debug.Log($"🎯 IDs registrados en este slot: {string.Join(", ", idsRegistrados)}");

                // Reproducir sonido solo si todas las cajas están correctas
                if (totalCajasCorrectas == 3)
                {
                    Debug.Log("🎵 ¡CONDICIONES CUMPLIDAS! Reproduciendo sonido de éxito:");
                    Debug.Log($"   - ID colocado (3) en posición {posicionEsperada}");
                    Debug.Log($"   - Total cajas correctas: {totalCajasCorrectas}");
                    Debug.Log($"   - Sonido disponible: {(sonidoExito != null ? "Sí" : "No")}");
                    Debug.Log($"   - AudioSource disponible: {(audioSource != null ? "Sí" : "No")}");

                    if (sonidoExito != null && audioSource != null && !sonidoExitoReproducido)
                    {
                        audioSource.PlayOneShot(sonidoExito);
                        Debug.Log("✅ Sonido de éxito reproducido correctamente");
                        sonidoExitoReproducido = true;
                    }
                    else
                    {
                        Debug.LogWarning("⚠️ No se pudo reproducir sonido de éxito:");
                        if (sonidoExito == null) Debug.LogWarning("   - sonidoExito es null");
                        if (audioSource == null) Debug.LogWarning("   - audioSource es null");
                    }
                }
                else
                {
                    Debug.Log($"❌ No se reproduce sonido porque totalCajasCorrectas ({totalCajasCorrectas}) != 3");
                }
            }
        }
        else
        {
            Debug.Log($"❌ Caja incorrecta en posición: {posicionEsperada}. Esperado ID: {idEsperado}, pero se colocó: {idColocado}");
            // TODO:ERRORUWU
            GameSession.AddQueueWrongAction();

            if (caja != null)
            {
                caja.CambiarColor(Color.red);
            }

            // Reproducir sonido de error cuando se coloca una caja incorrecta
            if (sonidoError != null && audioSource != null)
            {
                Debug.Log("🎵 Reproduciendo sonido de error - Caja incorrecta");
                audioSource.PlayOneShot(sonidoError);
            }
            else
            {
                Debug.LogWarning("⚠️ No se pudo reproducir sonido de error - sonidoError o audioSource es null");
            }
        }
    }

    public void HabilitarSocket()
    {
        socket.enabled = true;
    }

    // Método para resetear el estado del sonido de éxito
    public void ResetearSonidoExito()
    {
        sonidoExitoReproducido = false;
        Debug.Log("🔄 Sonido de éxito reseteado");
    }
}
