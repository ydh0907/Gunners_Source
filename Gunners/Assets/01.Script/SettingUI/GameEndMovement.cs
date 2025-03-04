using Cinemachine;
using UnityEngine;

public class GameEndMovement : MonoBehaviour
{
    private CinemachineVirtualCamera cam;

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        GameManager.Instance.onGameWin += Move;
        GameManager.Instance.onGameLose += Move;
    }

    private void OnDisable()
    {
        GameManager.Instance.onGameWin -= Move;
        GameManager.Instance.onGameLose -= Move;
    }

    private void Move()
    {
        transform.position = Agent.Instance.transform.position + new Vector3(0, 0, -10);
        cam.m_Follow = Agent.Instance.transform;
        cam.m_Lens.OrthographicSize = 3;
    }
}
