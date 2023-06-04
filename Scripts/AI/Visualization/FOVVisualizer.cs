using UnityEngine;

public class FOVVisualizer : AIStatVisualizer
{
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private AISightProcessor aiSightProcessor;
    [SerializeField] private FOVMeshRenderer fovMeshRenderer;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color passiveColor;
    [SerializeField] private Color activeColor;

    private Transform _eyeTransform;
    private float _viewAngle;
    private float _viewDistance;

    private void Update()
    {
        if(currentTarget == null) return;
        
        fovMeshRenderer.UpdateVisualization();
        meshRenderer.material.color = aiSightProcessor.GetMainTargetInSightState ? activeColor : passiveColor;
    }

    private protected override void OnTargetChanged()
    {
        base.OnTargetChanged();

        if(currentTarget == null) return;

        fieldOfView = currentTarget.GetFieldOfView;
        aiSightProcessor = currentTarget.GetSightProcessor;
        
        _eyeTransform = fieldOfView.GetEyesTransform;
        _viewAngle = fieldOfView.GetViewAngle;
        _viewDistance = fieldOfView.GetViewDistance;
        
        fovMeshRenderer.UpdateInfo(_eyeTransform, _viewAngle, _viewDistance);
    }
}
