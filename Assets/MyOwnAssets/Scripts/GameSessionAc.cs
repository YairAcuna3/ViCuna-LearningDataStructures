using System;
using System.IO;
using UnityEngine;

public static class GameSession
{
    // *Intrumento: Precisión de las tareas
    public static int correctActions;
    public static int wrongActions;

    // *Instrumento: Tiempo dedicado a tareas
    public static string startTime;
    public static string endTime;

    // *Instrumento: Tasa de finalización de tareas
    // el número de acciones correctas se divide entre 12 (total de tareas porque son 2 tareas por cubo y 6 cubos) en el escenario de pilas

    public static void StartTimer()
    {
        startTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("🕒 Inicio del juego: " + startTime);
    }

    public static void EndTimer()
    {
        endTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("🕛 Fin del juego: " + endTime);
    }

    public static string GetElapsedTime(string startTime, string endTime)
    {
        DateTime start;
        DateTime end;

        // Intentar parsear las fechas
        if (DateTime.TryParseExact(startTime, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out start) &&
            DateTime.TryParseExact(endTime, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out end))
        {
            TimeSpan duration = end - start;

            // Retornar en formato HH:mm:ss
            return string.Format("{0:D2}:{1:D2}:{2:D2}", (int)duration.TotalHours, duration.Minutes, duration.Seconds);
        }
        else
        {
            Debug.LogError("❌ Error al parsear las fechas. Verifica el formato.");
            return "00:00:00";
        }
    }

    public static string GetCompletionRate()
    {
        // Tasa de finalización de tareas: acciones correctas / 12 (total de tareas)
        if (correctActions == 0)
            return "0";

        float completionRate = (float)correctActions / 12;
        return $"{completionRate}";
    }

    public static string GetPrecisionRate()
    {
        // Precisión de las tareas: acciones correctas / (acciones correctas + acciones incorrectas)
        if (correctActions + wrongActions == 0)
            return "0";

        float precisionRate = (float)correctActions / (correctActions + wrongActions);
        return $"{precisionRate}";
    }

    public static void GenerateStatsLog()
    {
        string logContent = $"===== ESTADISTICAS DE LA PARTIDA =====\n" +
                            $"Acciones correctas: {correctActions}\n" +
                            $"Acciones incorrectas: {wrongActions}\n" +
                            $"Inicio del juego: {startTime}\n" +
                            $"Fin del juego: {endTime}\n" +
                            $"Precision de las tareas: {GetPrecisionRate()}\n" +
                            $"Tasa de finalizacion: {GetCompletionRate()}\n" +
                            $"Tiempo transcurrido: {GetElapsedTime(startTime, endTime)}";


        // Ruta de descargas según el sistema operativo
        string downloadsPath = "";
#if UNITY_STANDALONE_WIN
        downloadsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "\\Downloads";
#elif UNITY_STANDALONE_OSX
        downloadsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/Downloads";
#elif UNITY_ANDROID
        // En Android Application.persistentDataPath o pedir permisos
        downloadsPath = Application.persistentDataPath;
#else
        downloadsPath = Application.persistentDataPath;
#endif
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"GameTimeStats_{timestamp}.txt";
        string filePath = Path.Combine(downloadsPath, fileName);

        try
        {
            File.WriteAllText(filePath, logContent);
            Debug.Log("📄 Archivo guardado en: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ Error al guardar el archivo: " + e.Message);
        }
    }
}
