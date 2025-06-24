using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [Header("Materiales para asignar")]
    public Material defaultMaterial;
    public Material errorMaterial;
    public Material aciertoMaterial;
    public Material midEventMaterial;
    public Material finalEventMaterial;

    [Header("Objeto al que se le asignarán los materiales")]
    public Renderer avatarObject;

    public void setDefaultMaterial()
    {
        if (avatarObject != null && defaultMaterial != null)
        {
            avatarObject.material = defaultMaterial;
        }
    }

    public void setErrorMaterial()
    {
        if (avatarObject != null && errorMaterial != null)
        {
            avatarObject.material = errorMaterial;
        }
    }

    public void setAciertoMaterial()
    {
        if (avatarObject != null && aciertoMaterial != null)
        {
            avatarObject.material = aciertoMaterial;
        }
    }

    public void setMidMaterial()
    {
        if (avatarObject != null && midEventMaterial != null)
        {
            avatarObject.material = midEventMaterial;
        }
    }

    public void setFinalMaterial()
    {
        if (avatarObject != null && finalEventMaterial != null)
        {
            avatarObject.material = finalEventMaterial;
        }
    }
}
