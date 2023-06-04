using UnityEngine;

public class CursourFollower : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    
    private void Update()
    {
        targetTransform.position = InputDetector.Instance.GetMousePosition;
    }
}
