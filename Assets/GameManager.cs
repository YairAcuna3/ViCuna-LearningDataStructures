using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    private Queue<int> colaCorrecta = new Queue<int>();
    public List<int> ordenCorrecto = new List<int>();
    private int cajasCorrectasTotal = 0; // Nueva variable para llevar el conteo total

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            //! TIEMPO INICIO
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public List<int> ObtenerEstadoColaPeek()
    {
        return new List<int>(colaCorrecta); // Retorna una copia para debugging
    }
    public void RegistrarID(int id)
    {
        // Solo registra si no está ya en la cola
        if (!colaCorrecta.Contains(id))
        {
            colaCorrecta.Enqueue(id);
            Debug.Log("📦 Encolado en GameManager: " + id);
        }
        else
        {
            Debug.Log("⚠️ ID ya existente en la cola: " + id);
        }
    }

    public int VerIDEsperado()
    {
        if (colaCorrecta.Count > 0)
            return colaCorrecta.Peek(); // Solo lo mira, no lo saca
        return -1;
    }

    public void ConfirmarCajaCorrecta()
    {
        if (colaCorrecta.Count > 0)
        {
            int idDesencolado = colaCorrecta.Dequeue();
            Debug.Log($"🚚 Desencolado ID: {idDesencolado}, Próximo en cola: {(colaCorrecta.Count > 0 ? colaCorrecta.Peek().ToString() : "ninguno")}");
        }
    }

    public bool HayIDsRestantes()
    {
        return colaCorrecta.Count > 0;
    }

    public void EstablecerOrdenCorrecto(List<int> orden)
    {
        ordenCorrecto = orden;
        colaCorrecta.Clear(); // Asegura que no haya residuos
        ResetearCajasCorrectas(); // Resetear el contador cuando se establece nuevo orden

        // Resetear el sonido de éxito en todos los EvaluadorSlots
        EvaluadorSlot[] evaluadorSlots = FindObjectsOfType<EvaluadorSlot>();
        foreach (EvaluadorSlot slot in evaluadorSlots)
        {
            slot.ResetearSonidoExito();
        }

        foreach (int id in ordenCorrecto)
        {
            colaCorrecta.Enqueue(id); // Agrega a la cola
        }

        Debug.Log("📋 Orden correcto cargado: " + string.Join(", ", ordenCorrecto));
    }

    public int ObtenerIDEsperadoEnPosicion(int posicion)
    {
        if (posicion >= 0 && posicion < ordenCorrecto.Count)
            return ordenCorrecto[posicion];
        return -1;
    }

    // Nuevo método para obtener el total de cajas correctas
    public int ObtenerTotalCajasCorrectas()
    {
        return cajasCorrectasTotal;
    }

    // Nuevo método para incrementar el contador de cajas correctas
    public void IncrementarCajasCorrectas()
    {
        cajasCorrectasTotal++;
        Debug.Log($"📊 Total de cajas correctas: {cajasCorrectasTotal}");
    }

    // Nuevo método para resetear el contador de cajas correctas
    public void ResetearCajasCorrectas()
    {
        cajasCorrectasTotal = 0;
        Debug.Log("🔄 Contador de cajas correctas reseteado");
    }
}