using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTaskPerformer : MonoBehaviour
{
    public event UnityAction<PlayerTask> OnPerformingStarted; 
    public event UnityAction<PlayerTask> OnPerformingStartDeclined;
    public event UnityAction<PlayerTask> OnPerformingCancelled; 
    public event UnityAction<PlayerTask> OnPerformingCompleted; 

    public static PlayerTaskPerformer Instance;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private string cancelPerformingInputName;

    private IEnumerator _performingCoroutine;
    private PlayerTask current;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputDetector.Instance.AddInputListener(cancelPerformingInputName, CancelPerforming);

        OnPerformingStarted += taskContainer =>
        {
            playerController.Disable(this);
            playerAnimator.PlayTaskPerformingAnimation(taskContainer.GetTaskInfo);
        };
        OnPerformingCancelled += _ =>
        {
            playerController.Enable(this);
        };
        OnPerformingCompleted += _ =>
        {
            playerController.Enable(this);
        };
    }

    public bool TryPerform(PlayerTask playerTask)
    {
        if (StaticFunctions.FlatDistance(playerTransform.position, playerTask.GetOriginPosition) <=
            playerTask.GetPerformRadius)
        {
            CancelPerforming();
            StartPerforming(playerTask);
            return true;
        }
        OnPerformingStartDeclined?.Invoke(playerTask);
        return false;
    }

    private void StartPerforming(PlayerTask playerTask)
    {
        current = playerTask;
        _performingCoroutine = Performing(playerTask.GetTaskInfo);
        StartCoroutine(_performingCoroutine);
        OnPerformingStarted?.Invoke(current);
    }

    private void CancelPerforming()
    {
        if(current == null) return;
        
        current = null;
        StopCoroutine(_performingCoroutine);
        OnPerformingCancelled?.Invoke(current);
    }

    private void CompletePerforming()
    {
        current.CompleteTask();
        current = null;
        OnPerformingCompleted?.Invoke(current);
    }

    private IEnumerator Performing(TaskInfo taskInfo)
    {
        float targetTime = taskInfo.GetPerformTime;
        yield return new WaitForSeconds(targetTime);
        CompletePerforming();
    }
}
