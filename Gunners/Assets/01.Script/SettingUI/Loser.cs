using UnityEngine;

public class Loser : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.onGameLose += TurnOn;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameLose -= TurnOn;
    }

    private void TurnOn()
    {
        gameObject.SetActive(true);
    }
}
