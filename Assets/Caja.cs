using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Caja : MonoBehaviour
{
    public int id; // Se asigna desde el spawner

    private TextMesh textMesh;
    private Renderer cajaRenderer;
    private EvaluadorSlot slotOriginal; // Referencia al slot original

    void Start()
    {
        // Busca el componente TextMesh dentro del hijo
        textMesh = GetComponentInChildren<TextMesh>();
        cajaRenderer = GetComponent<Renderer>();
        ActualizarTexto();
    }

    // Por si quieres actualizar el ID dinámicamente antes de Start
    public void AsignarID(int nuevoID)
    {
        id = nuevoID;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        if (textMesh != null)
        {
            textMesh.text = id.ToString();
        }
    }

    public void CambiarColor(Color color)
    {
        if (cajaRenderer == null)
            cajaRenderer = GetComponent<Renderer>();

        if (cajaRenderer != null)
            cajaRenderer.material.color = color;
    }

    // Método para establecer el slot original
    public void EstablecerSlotOriginal(EvaluadorSlot slot)
    {
        slotOriginal = slot;
    }

    // Método para obtener el slot original
    public EvaluadorSlot ObtenerSlotOriginal()
    {
        return slotOriginal;
    }
}
