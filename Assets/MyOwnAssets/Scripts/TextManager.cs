using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public TextMeshPro correctActions;
    public TextMeshPro wrongActions;

    public void update()
    {
        correctActions.text = "" + GameManagerPiles.correctActions;
        wrongActions.text = "" + GameManagerPiles.wrongActions;
    }
}
