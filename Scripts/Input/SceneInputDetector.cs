using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneInputDetector : MonoBehaviour
{
    public static SceneInputDetector Instance;

    [SerializeField] private List<SceneInput> sceneInputs;
    [SerializeField] private Camera raycastCamera;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ConnectWithInputDetector();
    }

    private void ConnectWithInputDetector()
    {
        foreach (SceneInput sceneInput in sceneInputs)
        {
            InputDetector.Instance.AddInputListener(sceneInput.GetGameInputName, () => sceneInput.CallObjectReceived());
        }
    }

    public void AddSceneInputListener<T>(string gameInputName, UnityAction<T> listenerAction)
    {
        if (!TryGetSceneInput(gameInputName, out SceneInput sceneInput)) return;
        
        sceneInput.OnObjectReceived += () =>
        {
            GameObject raycastObject = GetRaycastObject(sceneInput, out Vector3 _);
            if (raycastObject != null && raycastObject.TryGetComponent(out T component))
            {
                listenerAction?.Invoke(component);
            }
        };
    }
    
    public void AddSceneInputListener<T>(string gameInputName, UnityAction<T, Vector3> listenerAction)
    {
        if (!TryGetSceneInput(gameInputName, out SceneInput sceneInput)) return;
        
        sceneInput.OnObjectReceived += () =>
        {
            GameObject raycastObject = GetRaycastObject(sceneInput, out Vector3 hitPosition);
            if (raycastObject != null && raycastObject.TryGetComponent(out T component))
            {
                listenerAction?.Invoke(component, hitPosition);
            }
        };
    }

    private bool TryGetSceneInput(string gameInputName, out SceneInput sceneInput)
    {
        sceneInput = sceneInputs.Find(input => input.GetGameInputName == gameInputName);
        return sceneInput != null;
    }

    private GameObject GetRaycastObject(SceneInput sceneInput, out Vector3 hitPosition)
    {
        GameObject raycastObject = null;
        hitPosition = Vector3.zero;
        if (Physics.Raycast(raycastCamera.ScreenPointToRay(InputDetector.Instance.GetMousePosition),
                out RaycastHit hit, Mathf.Infinity, sceneInput.GetRaycastMask))
        {
            raycastObject = hit.collider.gameObject;
            hitPosition = hit.point;
        }

        return raycastObject;
    }

    public Vector3 SceneToScreenPosition(Vector3 target)
    {
        return raycastCamera.WorldToScreenPoint(target);
    }
}

[Serializable]
public class SceneInput
{
    public event UnityAction OnObjectReceived; 

    public string GetGameInputName => gameInputName;
    public LayerMask GetRaycastMask => raycastMask;
    
    [SerializeField] private string gameInputName;
    [SerializeField] private LayerMask raycastMask;

    public void CallObjectReceived()
    {
        OnObjectReceived?.Invoke();
    }
}