using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AIExecution : MonoBehaviour
{
    public event UnityAction<DeathType> OnExecutionStarted;
    public event UnityAction<DeathType> OnExecutionCompleted;
    public event UnityAction OnExecutionCancelled;

    [SerializeField] private AIWalkController aiWalkController;
    [SerializeField] private AIAnimator aiAnimator;
    [SerializeField] private HPContainer hpContainer;

    private IEnumerator _executionCoroutine;
    private bool _beingExecuted;

    private void Start()
    {
        OnExecutionStarted += _ =>
        {
            // Debug.Log("Started Execution");
            aiWalkController.DisableWalking();
        };
        
        OnExecutionCompleted += _ =>
        {
            // Debug.Log("Execution Completed");
        };
        
        OnExecutionCancelled += () =>
        {
            // Debug.Log("Execution Cancelled");
            aiWalkController.EnableWalking();
        };
    }

    public void Execute(DeathType deathType, float time)
    {
        if(_beingExecuted) return;
        
        CancelExecution();   
        _beingExecuted = true;
        OnExecutionStarted?.Invoke(deathType);
        hpContainer.InstantKill(deathType);
        _executionCoroutine = Execution(deathType, time);
        StartCoroutine(_executionCoroutine);
    }

    private void CancelExecution()
    {
        if(!_beingExecuted) return;
        
        aiAnimator.ReturnToDefaultAnimationsCycle();
        StopCoroutine(_executionCoroutine);
        _beingExecuted = false;
        OnExecutionCancelled?.Invoke();
    }

    private IEnumerator Execution(DeathType deathType, float time)
    {
        yield return new WaitForSeconds(time);
        OnExecutionCompleted?.Invoke(deathType);
    }
}
