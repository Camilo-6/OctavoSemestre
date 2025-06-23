/*using TMPro;
using UnityEngine;

public class ContadorRecursosUI : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    private SistemaRecurso resourceSystem;

    private void Start()
    {
        resourceSystem = Object.FindFirstObjectByType<SistemaRecurso>();
        if (resourceSystem != null)
        {
            UpdateCounter(resourceSystem.GetCurrentResources());
            SistemaRecurso.OnResourcesUpdated += UpdateCounter;
        }
    }

    private void OnDestroy()
    {
        if (resourceSystem != null)
        {
            SistemaRecurso.OnResourcesUpdated -= UpdateCounter;
        }
    }

    private void UpdateCounter(int count)
    {
        if (counterText != null)
        {
            counterText.text = $"Recursos: {count}";
            UnityEngine.Debug.Log($"UI Updated to: {count}");
        }
    }
}*/