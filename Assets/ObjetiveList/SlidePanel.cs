using System.Collections;
using UnityEngine;

public class SlidePanel : MonoBehaviour
{
    [SerializeField] private RectTransform panel;

    [SerializeField] private Vector2 shownPosition;
    [SerializeField] private Vector2 hiddenPosition; // posición fuera de pantalla

    [SerializeField] private float duration = 0.3f;

    private bool isShown = true;
    private Coroutine currentAnim;

    private bool isAnimating = false; // Para evitar que se solapen animaciones

    public void Toggle()
    {
        if (isAnimating) return; // Ya estamos animando, no hacer nada

        isShown = !isShown;

        if (currentAnim != null)
            StopCoroutine(currentAnim);

        currentAnim = StartCoroutine(AnimSlide(isShown ? shownPosition : hiddenPosition));
    }

    IEnumerator AnimSlide(Vector2 target)
    {
        isAnimating = true; // Informar que estamos animando

        Vector2 start = panel.anchoredPosition;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            t = t * t * (3f - 2f * t); // Suavizado (ease in-out)

            panel.anchoredPosition = Vector2.Lerp(start, target, t);

            time += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = target;
        isAnimating = false; // Animación terminada, permitir nuevas animaciones
    }
}