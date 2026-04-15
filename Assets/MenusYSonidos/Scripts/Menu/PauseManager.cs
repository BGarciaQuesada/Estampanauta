using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;   // Objeto del menú
    [SerializeField] private CanvasGroup canvasGroup; // Para controlar el fade
    [SerializeField] private float fadeDuration = 0.3f; // Duración del fade

    private bool isPaused = false;          // Estado del juego
    private bool isTransitioning = false;   // Evita spam mientras anima
    private Coroutine fadeCoroutine;        // Referencia a la animación actual

    void Start()
    {
        // IMPORTANTE: el objeto debe estar activo para poder hacer fade
        pauseMenu.SetActive(true);

        // Estado inicial: invisible y sin interacción
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        // Solo permite pulsar ESC si NO está en transición
        if (Input.GetKeyDown(KeyCode.Escape) && !isTransitioning)
        {
            TogglePause();
        }
    }

    // Cambia entre pausa y juego
    public void TogglePause()
    {
        // Seguridad extra
        if (isTransitioning) return;

        // Si hay una animación en curso, la paramos
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        // Elegimos qué animación lanzar
        if (isPaused)
            fadeCoroutine = StartCoroutine(FadeOut());
        else
            fadeCoroutine = StartCoroutine(FadeIn());
    }

    // Animación de entrada (pausar)
    IEnumerator FadeIn()
    {
        isTransitioning = true; // Bloquea input
        isPaused = true;

        // Pausamos el juego
        Time.timeScale = 0f;
        AudioListener.pause = true;

        // Activamos interacción del menú
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float time = 0f;

        // Fade de 0 --> 1
        while (time < fadeDuration)
        {
            // Usamos unscaledDeltaTime porque el juego está pausado
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f; // Asegura valor final
        isTransitioning = false; // Permite input otra vez
    }

    // Animación de salida (reanudar)
    IEnumerator FadeOut()
    {
        isTransitioning = true;

        // Quitamos interacción mientras desaparece
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float time = 0f;

        // Fade de 1 --> 0
        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;

        // Reanudamos el juego
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;

        isTransitioning = false; // Permite input otra vez
    }
}