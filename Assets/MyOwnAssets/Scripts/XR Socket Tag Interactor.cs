using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRSocketTagInteractor : XRSocketInteractor
{
    public GameObject thisCube;
    public GameObject nextSocket;
    public string idCubeTag;
    public AudioManagerPiles audioManager;
    public TextManager textManager;
    public MaterialManager materialManager;
    private bool hasPlayedInvalidSound = false;
    private bool hasPlayedValidSound = false;
    public bool isEvent;
    public bool isPop;
    public GameObject startEvent;
    public GameObject pad;

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(OnObjectEntered);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(OnObjectEntered);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        bool isValid = interactable.transform.CompareTag(idCubeTag);
        return base.CanSelect(interactable) && isValid;
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        bool isValid = interactable.transform.CompareTag(idCubeTag);

        if (!isValid)
        {
            CambiarColor(pad, new Color(1, 0.392f, 0.392f)); //🔴
            TryPlayInvalidHoverSound();
        }
        else
        {
            CambiarColor(pad, new Color(0.659f, 1f, 0.675f)); //💚
            TryPlayValidHoverSound();
        }

        return base.CanHover(interactable);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        CambiarColor(pad, new Color(1f, 1f, 1f)); //⬜
        base.OnHoverExited(args);
        GameObject insertedObject = args.interactableObject.transform.gameObject;
        if (!insertedObject.CompareTag(idCubeTag))
        {
            hasPlayedInvalidSound = false;
        }
        if (!isEvent)
        {
            materialManager.setDefaultMaterial();
        }
    }

    private void OnObjectEntered(SelectEnterEventArgs args)
    {
        CambiarColor(pad, new Color(1f, 1f, 1f)); //⬜
        if (isPop && isEvent)
        {
            Invoke(nameof(startEndEventFunction), 0.09f);
            return;
        }
        if (isPop)
        {
            Invoke(nameof(pop), 0.09f);
            return;
        }
        if (!isEvent)
        {
            audioManager.PlayCorrectPush();
        }
        materialManager.setAciertoMaterial();
        GameManagerPiles.AddCorrectAction();
        textManager.update();
        Invoke(nameof(nextSteps), 0.09f);
        if (isEvent)
            Invoke(nameof(startPopEventFunction), 0.09f);
    }

    // Suena varias veces
    private void TryPlayInvalidHoverSound()
    {
        if (!hasPlayedInvalidSound)
        {
            if (!isPop)
                audioManager.PlayNoPushHover();
            else
                audioManager.PlayNoPopHover();

            hasPlayedInvalidSound = true;
            materialManager.setErrorMaterial();
            GameManagerPiles.AddWrongAction();
            textManager.update();
        }
    }

    //Suena una sola vez
    private void TryPlayValidHoverSound()
    {
        if (!hasPlayedValidSound)
        {
            if (!isPop)
                audioManager.PlayOkPushHover();
            else
                audioManager.PlayOkPopHover();

            hasPlayedValidSound = true;
            materialManager.setAciertoMaterial();
            GameManagerPiles.AddCorrectAction();
            textManager.update();
        }
    }

    private void nextSteps()
    {
        if (nextSocket != null)
        {
            nextSocket.SetActive(true);
        }
        // Importante que el Grab se desactive primero
        thisCube.GetComponent<XRGrabInteractable>().enabled = false;
        thisCube.GetComponent<Rigidbody>().isKinematic = true;
        materialManager.setAciertoMaterial();
        gameObject.SetActive(false);
    }

    private void startPopEventFunction()
    {
        materialManager.setMidMaterial();
        startEvent.SetActive(true);
    }

    private void startEndEventFunction()
    {
        Debug.Log("👧 Última desapilada correctamente!");
        materialManager.setFinalMaterial();
        GameManagerPiles.AddCorrectAction();
        textManager.update();
        GameManagerPiles.GenerateStatsLog();
        startEvent.SetActive(true);
    }

    private void pop()
    {
        audioManager.PlayCorrectPop();
        GameManagerPiles.AddCorrectAction();
        textManager.update();
        if (nextSocket != null)
        {
            nextSocket.SetActive(true);
        }
        Destroy(thisCube);
        materialManager.setAciertoMaterial();
        gameObject.SetActive(false);
    }

    public void CambiarColor(GameObject objeto, Color nuevoColor)
    {
        Renderer renderer = objeto.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = nuevoColor;
        }
    }

}
