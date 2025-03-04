using UnityEngine;

public class Winner : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.onGameWin += TurnOn;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameWin -= TurnOn;
    }

    private void TurnOn()
    {
        gameObject.SetActive(true);
    }
}
