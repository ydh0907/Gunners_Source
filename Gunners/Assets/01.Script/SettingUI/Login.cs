using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private Button loginButton;

    [SerializeField] private TMP_InputField ipInput;

    private void Awake()
    {
        loginButton = GetComponent<Button>();
        loginButton.onClick.AddListener(HandleLogin);
    }

    private void HandleLogin()
    {
        NetworkManager.Instance.Connect(ipInput.text);
    }
}
