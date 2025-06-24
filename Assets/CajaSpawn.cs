using UnityEngine;
using System.Collections.Generic;
using System.Collections; // Añadido para usar corrutinas
using UnityEngine.UI; // Añadido para UI
                      // Añadido para XR Interaction

public class CajaSpawn : MonoBehaviour
{
    [Header("Caja a instanciar")]
    public GameObject cajaPrefab;

    [Header("Posición donde aparecerán las cajas")]
    public Transform puntoDeSpawn;

    [Header("Configuración de Cooldown")]
    public float tiempoCooldown = 0.5f; // Tiempo de espera entre instanciaciones
    private bool puedeInstanciar = true; // Flag para controlar el cooldown

    [Header("UI y Botón")]
    public Text mensajeMaxCajas; // Referencia al texto UI
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable botonSpawn; // Referencia al botón XR
    public float tiempoMensajeVisible = 2f; // Tiempo que se muestra el mensaje

    private int contadorID = 1;
    private int cajasInstanciadas = 0;
    private const int maxCajas = 3;

    private Queue<int> ordenCorrecto = new Queue<int>();

    void Start()
    {
        // Asegurarse de que el mensaje esté oculto al inicio
        if (mensajeMaxCajas != null)
        {
            mensajeMaxCajas.gameObject.SetActive(false);
        }
    }

    public void InstanciarCaja()
    {
        if (!puedeInstanciar)
        {
            Debug.Log("⏳ Esperando cooldown...");
            return;
        }

        if (cajasInstanciadas >= maxCajas)
        {
            Debug.Log("⚠️ Ya se instanciaron el máximo de cajas.");
            MostrarMensajeMaxCajas();
            DeshabilitarBoton();
            return;
        }

        if (cajaPrefab != null && puntoDeSpawn != null)
        {
            GameObject nuevaCaja = Instantiate(cajaPrefab, puntoDeSpawn.position, puntoDeSpawn.rotation);

            Caja scriptCaja = nuevaCaja.GetComponent<Caja>();
            if (scriptCaja != null)
            {
                scriptCaja.AsignarID(contadorID);
                ordenCorrecto.Enqueue(contadorID);
                contadorID++;
                cajasInstanciadas++;
                Debug.Log($"📦 Caja {contadorID - 1} instanciada.");

                // Verificar si alcanzamos el máximo de cajas
                if (cajasInstanciadas >= maxCajas)
                {
                    MostrarMensajeMaxCajas();
                    DeshabilitarBoton();
                }

                // Iniciar el cooldown
                StartCoroutine(CooldownInstanciacion());
            }
        }
    }

    private void MostrarMensajeMaxCajas()
    {
        if (mensajeMaxCajas != null)
        {
            mensajeMaxCajas.gameObject.SetActive(true);
            mensajeMaxCajas.text = "¡Máximo de cajas alcanzado!";
            StartCoroutine(OcultarMensajeDespuesDeTiempo());
        }
    }

    private IEnumerator OcultarMensajeDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoMensajeVisible);
        if (mensajeMaxCajas != null)
        {
            mensajeMaxCajas.gameObject.SetActive(false);
        }
    }

    private void DeshabilitarBoton()
    {
        if (botonSpawn != null)
        {
            botonSpawn.enabled = false;
            // Opcional: Cambiar el color o apariencia del botón para indicar que está deshabilitado
            var renderer = botonSpawn.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.gray;
            }
        }
    }

    // Método para resetear el sistema (por ejemplo, cuando se inicia un nuevo juego)
    public void ResetearSistema()
    {
        cajasInstanciadas = 0;
        contadorID = 1;
        ordenCorrecto.Clear();
        puedeInstanciar = true;

        // Reactivar el botón
        if (botonSpawn != null)
        {
            botonSpawn.enabled = true;
            var renderer = botonSpawn.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.white; // O el color original del botón
            }
        }

        // Ocultar mensaje si está visible
        if (mensajeMaxCajas != null)
        {
            mensajeMaxCajas.gameObject.SetActive(false);
        }
    }

    private IEnumerator CooldownInstanciacion()
    {
        puedeInstanciar = false;
        Debug.Log($"⏳ Iniciando cooldown de {tiempoCooldown} segundos...");
        yield return new WaitForSeconds(tiempoCooldown);
        puedeInstanciar = true;
        Debug.Log("✅ Cooldown completado, se puede instanciar otra caja");
    }

    public Queue<int> ObtenerOrdenCorrecto()
    {
        return new Queue<int>(ordenCorrecto); // copia segura
    }

    public void EnviarOrdenAlGameManager()
    {
        GameManager.instancia.EstablecerOrdenCorrecto(new List<int>(ordenCorrecto));
        Debug.Log($"📤 Orden enviado al GameManager: {string.Join(", ", ordenCorrecto)}");
    }
}
