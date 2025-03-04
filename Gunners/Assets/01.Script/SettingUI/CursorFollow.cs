using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorFollow : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
