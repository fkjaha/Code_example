using System;
using UnityEngine;

public class FOVVisualizerOld : MonoBehaviour
{
    [Header("FOV Visualizer")]
    
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private AISightProcessor aiSightProcessor;
    
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int numberOfRenderCircleSegments;
    [SerializeField] private Gradient passiveColor;
    [SerializeField] private Gradient activeColor;

    private Transform _eyeTransform;
    private float _viewAngle;
    private float _viewDistance;

    private void Start()
    {
        _eyeTransform = fieldOfView.GetEyesTransform;
        _viewAngle = fieldOfView.GetViewAngle;
        _viewDistance = fieldOfView.GetViewDistance;
    }

    private void Update()
    {
        Vector3 eyeTransformPosition = _eyeTransform.position;
        Vector3 distanceVector = new Vector3(_viewDistance, 0f, 0f);
        float yRotation = _eyeTransform.eulerAngles.y - 90;

        Vector3 pos1 = eyeTransformPosition + Quaternion.AngleAxis(_viewAngle / 2 + yRotation, Vector3.up) * distanceVector;
        Vector3 pos2 = eyeTransformPosition;
        Vector3 pos3 = eyeTransformPosition + Quaternion.AngleAxis(-_viewAngle / 2 + yRotation, Vector3.up) * distanceVector;
        
        lineRenderer.SetPositions(new [] {pos1, pos2, pos3});
        
        float angleSegment = _viewAngle / numberOfRenderCircleSegments;
        for (int i = 0; i < numberOfRenderCircleSegments; i++)
        {
            Vector3 pos = eyeTransformPosition + Quaternion.AngleAxis(-_viewAngle / 2 + angleSegment * (i+1) + yRotation, Vector3.up) * distanceVector;
            lineRenderer.SetPosition(3 + i, pos);
        }

        lineRenderer.colorGradient = aiSightProcessor.GetMainTargetInSightState ? activeColor : passiveColor;
    }
}
