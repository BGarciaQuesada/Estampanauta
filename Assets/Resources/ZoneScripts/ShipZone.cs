using System.Collections.Generic;
using UnityEngine;

// Esta clase maneja el comportamiento de la zona de la nave, la cual recibe consumibles

public class ShipZone : MonoBehaviour, IItemReceiver
{
    // [!] La nave necesita conocer la lista de objetivos para aceptar o no los items, y para actualizar el progreso de los objetivos
    [SerializeField] private List<Objective> objectives;
    public FloatingText feedbackText; // para "¡Lleno!"

    [SerializeField] private ObjectivesUIManager uiManager;

    public bool Receive(IItem item, GameObject user)
    {
        // Lo del as: básicamente comprueba de forma segura si el item recibido es del tipo de la variable.
        // Si lo es, se le asigna a la variable
        // Si no, se le asigna null (manejado por el if)
        Bucket bucket = item as Bucket;
        if (bucket != null && bucket.IsFull)
        {
            bucket.Empty();
            Debug.Log("Cubo vaciado");
            return true;
        }

        Consumable consumable = item as Consumable;
        if (consumable != null)
        {
            Debug.Log("Consumible aplicado a la nave");
            return true; // Se destruye dentro de UseOn
        }

        // Si no se reconoce el item (aka no es un consumible), no se acepta
        return false;
    }

    // --- LÓGICA OBJETIVOS ---
    bool TryAdd(string id, System.Action onSuccess = null)
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

        onSuccess?.Invoke();

        UpdateUI();

        return true;
    }

    // --- UI ---
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
