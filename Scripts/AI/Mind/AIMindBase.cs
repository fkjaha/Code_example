using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AIMindBase : MonoBehaviour
{
    public event UnityAction OnDestroyEvent;
    public event UnityAction OnConcernedStateChanged;
    public event UnityAction OnSuspectedStateChanged;
    
    public bool IsConcerned => _isConcerned;
    public bool IsSuspected => _isSuspected;

    public AISightProcessor GetSightProcessor => aiSightProcessor;
    public FieldOfView GetFieldOfView => fieldOfView;
    
    [SerializeField] private AISightProcessor aiSightProcessor;
    [SerializeField] private AIHead aiHead;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private protected AIWalkController walkController;
    [SerializeField] private AIPatrolMind patrolMind;
    [SerializeField] private AIVoice aiVoice;
    [SerializeField] private Ear ear;
    [SerializeField] private AIAnimator aiAnimator;
    [Space(10f)] 
    [SerializeField] private float memoryTime;
    [SerializeField] private float concernedSaveTime;
    [SerializeField] private float timeBeforeSuspected;
    [SerializeField] private float timeBeforeConcerned;
    [SerializeField] private float searchRadiusOnConcerned;

    private protected Transform _target;
    private protected bool _performingResearch;

    private bool _wasInSightInPreviousFrame;
    private bool _isInSight;

    private bool _isSuspected;
    private bool _isConcerned;
    private bool _isInMemory;
    private float _concernedTimeLeft;
    private float _inSightTime;
    private bool _enabled;

    private IEnumerator _searchAreaCoroutine;
    private IEnumerator _memoryCoroutine;

    private void Start()
    {
        _enabled = true;
        ear.OnHear += GetSound;
        ear.OnHearAndAlerted += GetAlerted;
        aiSightProcessor.OnAdditionalTargetInSight += GetAlerted;
        _target = AISightTargets.Instance.GetMainTarget;
    }

    private void OnDestroy()
    {
        StopAllActivities();
        _enabled = false;
        OnDestroyEvent?.Invoke();
    }

    #region Main Logic

    private void Update()
    {
        if(!_enabled) return;
        PerformMindCycle();
    }

    private void PerformMindCycle()
    {
        EstablishInSightState();
        CountSightTime();
        CountConcernedTimeLeft();
        MainLogicCycle();
    }

    private void MainLogicCycle()
    {
        if (_isInSight)
            InSight();
        else
            NotInSight();
    }

    private void InSight()
    {
        if (_isConcerned)
            InSightAndConcerned();
        else if (IsSuspected)
            InSightAndSuspected();
    }

    private protected virtual void InSightAndConcerned()
    {
        StopAllActivities();
        StraightTargetFollow();
        // AttackTarget();
        AlertAboutTarget();
    }
    
    private protected virtual void InSightAndSuspected()
    {
        StopAllActivities();
        SuspectTargetPosition();
    }

    private void NotInSight()
    {
        if (_wasInSightInPreviousFrame)
        {
            if (IsConcerned)
            {
                StartMemory();
            }

            _wasInSightInPreviousFrame = false;
        }

        if (_isInMemory)
        {
            IsInMemory();
            return;
        }

        NotInMemory();
    }

    private protected virtual void IsInMemory()
    {
        StraightTargetFollow();
    }
    
    private protected virtual void NotInMemory()
    {
        if (!_performingResearch)
        {
            if (!patrolMind.IsPatrolling)
            {
                GoPatrolling();
            }
        }
    }

    #endregion
    
    private protected void StopAllActivities()
    {
        StopPatrolling();
        StopSearchingInArea();
    }
    
    private void StraightTargetFollow()
    {
        ConcernedCheckPosition(_target.position);
    }

    private void ConcernedCheckPosition(Vector3 position)
    {
        _performingResearch = true;
        walkController.GoTo(position, () => SearchInArea(position, searchRadiusOnConcerned, 
            () => _performingResearch = false));
    }

    private void SuspectTargetPosition()
    {
        SuspectPosition(_target.position);
    }
    
    private protected void SuspectPosition(Vector3 position)
    {
        _performingResearch = true;
        walkController.GoTo(position, () => LookAround(() => _performingResearch = false));
    }

    private protected void AlertAboutTarget()
    {
        aiVoice.AlertNearby(_target.position, this);
    }

    private protected virtual void GetAlerted(Vector3 position)
    {
        StopAllActivities();
        RenewConcernedTimeLeft();
        SuspectPosition(position);
    }

    private protected virtual void GetSound(Vector3 position)
    {
        StopAllActivities();
        
        if (IsConcerned)
            ConcernedCheckPosition(position);
        else
            SuspectPosition(position);
    }

    #region Memory

    private void StartMemory()
    {
        ClearMemory();
        _memoryCoroutine = Memory();
        StartCoroutine(_memoryCoroutine);
    }

    private IEnumerator Memory()
    {
        _isInMemory = true;
        yield return new WaitForSeconds(memoryTime);
        _isInMemory = false;
    }

    private void ClearMemory()
    {
        if(!_isInMemory) return;
        
        StopCoroutine(_memoryCoroutine);
        _isInMemory = false;
    }
    
    #endregion

    #region Patrolling

    private void GoPatrolling()
    {
        patrolMind.GoPatrolling();
    }

    private void StopPatrolling()
    {
        patrolMind.StopPatrolling();
    }

    #endregion
    
    private void SearchInArea(Vector3 position, float radius, UnityAction onPerformed = null)
    {
        _searchAreaCoroutine = SearchAreaCoroutine(position, radius, onPerformed);
        StartCoroutine(_searchAreaCoroutine);
    }
    
    private IEnumerator SearchAreaCoroutine(Vector3 position, float radius, UnityAction onPerformed = null)
    {
        bool pathCompleted = false;
        walkController.GoTo(position, () =>
        {
            LookAround(() => pathCompleted = true);
        });

        for (int i = 0; i < 3; i++)
        {
            while (!pathCompleted)
            {
                yield return new WaitForEndOfFrame();
            }

            pathCompleted = false;
            walkController.GoTo(position + new Vector3(Random.Range(-1, 1), 0f, Random.Range(-1, 1)).normalized * radius, () =>
            {
                LookAround(() => pathCompleted = true);
            });
        }
        
        onPerformed?.Invoke();
    }

    private void StopSearchingInArea()
    {
        if (_performingResearch && _searchAreaCoroutine != null)
        {
            StopCoroutine(_searchAreaCoroutine);
            _performingResearch = false;
        }
    }
    
    private protected void LookAround(UnityAction onPerformed = null)
    {
        aiHead.LookAround(onPerformed);
    }
    
    private void EstablishInSightState()
    {
        _isInSight = aiSightProcessor.GetMainTargetInSightState;
        if(_isInSight) _wasInSightInPreviousFrame = _isInSight;
    }

    private void CountSightTime()
    {
        if (_isInSight)
        {
            _inSightTime += Time.deltaTime;
            if (_isConcerned || _inSightTime >= timeBeforeConcerned)
            {
                RenewConcernedTimeLeft();
            }
            else if (_inSightTime >= timeBeforeSuspected)
            {
                SetSuspectedState(true);
            }
            else
            {
                SetSuspectedState(false);
            }
        }
        else
            ResetInSightTime();
    }

    private void CountConcernedTimeLeft()
    {
        if (_concernedTimeLeft > 0)
        {
            _concernedTimeLeft -= Time.deltaTime;
            SetConcernedState(_concernedTimeLeft > 0);
        }
    }

    private void SetConcernedState(bool newState)
    {
        if (_isConcerned != newState)
        {
            _isConcerned = newState;
            OnConcernedStateChanged?.Invoke();
        }
    }
    
    private void SetSuspectedState(bool newState)
    {
        if (_isSuspected != newState)
        {
            _isSuspected = newState;
            OnSuspectedStateChanged?.Invoke();
        }
    }

    private void ResetInSightTime()
    {
        _inSightTime = 0f;
        SetSuspectedState(false);
    }

    private void RenewConcernedTimeLeft()
    {
        _concernedTimeLeft = concernedSaveTime;
    }

    public PathActionsEvents GetActionsEvents()
    {
        return new PathActionsEvents(walkController.GoTo, aiAnimator.PlayAnimation, WaitAction,
             walkController.LookAtPosition);
    }

    private void WaitAction(float time, UnityAction onCompleted = null)
    {
        StartCoroutine(WaitActionCoroutine(time, onCompleted));
    }
    
    private IEnumerator WaitActionCoroutine(float time, UnityAction onCompleted = null)
    {
        yield return new WaitForSeconds(time);
        onCompleted?.Invoke();
        StopAllCoroutines();
    }
}
