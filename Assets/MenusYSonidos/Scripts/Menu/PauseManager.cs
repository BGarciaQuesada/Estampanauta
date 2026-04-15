using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; // Referencia al menú de pausa
    private bool isPaused = false; // Estado de pausa
    //public static bool InputsBlocked = false; // Variable estática para bloquear inputs en otros scripts

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false); // Asegurarse de que el menú de pausa esté oculto al inicio
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Método para alternar entre pausar y reanudar el juego
    public void TogglePause()
    {
        if(isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Método para pausar el juego
    void PauseGame()
    {
        pauseMenu.SetActive(true); // Mostrar el menú de pausa
        Time.timeScale = 0f; // Detener el tiempo del juego
        AudioListener.pause = true; // Pausar el audio
        isPaused = true; // Actualizar el estado de pausa
        //InputsBlocked = true; // Bloquear inputs en otros scripts
    }

    // Método para reanudar el juego
    void ResumeGame()
    {
        pauseMenu.SetActive(false); // Ocultar el menú de pausa
        Time.timeScale = 1f; // Reanudar el tiempo del juego
        AudioListener.pause = false; // Reanudar el audio
        isPaused = false; // Actualizar el estado de pausa
        //InputsBlocked = false; // Desbloquear inputs en otros scripts
    }

}
