using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PokeButton : MonoBehaviour
{
    public CajaSpawn cajaSpawn;

    [Header("Evaluador Slots")]
    public EvaluadorSlot[] evaluadorSlots;

    private XRBaseInteractable interactable;

    [Header("Audio al completar")]
    public AudioClip audioSiguientePaso;
    private AudioSource audioSource;
    private bool audioReproducido = false;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (evaluadorSlots == null || evaluadorSlots.Length == 0)
        {
            evaluadorSlots = FindObjectsOfType<EvaluadorSlot>();
        }
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnPressed);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnPressed);
    }

    private void OnPressed(SelectEnterEventArgs args)
    {
        if (cajaSpawn != null)
        {
            cajaSpawn.EnviarOrdenAlGameManager();

            if (evaluadorSlots != null)
            {
                foreach (EvaluadorSlot slot in evaluadorSlots)
                {
                    slot.HabilitarSocket();
                }
            }

            if (audioSiguientePaso != null && !audioReproducido)
            {
                audioSource.PlayOneShot(audioSiguientePaso);
                audioReproducido = true;
                Debug.Log("🔊 Audio del siguiente paso reproducido.");
            }

            Debug.Log("🔘 PokeButton presionado: orden enviado al GameManager ✅");
        }
        else
        {
            Debug.LogWarning("⚠️ No se ha asignado el CajaSpawn al PokeButton.");
        }
    }
}
