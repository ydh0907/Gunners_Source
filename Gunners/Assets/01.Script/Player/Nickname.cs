using TMPro;
using UnityEngine;

public class Nickname : MonoBehaviour
{
    private TextMeshPro text;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();

        if (transform.parent.TryGetComponent(out Agent agent))
            text.text = GameManager.Instance.nickname;
        else if (transform.parent.TryGetComponent(out EnemyDummy enemy))
            text.text = enemy.nickname;
    }
}
