using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputDetector : MonoBehaviour
{
    public static InputDetector Instance;

    public Vector2 GetMousePosition => Input.mousePosition;
    public Vector2 GetMousePositionDelta => _mousePositionDelta;

    [SerializeField] private List<GameInput> gameInputs;

    private Vector2 _lastMousePosition;
    private Vector2 _mousePositionDelta;
    

    private void Awake()
    {
        Instance = this;
    }

    public void AddInputListener(string inputName, UnityAction inputAction, UnityAction noInputAction = null)
    {
        GameInput gameInput = gameInputs.Find(input => input.GetInputName == inputName);
        if (gameInput != null)
        {
            gameInput.OnInput += inputAction;
            if(noInputAction != null)
                gameInput.OnNoInput += noInputAction;
        }
    }

    private void Update()
    {
        CheckAllInputs();
        CountMousePositionDelta();
    }

    private void CheckAllInputs()
    {
        foreach (GameInput gameInput in gameInputs)
        {
            InputType inputType = gameInput.GetInputType;
            
            switch (inputType)
            {
                case InputType.FirstPressFrame:
                    if (Input.GetKeyDown(gameInput.GetKeyCode))
                    {
                        gameInput.CallInputAction();
                    }
                    break;
                case InputType.EveryPressFrame:
                    if (Input.GetKey(gameInput.GetKeyCode))
                    {
                        gameInput.CallInputAction();
                    }
                    break;
                case InputType.EveryFrame:
                    if (Input.GetKey(gameInput.GetKeyCode))
                    {
                        gameInput.CallInputAction();
                    }
                    else
                    {
                        gameInput.CallNoInputAction();
                    }
                    break;
            }
        }
    }

    private void CountMousePositionDelta()
    {
        _mousePositionDelta = GetMousePosition - _lastMousePosition;
        _lastMousePosition = GetMousePosition;
    }
}

[Serializable]
public class GameInput
{
    public event UnityAction OnInput;
    public event UnityAction OnNoInput;
    
    public KeyCode GetKeyCode => keyCode;
    public string GetInputName => inputName;
    public InputType GetInputType => inputType;
    
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private string inputName;
    [SerializeField] private InputType inputType;

    public void CallInputAction()
    {
        OnInput?.Invoke();
    }
    
    public void CallNoInputAction()
    {
        OnNoInput?.Invoke();
    }
}

public enum InputType
{
    FirstPressFrame,
    EveryPressFrame,
    EveryFrame
}
