using System.Collections.Generic;
using UnityEngine;

// He creado esta clase para manejar exclusivamente el UI de los objetivos.
// Si luego hubiese un UI Manager general, podríamos delegar esta parte a esta clase y así mantener el código más organizado y modularizado.

public class ObjectivesUIManager : MonoBehaviour
{
    [SerializeField] private List<ObjectiveUI> objectiveUIs;

    public void UpdateAll(List<Objective> objectives)
    {
        foreach (var ui in objectiveUIs)
        {
            Objective obj = objectives.Find(o => o.id == ui.GetID());

            if (obj != null)
            {
                ui.UpdateUI(obj);
            }
        }
    }
}