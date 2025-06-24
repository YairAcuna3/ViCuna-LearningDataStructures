using System;
using System.IO;
using UnityEngine;

public static class GameSession
{
    // *Intrumento: Precisión de las tareas
    public static int correctActions;
    public static int wrongActions;
    public static int queueCorrectActions;
    public static int queueWrongActions;

    // *Instrumento: Tiempo dedicado a tareas
    public static string startTime;
    public static string endTime;
    public static string queueStartTime;
    public static string queueEndTime;

    // *Instrumento: Tasa de finalización de tareas
    // el número de acciones correctas se divide entre 12 (total de tareas porque son 2 tareas por cubo y 6 cubos) en el escenario de pilas

    public static void AddQueueCorrectAction()
    {
        queueCorrectActions++;
        Debug.Log("💚 Acciones correctas: " + queueCorrectActions);
    }

    public static void AddQueueWrongAction()
    {
        queueWrongActions++;
        Debug.Log("🔴 Acciones incorrectas: " + queueWrongActions);
    }

    public static void QueueStartTimer()
    {
        queueStartTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("🕒 Inicio de la cola: " + queueStartTime);
    }

    public static void QueueEndTimer()
    {
        queueEndTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("🕛 Fin de la cola: " + queueEndTime);
    }

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

    //* Tasa de finalización de tareas: acciones correctas / 12 (total de tareas)
    public static string GetCompletionRate()
    {
        if (correctActions == 0)
            return "0";

        float completionRate = (float)correctActions / 12;
        return $"{completionRate}";
    }

    //* Precisión de las tareas: acciones correctas / (acciones correctas + acciones incorrectas)
    public static string GetPrecisionRate()
    {
        if (correctActions + queueCorrectActions + wrongActions + queueWrongActions == 0)
            return "0";

        float precisionRate = (float)(correctActions + queueCorrectActions) / (correctActions + queueCorrectActions + wrongActions + queueWrongActions);
        return $"{precisionRate}";
    }

    public static void GenerateStatsLog()
    {
        string logContent = $"===== ESTADISTICAS DE LA PARTIDA =====\n" +
                            $"Pilas AC: {correctActions}\n" +
                            $"Colas AC: {queueCorrectActions}\n" +
                            $"Pilas AI: {wrongActions}\n" +
                            $"Colas AI: {queueWrongActions}\n" +
                            $"Acciones correctas: {correctActions + queueCorrectActions}\n" +
                            $"Acciones incorrectas: {wrongActions + queueWrongActions}\n" +
                            $"Pilas Start: {startTime}\n" +
                            $"Pilas Queue Start: {queueStartTime}\n" +
                            $"Pilas End: {endTime}\n" +
                            $"Pilas Queue End: {queueEndTime}\n" +
                            $"Precision de las tareas: {GetPrecisionRate()}\n" +
                            $"Tasa de finalizacion: {GetCompletionRate()}\n" +
                            $"Tiempo pilas: {GetElapsedTime(startTime, endTime)}\n" +
                            $"Tiempo colas: {GetElapsedTime(queueStartTime, queueEndTime)}";


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
