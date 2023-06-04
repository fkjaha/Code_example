using UnityEngine;

public class AIGunHolderController : MonoBehaviour
{
    [SerializeField] private Transform transformToControl;
    [SerializeField] private AIHead aiHead;
    
    private void Update()
    {
        transformToControl.rotation =
            Quaternion.LookRotation(aiHead.GetHeadTransform.forward, Vector3.up);
    }
}
