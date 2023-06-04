using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AIMindFirstOk : MonoBehaviour
{
    public bool IsConcerned => _isConcerned;
    public bool IsSuspected => _isSuspected;

    [SerializeField] private AISightProcessor aiSightProcessor;
    [SerializeField] private ItemHolderSlot itemHolderSlot;
    [SerializeField] private AIHead aiHead;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private AIWalkController walkController;
    [SerializeField] private AIPatrolMind patrolMind;
    [SerializeField] private AIVoice aiVoice;
    [SerializeField] private Ear ear;
    [Space(10f)] 
    [SerializeField] private Transform target;
    [SerializeField] private float memoryTime;
    [SerializeField] private float concernedSaveTime;
    [SerializeField] private float timeBeforeSuspected;
    [SerializeField] private float timeBeforeConcerned;
    [SerializeField] private float searchRadiusOnConcerned;

    private bool _wasInSightInPreviousFrame;
    private bool _performingResearch;
    private bool _isInSight;
    private bool _isSuspected;
    private bool _isConcerned;
    private bool _isInMemory;
    private float _concernedTimeLeft;
    private float _inSightTime;

    private IEnumerator _searchAreaCoroutine;
    private IEnumerator _memoryCoroutine;

    private void Start()
    {
        ear.OnHear += GetSound;
        aiSightProcessor.OnAdditionalTargetInSight += GetAlerted;
    }

    #region Main Logic

    private void Update()
    {
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
        {
            StopAllActivities();
            StraightTargetFollow();
            AttackTarget();
            AlertAboutTarget();
        }
        else if (IsSuspected)
        {
            StopAllActivities();
            SuspectTargetPosition();
        }
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
            StraightTargetFollow();
            return;
        }

        if (!_performingResearch)
        {
            if (!patrolMind.IsPatrolling)
            {
                GoPatrolling();
            }
        }
    }

    #endregion
    
    private void StopAllActivities()
    {
        StopPatrolling();
        StopSearchingInArea();
    }
    
    private void StraightTargetFollow()
    {
        ConcernedCheckPosition(target.position);
    }

    private void ConcernedCheckPosition(Vector3 position)
    {
        _performingResearch = true;
        walkController.GoTo(position, () => SearchInArea(position, searchRadiusOnConcerned, 
            () => _performingResearch = false));
    }

    private void AttackTarget()
    {
        // itemHolderSlot.TryToUse(AISightTargets.Instance.GetMainTarget.gameObject);
    }

    private void SuspectTargetPosition()
    {
        SuspectPosition(target.position);
    }
    
    private void SuspectPosition(Vector3 position)
    {
        _performingResearch = true;
        walkController.GoTo(position, () => LookAround(() => _performingResearch = false));
    }

    private void AlertAboutTarget()
    {
        aiVoice.AlertNearby(target.position);//, this);
    }

    public void GetAlerted(Vector3 position)
    {
        StopAllActivities();
        RenewConcernedTimeLeft();
        SuspectPosition(position);
    }

    private void GetSound(Vector3 position)
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
        Debug.Log("Searching in area!");
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
    
    private void LookAround(UnityAction onPerformed = null)
    {
        Debug.Log("Look around!");
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
                _isSuspected = true;
            }
            else
            {
                _isSuspected = false;
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
            _isConcerned = _concernedTimeLeft > 0;
        }
    }

    private void ResetInSightTime()
    {
        _inSightTime = 0f;
        _isSuspected = false;
    }

    private void RenewConcernedTimeLeft()
    {
        _concernedTimeLeft = concernedSaveTime;
    }
}