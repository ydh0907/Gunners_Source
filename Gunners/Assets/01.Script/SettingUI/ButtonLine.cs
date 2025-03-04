using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLine : MonoBehaviour
{
    [SerializeField] private List<Transform> buttons;

    [SerializeField] private bool character = false;
    [SerializeField] private bool gun = true;

    private void Start()
    {
        if (character) Line(AgentInfoManager.Instance.CharacterList.characters.IndexOf(AgentInfoManager.Instance.Character));
        if (gun) Line(AgentInfoManager.Instance.GunList.guns.IndexOf(AgentInfoManager.Instance.Gun));
    }

    public void Line(int index)
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }

        buttons[index].gameObject.SetActive(true);
    }
}
