using UnityEngine;

public class SetCharacterImage : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sr.sprite = AgentInfoManager.Instance.Character.sprite;
    }
}
