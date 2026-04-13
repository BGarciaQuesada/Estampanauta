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
            checkmark.SetActive(obj.IsComplete);
            counterText.gameObject.SetActive(false);
        }
        else
        {
            checkmark.SetActive(false);
            counterText.gameObject.SetActive(true);
            counterText.text = $"{obj.current}/{obj.required}";
        }
    }

    public string GetID()
    {
        return id;
    }
}