using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "SO/CharacterListSO")]
public class CharacterListSO : ScriptableObject
{
    public List<CharacterSO> characters;
}
