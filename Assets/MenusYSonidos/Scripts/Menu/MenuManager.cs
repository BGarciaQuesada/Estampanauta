using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles del Menu")]
    [SerializeField] private GameObject panelOpciones; // Panel de opciones
    [SerializeField] private GameObject panelMenuPrincipal; // Panel del menu principal
    [SerializeField] private GameObject panelControles; // Panel de controles

    [Header("Audio")]
    [SerializeField] private AudioSource musicaMenu; // Fuente de audio para reproducir sonidos del menú
    [SerializeField] private AudioSource musicaFondo; // Fuente de audio para reproducir la música de fondo del juego
    [SerializeField] private float duracionFade = 2f; // Duración del fade out de la música de fondo al cargar el nivel

    [Header("Volumen")]
    [SerializeField] private Slider sliderVolumen; // Slider para controlar el volumen de la música del menú
    [SerializeField] private float volumenActual = 1f; // Variable para almacenar el volumen actual de la música del menú

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //int SceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; // Obtener el índice de la escena actual
        CinemInicio();
        CinemVictoria(); // Llamar al método para manejar la cinemática de victoria si estamos en esa escena

                         // Si estamos en el menu de inicio
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (panelMenuPrincipal != null && panelOpciones != null)
            {
                panelMenuPrincipal.SetActive(true); // Asegurarse de que el panel del menú principal esté activo al iniciar
                panelOpciones.SetActive(false); // Asegurarse de que el panel de opciones esté desactivado al iniciar
                panelControles.SetActive(false);
            }

            if(musicaMenu != null)
            {
                musicaMenu.volume = volumenActual;
                musicaMenu.loop = true; // Configurar la música del menú para que se reproduzca en bucle
                musicaMenu.Play();
            }
        }

        if (sliderVolumen != null) // Si hay un slider de volumen asignado
        {
            volumenActual = PlayerPrefs.GetFloat("VolumenMusica", 1f); // Cargar el volumen guardado en las preferencias del jugador
            sliderVolumen.value = volumenActual; // Establecer el valor del slider al volumen actual
            ActualizarVolumen(volumenActual); // Actualizar el volumen de la musica
            sliderVolumen.onValueChanged.AddListener(ActualizarVolumen); // Agregar un listener para actualizar el volumen cuando el slider cambie
        }



    }

    // Metodo para actualizar el volumen de la música en cualquier escena, se llama desde el slider de volumen en el panel de opciones
    public void ActualizarVolumen(float valor)
    {
        volumenActual = valor; // Actualizar la variable de volumen actual con el nuevo valor del slider
        AudioListener.volume = volumenActual; // Establecer el volumen global del audio en el valor actual
        PlayerPrefs.SetFloat("VolumenMusica", volumenActual); // Guardar el nuevo volumen en las preferencias del jugador
    }

    // Método para abrir el panel de opciones desde el menú principal
    public void AbrirOpciones()
    {
        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(false); // Desactivar el panel del menú principal
        }
        panelOpciones.SetActive(true); // Activar el panel de opciones
    }

    public void AbrirControles()
    {
        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(false); // Desactivar el panel del menú principal
        }
        panelControles.SetActive(true); // Activar el panel de controles
    }

    // Método para volver al menú principal desde el panel de opciones
    public void VolverAlMenu()
    {
        panelMenuPrincipal.SetActive(true); // Activar el panel del menú principal
        panelOpciones.SetActive(false); // Desactivar el panel de opciones
        panelControles.SetActive(false);
    }

    public void Load()
    {
        StartCoroutine(FadeOutYLoad()); // Iniciar la corrutina para hacer el fade out y cargar la escena del juego
    }

    private IEnumerator FadeOutYLoad()
    {
        if (musicaMenu != null) // Si hay musica de fondo asignada
        {
            float volumenInicial = musicaMenu.volume; // Guardar el volumen inicial de la musica

            for (float t = 0; t < duracionFade; t += Time.deltaTime) // Hacer un bucle durante la duracion del fade
            {
                musicaMenu.volume = Mathf.Lerp(volumenInicial, 0, t / duracionFade); // Interpolar el volumen de la musica
                yield return null;
            }

            musicaMenu.volume = 0; // Asegurarse de que el volumen sea 0 al final del fade
            musicaMenu.Stop(); // Detener la musica de fondo
        }

        SceneManager.LoadScene("CinematicaInicio"); // Cargar la escena del nivel
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");

        // Directiva de preprocesador
        #if UNITY_EDITOR
                // Si estamos en el editor de Unity, usamos el comando para detener el juego.
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                                        // Si estamos en un ejecutable (Build), cerramos la aplicación.
                                        Application.Quit();
        #endif
    }

    // Metodo para volver al menu principal independientemente de la escena
    public void VolverAlMenuPrincipal()
    {
        SceneManager.LoadScene("Menu");
    }

    public void CinemVictoria()
    {
        if(SceneManager.GetActiveScene().name == "CinemVictoria")
        {
            StartCoroutine(IrAMenuPrincipal()); // Esperar 5 segundos antes de cargar el menú principal
        }
    }

    IEnumerator IrAMenuPrincipal()
    {
        yield return new WaitForSeconds(13.5f); // Esperar el tiempo especificado antes de continuar con la ejecución del código siguiente
        VolverAlMenuPrincipal(); // Llamar al método para volver al menú principal después de la espera
    }

    public void CinemInicio()
    {
        if (SceneManager.GetActiveScene().name == "CinematicaInicio")
        {
            StartCoroutine(EmpezarJuego()); // Esperar 5 segundos antes de cargar el nivel
        }
    }

    IEnumerator EmpezarJuego()
    {
        yield return new WaitForSeconds(24f); // Esperar el tiempo especificado antes de continuar con la ejecución del código siguiente
        SceneManager.LoadScene("Juego");
    }

}
