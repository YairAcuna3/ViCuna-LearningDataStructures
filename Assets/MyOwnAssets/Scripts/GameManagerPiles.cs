using System.IO;
using UnityEngine;

public static class GameManagerPiles
{
    public static int correctActions = 0;
    public static int wrongActions = 0;

    public static void AddCorrectAction()
    {
        correctActions++;
        Debug.Log("💚 Acciones correctas: " + correctActions);
    }

    public static void AddWrongAction()
    {
        wrongActions++;
        Debug.Log("🔴 Acciones incorrectas: " + wrongActions);
    }

    public static void GenerateStatsLog()
    {
        GameSession.wrongActions = wrongActions;
        GameSession.correctActions = correctActions;
    }
}
