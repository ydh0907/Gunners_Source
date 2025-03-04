using UnityEngine;

public class SetGunImage : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sr.sprite = AgentInfoManager.Instance.Gun.sprite;
    }
}
