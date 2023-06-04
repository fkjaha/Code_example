using UnityEngine;

public class UseItemSubscriber : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private string inputName;
    
    private void Start()
    {
        SceneInputDetector.Instance.AddSceneInputListener<GameObject>(inputName, inventory.TryUseCurrentItem);
    }
}
