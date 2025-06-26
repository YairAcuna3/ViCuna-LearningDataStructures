using UnityEngine;
using UnityEngine.Rendering;

public class AudioManagerPiles : MonoBehaviour
{
    public AudioSource[] okPushHover;
    public AudioSource[] noPushHover;
    public AudioSource[] correctPush;
    public AudioSource[] okPopHover;
    public AudioSource[] noPopHover;
    public AudioSource[] correctPop;

    public void Start()
    {
        GameSession.resetStats();
        GameSession.StartTimer();
    }
    public void PlayOkPushHover()
    {
        int index = Random.Range(0, okPushHover.Length);
        okPushHover[index].Play();
    }

    public void PlayNoPushHover()
    {
        int index = Random.Range(0, noPushHover.Length);
        noPushHover[index].Play();
    }

    public void PlayCorrectPush()
    {
        int index = Random.Range(0, correctPush.Length);
        correctPush[index].Play();
    }

    public void PlayOkPopHover()
    {
        int index = Random.Range(0, okPopHover.Length);
        okPopHover[index].Play();
    }

    public void PlayNoPopHover()
    {
        int index = Random.Range(0, noPopHover.Length);
        noPopHover[index].Play();
    }

    public void PlayCorrectPop()
    {
        int index = Random.Range(0, correctPop.Length);
        correctPop[index].Play();
    }
}
