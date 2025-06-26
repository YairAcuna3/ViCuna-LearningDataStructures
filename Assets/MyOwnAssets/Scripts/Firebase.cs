using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class FirebaseController : MonoBehaviour
{
    public static FirebaseController instancia;

    DatabaseReference dbReference;
    bool firebaseListo = false;

    // Eventos para notificar el estado de Firebase
    public static event Action OnFirebaseReady;
    public static event Action<string> OnFirebaseError;
    public static event Action<string> OnDataSentSuccess;
    public static event Action<string> OnDataSentError;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InicializarFirebase();
    }

    void InicializarFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                firebaseListo = true;

                Debug.Log("Firebase listo para enviar datos");
                OnFirebaseReady?.Invoke();
            }
            else
            {
                string error = "Error inicializando Firebase: " + task.Result;
                Debug.LogError(error);
                OnFirebaseError?.Invoke(error);
            }
        });
    }

    // Método principal para enviar datos
    public void EnviarDatos(Action<bool> callback = null)
    {
        if (!ValidarEstadoFirebase())
        {
            callback?.Invoke(false);
            return;
        }

        DatosJugador datos = new DatosJugador
        {
            correctActions = GameSession.correctActions,
            wrongActions = GameSession.wrongActions,
            startTime = GameSession.startTime,
            endTime = GameSession.endTime,
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        string json = JsonUtility.ToJson(datos);
        string idUnico = GenerarIdUnico();

        dbReference.Child("jugadores").Child(idUnico).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Datos enviados correctamente a Firebase");
                OnDataSentSuccess?.Invoke("Datos enviados exitosamente");
                callback?.Invoke(true);
            }
            else
            {
                string error = task.Exception?.GetBaseException().Message ?? "Error desconocido";
                Debug.LogError("Error al enviar datos: " + error);
                OnDataSentError?.Invoke(error);
                callback?.Invoke(false);
            }
        });
    }

    // Método sobrecargado para enviar datos con callback
    // public void EnviarDatos(DatosJugador datos, Action<bool> callback = null)
    // {
    //     EnviarDatos(datos.correctActions, datos.wrongActions, datos.startTime, datos.endTime, callback);
    // }

    // Método para verificar si Firebase está listo
    public bool EstaListo()
    {
        return firebaseListo && dbReference != null;
    }

    // Método para reintentar inicialización si falló
    public void ReintentarInicializacion()
    {
        if (!firebaseListo)
        {
            InicializarFirebase();
        }
    }

    private bool ValidarEstadoFirebase()
    {
        if (!firebaseListo || dbReference == null)
        {
            Debug.LogWarning("Firebase no está listo, no se puede enviar datos.");
            OnDataSentError?.Invoke("Firebase no está inicializado");
            return false;
        }
        return true;
    }

    private string GenerarIdUnico()
    {
        // Combinamos timestamp con un número aleatorio para mayor unicidad
        return DateTime.Now.Ticks.ToString() + "_" + UnityEngine.Random.Range(1000, 9999);
    }

    [System.Serializable]
    public class DatosJugador
    {
        public int correctActions;
        public int wrongActions;
        public string startTime;
        public string endTime;
        public string timestamp; // Agregamos timestamp para saber cuándo se guardó
    }
}