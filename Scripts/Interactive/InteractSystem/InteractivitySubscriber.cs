using UnityEngine;

public class InteractivitySubscriber : MonoBehaviour
{
    [SerializeField] private string gameInputName;
    
    private void Start()
    {
        SceneInputDetector.Instance.AddSceneInputListener(gameInputName,
            (Interactive interactiveObject) =>
            {
                interactiveObject.Interact();
            });
    }
}
