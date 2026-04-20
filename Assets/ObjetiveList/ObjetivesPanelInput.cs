using UnityEngine;

// Este script se encarga de detectar la pulsación de la tecla Tab y llamar al método Toggle del SlidePanel para mostrar u ocultar el panel de objetivos
// (Lo he creado aparte por hacerlo menos lioso, pero que se puede añadir a otros scripts de controles)
public class ObjectivesPanelInput : MonoBehaviour
{
    [SerializeField] private SlidePanel slidePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            slidePanel.Toggle();
        }
    }
}