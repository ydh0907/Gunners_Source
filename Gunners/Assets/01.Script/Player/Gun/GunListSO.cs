using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "SO/GunListSO")]
public class GunListSO : ScriptableObject
{
    public List<GunSO> guns;
}
