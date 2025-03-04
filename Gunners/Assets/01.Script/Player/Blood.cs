using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Blood : MonoBehaviour
{
    private Image image;
    private float blood = 0;
    private int past;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitUntil(() => Agent.Instance != null);

        Agent.Instance.character.onHit += SetBlood;
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if(Agent.Instance != null)
        {
            Agent.Instance.character.onHit -= SetBlood;
        }
    }

    private void Update()
    {
        if(Agent.Instance != null)
        {
            blood = Mathf.Clamp(blood -= Time.deltaTime, 0, 1);
            image.color = new Color(1, 1, 1, blood);
        }
    }

    private void SetBlood(int hp)
    {
        blood += (1 - (float)hp / Agent.Instance.character.maxHp) * 0.5f + (1 - (float)hp / past) * 0.5f;
        past = hp;
    }
}
