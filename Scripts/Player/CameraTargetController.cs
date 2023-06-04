using System;
using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform defaultTarget;
    [SerializeField] private float moveSpeed;
    [SerializeField] private string moveGameInputName;
    [SerializeField] private string resetGameInputName;

    private Vector2 _defaultPositionOffset;

    private void Start()
    {
        InputDetector.Instance.AddInputListener(moveGameInputName, MoveCameraTargetWithMouse);
        InputDetector.Instance.AddInputListener(resetGameInputName, ResetCameraTargetPosition);
    }

    private void Update()
    {
        MoveCamera();
    }

    private void ResetCameraTargetPosition()
    {
        _defaultPositionOffset = Vector2.zero;
    }

    private void MoveCameraTargetWithMouse()
    {
        _defaultPositionOffset += InputDetector.Instance.GetMousePositionDelta * moveSpeed * Time.deltaTime;
    }

    private void MoveCamera()
    {
        targetTransform.position = defaultTarget.position + new Vector3(_defaultPositionOffset.x, 0f, _defaultPositionOffset.y);
    }
}
