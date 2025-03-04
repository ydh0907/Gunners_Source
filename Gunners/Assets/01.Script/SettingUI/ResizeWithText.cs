using TMPro;
using UnityEngine;

public class ResizeWithText : MonoBehaviour
{
    [SerializeField] private bool isNickname = true;

    private TextMeshProUGUI text;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        if(isNickname) text.text = GameManager.Instance.nickname;
    }

    private void Update()
    {
        rect.sizeDelta = new Vector2(text.GetRenderedValues(false).x + 50f, rect.sizeDelta.y);
    }
}
