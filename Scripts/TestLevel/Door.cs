using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Door : InteractiveObject
{
    public bool IsOpen => _isOpen;

    [SerializeField] private Transform rotationTarget;
    [SerializeField] private Vector2 openDegreesBounds;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float defaultYRotation;
    [SerializeField] private bool invertRotation;
    [Space(10f)] 
    [SerializeField] private SoundPresetBase openCloseSound;

    private IEnumerator _rotationCoroutine;
    private bool _isOpen;

    public override void Interact()
    {
        ToggleState(AISightTargets.Instance.GetMainTarget);
    }

    public override void Interact(GameObject client)
    {
        ToggleState(client.transform);
    }

    private void ToggleState(Transform clientTransform)
    {
        _isOpen = !_isOpen;
        OnStateToggled(clientTransform);
    }

    private void OnStateToggled(Transform clientTransform)
    {
        if(_rotationCoroutine != null) 
            StopCoroutine(_rotationCoroutine);
        _rotationCoroutine = RotateOverTime(GetYRotationTarget(_isOpen, clientTransform));
        StartCoroutine(_rotationCoroutine);
        
        SoundsPlayer.Instance.PlaySoundOnly(openCloseSound, rotationTarget.position);
    }

    private IEnumerator RotateOverTime(float targetRotationY)
    {
        Quaternion targetRotation = Quaternion.Euler(0f, targetRotationY, 0f);
        while (!rotationTarget.localRotation.Equals(targetRotation))
        {
            rotationTarget.localRotation =
                Quaternion.RotateTowards(rotationTarget.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private float GetYRotationTarget(bool isOpen, Transform clientTransform)
    {
        if (!isOpen) return defaultYRotation;
        int directionMultiplier =
            (Vector3.Dot(rotationTarget.forward, clientTransform.position - rotationTarget.position) >= 0
                ? -1 : 1) 
            * (invertRotation ? -1 : 1);
        return defaultYRotation + Random.Range(openDegreesBounds.x, openDegreesBounds.y) * directionMultiplier;
    }
}
