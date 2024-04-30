using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInput : MonoBehaviour
{
    [SerializeField] // 标记为可序列化，以便在 Inspector 中看到
    private InputAction mouseClickAction; // 引用你的鼠标点击输入动作
    public static event Action OnMouseDown;
    private void OnEnable()
    {
        mouseClickAction.Enable();
    }
    private void OnDisable()
    {
        mouseClickAction.Disable();
    }
    void Update()
    {
        if (mouseClickAction.triggered)
        {
            OnMouseDown?.Invoke();
        }
    }
}
