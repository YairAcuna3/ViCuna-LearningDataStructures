using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ConveyorBeltSurface : MonoBehaviour
{
    public float velocidad = 0f;
    public Vector3 direccion = Vector3.right;

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            // Solo afectamos la velocidad en el plano X-Z
            Vector3 movimientoDeseado = direccion.normalized * velocidad;
            movimientoDeseado.y = rb.linearVelocity.y; // Conservamos la velocidad vertical (si la hay)

            // Asignamos la velocidad directamente
            rb.linearVelocity = movimientoDeseado;

            // Corregimos rotación para que no gire (sin constraints)
            rb.angularVelocity = Vector3.zero;
        }
    }
}
