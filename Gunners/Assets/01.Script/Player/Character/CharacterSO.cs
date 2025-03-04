using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "SO/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public ICharacter character;
    public Sprite sprite;
}
