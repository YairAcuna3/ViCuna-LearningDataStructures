using UnityEngine;

public class StartStartEvent : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioWelcome;
    public AudioClip[] randomPileExampleClips;
    public AudioClip audioExplanation;
    public GameObject showWindow;
    public MaterialManager avatar;
    void Start()
    {
        StartCoroutine(PlayAudioSequence());
    }

    private System.Collections.IEnumerator PlayAudioSequence()
    {
        // ▶️ Audio de saludo
        audioSource.clip = audioWelcome;
        audioSource.Play();
        avatar.setMidMaterial();
        yield return new WaitForSeconds(audioWelcome.length);
        avatar.setDefaultMaterial();
        yield return new WaitForSeconds(0.25f);

        // ▶️ Audio aleatorio para dar un ejemplo en la vida real de qué es una pila
        AudioClip randomClip = randomPileExampleClips[Random.Range(0, randomPileExampleClips.Length)];
        audioSource.clip = randomClip;
        audioSource.Play();
        avatar.setMidMaterial();
        yield return new WaitForSeconds(randomClip.length);
        avatar.setDefaultMaterial();
        yield return new WaitForSeconds(0.25f);

        // ▶️ Explicación de lo que es una pila y de las mecánicas
        audioSource.clip = audioExplanation;
        audioSource.Play();
        avatar.setMidMaterial();
        yield return new WaitForSeconds(audioExplanation.length);
        avatar.setDefaultMaterial();

        // ✅ Lógica que se ejecuta después
        Destroy(showWindow);
    }
}
