using UnityEngine;

public class Hide : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.onGameWin += Hiding;
        GameManager.Instance.onGameLose += Hiding;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameWin -= Hiding;
        GameManager.Instance.onGameLose -= Hiding;
    }

    private void Hiding()
    {
        gameObject.SetActive(false);
    }
}
