using System;
using UnityEngine;

public class MouseInput : MonoBehaviour
{

    public static event Action OnMouseDown;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键点击
        {
            OnMouseDown?.Invoke();
        }
    }
}
