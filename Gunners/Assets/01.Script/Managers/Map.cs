using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private List<Transform> Packs;

    public static Vector2 Host;
    public static Vector2 Enterer;

    private Transform pack;

    private void Awake()
    {
        Packs.ForEach((b) => b.gameObject.SetActive(false));

        pack = Packs[GameManager.Instance.map];
        pack.gameObject.SetActive(true);

        Host = pack.Find("Host").position;
        Enterer = pack.Find("Enterer").position;
    }
}
