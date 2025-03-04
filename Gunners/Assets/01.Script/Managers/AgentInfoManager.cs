using System;
using UnityEngine;

public class AgentInfoManager : MonoBehaviour
{
    public static AgentInfoManager Instance;

    public Action<CharacterSO, GunSO> OnValueChange = null;

    private GunSO gun = null;
    private CharacterSO character = null;

    public CharacterListSO CharacterList => GameManager.Instance.CharacterList;
    public GunListSO GunList => GameManager.Instance.GunList;

    public void Init()
    {
        Gun = GunList.guns[0];
        Character = CharacterList.characters[0];
    }

    public CharacterSO Character
    {
        get => Instance.character;
        set
        {
            Instance.character = value;
            OnValueChange?.Invoke(Character, Gun);
        }
    }

    public GunSO Gun
    {
        get => Instance.gun;
        set
        {
            Instance.gun = value;
            OnValueChange?.Invoke(Character, Gun);
        }
    }

    public void ChangeCharacter(int index)
    {
        Character = CharacterList.characters[index];
    }

    public void ChangeGun(int index)
    {
        Gun = GunList.guns[index];
    }
}
