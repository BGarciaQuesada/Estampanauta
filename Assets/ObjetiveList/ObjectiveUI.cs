using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private string id;

    [SerializeField] private GameObject checkmark; // ✔
    [SerializeField] private TextMeshProUGUI counterText; // 0/X

    public void UpdateUI(Objective obj)
    {
        if (obj.required == 1)
        {
            if (checkmark != null)
                checkmark.SetActive(obj.IsComplete);

            if (counterText != null)
                counterText.gameObject.SetActive(false);
        }
        else
        {
            if (checkmark != null)
                checkmark.SetActive(false);

            if (counterText != null)
            {
                counterText.gameObject.SetActive(true);
                counterText.text = $"{obj.current}/{obj.required}";
            }
        }
    }

    public string GetID()
    {
        return id;
    }
}