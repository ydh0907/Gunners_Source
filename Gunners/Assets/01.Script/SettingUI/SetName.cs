using System.Collections;
using TMPro;
using UnityEngine;

public class SetName : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitUntil(() => EnemyDummy.Instance != null);

        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = text.text + EnemyDummy.Instance.nickname;
    }
}
