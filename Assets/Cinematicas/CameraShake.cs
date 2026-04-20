using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration = 1f;
    public float magnitude = 0.2f;

    private Vector3 originalPos;
    private float currentTime = 0f;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void ShakeFromTimeline()
    {
        Shake(0.5f, 0.2f); // valores por defecto desde Timeline
    }

    public void Shake(float time, float strength)
    {
        duration = time;
        magnitude = strength;
        currentTime = duration;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            float progress = currentTime / duration;

            float x = Random.Range(-1f, 1f) * magnitude * progress;
            float y = Random.Range(-1f, 1f) * magnitude * progress;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            currentTime -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }
}