using UnityEngine;

public class HPBar : MonoBehaviour
{
    private ICharacter character;

    private void Awake()
    {
        character = transform.parent.parent.GetComponent<ICharacter>();
    }

    private void Start()
    {
        character.onHit += SetAmount;
    }

    private void OnDestroy()
    {
        character.onHit -= SetAmount;
    }

    private void SetAmount(int hp)
    {
        transform.localScale = new Vector3(Mathf.Clamp((float)hp / character.maxHp, 0, 1), 1, 1);
    }
}
