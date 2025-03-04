using UnityEngine;

public class NetButtonManager : MonoBehaviour
{
    [SerializeField] private Transform loading;
    [SerializeField] private AudioClip buttonSound;

    public void Loading()
    {
        loading.gameObject.SetActive(true);
    }

    public void Matching()
    {
        GameManager.Instance.StartMatching();
    }

    public void LoadScene(int num)
    {
        LoadSceneManager.Instance.LoadSceneAsync(num, null);
    }

    public void PlayButtonSound()
    {
        AudioManager.Instance.Play(buttonSound);
    }

    public void Lose()
    {
        ++IOManager.Instance.Lose;
    }
}
