using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AIHead : MonoBehaviour
{
    public Transform GetHeadTransform => headTransform;
    
    [SerializeField] private Transform headTransform;
    [SerializeField] private float lookAroundTime;
    [SerializeField] private float lookAtTargetTime;
    [SerializeField] private float maxHeadRotation;
    
    public void LookAround(UnityAction onPerformed = null)
    {
        StopAllCoroutines();
        ResetHeadRotation();
        StartCoroutine(LookAroundCoroutine(onPerformed));
    }

    private void ResetHeadRotation()
    {
        headTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }
    
    // public void LookAt(Vector3 target, UnityAction onCompleted = null)
    // {
    //     StopAllCoroutines();
    //     ResetHeadRotation();
    //     StartCoroutine(RotateHead(target, lookAtTargetTime, onCompleted));
    // }

    private IEnumerator LookAroundCoroutine(UnityAction onPerformed = null)
    {
        yield return StartCoroutine(RotateHead(maxHeadRotation, lookAroundTime / 4));
        yield return StartCoroutine(RotateHead(-maxHeadRotation, lookAroundTime / 2));
        yield return StartCoroutine(RotateHead(0, lookAroundTime / 4));
        
        onPerformed?.Invoke();
    }

    private IEnumerator RotateHead(float degrees, float time, UnityAction onCompleted = null)
    {
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, degrees, 0f));
        yield return StartCoroutine(RotateHeadTo(targetRotation, time, onCompleted));
    }
    
    private IEnumerator RotateHead(Vector3 target, float time, UnityAction onCompleted = null)
    {
        target.y = headTransform.position.y;
        Quaternion targetRotation = Quaternion.LookRotation(target, headTransform.up);
        yield return StartCoroutine(RotateHeadTo(targetRotation, time, onCompleted));
    }

    private IEnumerator RotateHeadTo(Quaternion targetRotation, float time, UnityAction onCompleted = null)
    {
        float timePast = 0;
        // float speed = 0;
        Quaternion startRotation = headTransform.localRotation;
        while (!headTransform.localRotation.Equals(targetRotation))
        {
            timePast += Time.deltaTime;
            headTransform.localRotation = Quaternion.Lerp(startRotation, targetRotation, timePast/time);
            // headTransform.localRotation = Quaternion.RotateTowards(headTransform.localRotation, targetRotation, timePast/time);
            yield return new WaitForEndOfFrame();
        }
        onCompleted?.Invoke();
    }
}
