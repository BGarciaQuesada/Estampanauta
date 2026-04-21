using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// Esta clase maneja el comportamiento de la zona de la nave, la cual recibe consumibles

public class ShipZone : MonoBehaviour, IItemReceiver
{
    // [!] La nave necesita conocer la lista de objetivos para aceptar o no los items, y para actualizar el progreso de los objetivos
    [SerializeField] private List<Objective> objectives;
    public FloatingText feedbackText; // para "¡Lleno!"

    [SerializeField] private ObjectivesUIManager uiManager;

    private void Start()
    {
        GenerateObjectives();
        UpdateUI(); //para mostrar los objetivos pendientes al inicio 
    }

    public bool Receive(IItem item, GameObject user)
    {
        Debug.Log("RECIBEEEEEE");
        // Lo del as: básicamente comprueba de forma segura si el item recibido es del tipo de la variable.
        // Si lo es, se le asigna a la variable
        // Si no, se le asigna null (manejado por el if)
        Bucket bucket = item as Bucket;
        if (bucket != null && bucket.IsFull)
        {
            if (TryAdd("Fuel"))
            {
                bucket.Empty();
                Debug.Log("Cubo vaciado");
                CheckVictory();
                return true;
            }
        }

        Consumable consumable = item as Consumable;
        if (consumable != null)
        {
            Debug.Log("Consumible aplicado a la nave");

            string id = consumable.GetObjectiveID();

            if (TryAdd(id))
            {
                CheckVictory();
                return true;
            }

        }

        // Si no se reconoce el item (aka no es un consumible), no se acepta
        return false;
    }

    // --- LÓGICA OBJETIVOS ---
    // [!] Lo del success se había borrado porque originalmente se iba a hacer con mensajes y,
    // [!] a pesar de ser borrado, regresó??? Asumo que ha sido lio de ramas de git. Arreglado.
    bool TryAdd(string id)
    {
        Objective obj = objectives.Find(o => o.id == id);

        if (obj == null)
        {
            // Feedback interno
            Debug.LogWarning($"No existe objetivo para {id}");
            return false;
        }

        if (!obj.CanReceive)
        {
            // Feedback externo (sale mensajito)
            ShowFullMessage();
            return false;
        }

        obj.Add();

        Debug.Log($"{id}: {obj.current}/{obj.required}");

        UpdateUI();

        return true;
    }

    private void CheckVictory()
    {
        bool allComplete = objectives.TrueForAll(o => o.IsComplete);
        if (allComplete)
        {
            Debug.Log("Todos los objetivos completados. Cargando escena de victoria...");
            SceneManager.LoadScene("CinemVictoria"); // Asegúrate de que el nombre coincide con tu escena de victoria
        }
    }

    // --- UI ---
    private void GenerateObjectives()
    {
        int fuel = Random.Range(3, 8); // 3–7
        int crystal = 10 - fuel;

        objectives = new List<Objective>
    {
        new Objective { id = "Fuel", required = fuel, current = 0 },
        new Objective { id = "Crystal", required = crystal, current = 0 },
        new Objective { id = "Scrap", required = 2, current = 0 } // [!] Valor temporal, cambiar de ser necesario
    };
    }

    void ShowFullMessage()
    {
        if (feedbackText != null)
        {
            feedbackText.Show("¡Lleno!");
        }
    }

    void UpdateUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateAll(objectives);
        }
    }

}
