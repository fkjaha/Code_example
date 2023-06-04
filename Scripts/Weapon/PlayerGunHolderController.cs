using UnityEngine;

public class PlayerGunHolderController : MonoBehaviour
{
    [SerializeField] private Transform holderCenter;
    [SerializeField] private Transform transformToControl;

    private void Update()
    {
        var holderCenterPosition = holderCenter.position;
        Vector2 playerPositionOnScreen = SceneInputDetector.Instance.SceneToScreenPosition(holderCenterPosition);
        Vector2 cursorVector = InputDetector.Instance.GetMousePosition - playerPositionOnScreen;
        transformToControl.rotation =
            Quaternion.LookRotation(cursorVector.Vector2ToVector3XZ(), Vector3.up);
    }
}
