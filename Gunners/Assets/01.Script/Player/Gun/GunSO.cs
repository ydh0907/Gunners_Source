using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(menuName = "SO/GunSO")]
public class GunSO : ScriptableObject
{
    public IGun gun;
    public Sprite sprite;
}
