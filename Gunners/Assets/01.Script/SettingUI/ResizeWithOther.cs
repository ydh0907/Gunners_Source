using UnityEngine;

public class ResizeWithOther : MonoBehaviour
{
    [SerializeField] private RectTransform other;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (rect.rect.size != other.rect.size)
        {
            rect.sizeDelta = other.sizeDelta;
        }
    }
}
