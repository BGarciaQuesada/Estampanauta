using System.Collections.Generic;
using UnityEngine;

// He creado esta clase para manejar exclusivamente el UI de los objetivos.
// Si luego hubiese un UI Manager general, podríamos delegar esta parte a esta clase y así mantener el código más organizado y modularizado.
// SE ASIGNA AL PANEL QUE CONTIENE TODOS LOS OBJETIVOS

public class ObjectivesUIManager : MonoBehaviour
{
    [SerializeField] private List<ObjectiveUI> objectiveUis;

    public void UpdateAll(List<Objective> objectives)
    {
        foreach (var ui in objectiveUis)
        {
            Objective obj = objectives.Find(o => o.id == ui.GetID());

            if (obj != null)
            {
                ui.UpdateUI(obj);
            }
        }
    }
}