using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AIMindOldest : MonoBehaviour
{
    public bool IsConcerned => _isConcerned;
    public bool IsSuspected => _isSuspected;

    [SerializeField] private ItemHolderSlot itemHolderSlot;
    [SerializeField] private AIHead aiHead;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private AIWalkController walkController;
    [SerializeField] private AIPatrolMind patrolMind;
    [Space(10f)] 
    [SerializeField] private Transform target;
    [SerializeField] private float memoryTime;

    [SerializeField] private float timeBeforeSuspected;
    [SerializeField] private float timeBeforeConcerned;

    [SerializeField] private float searchRadiusOnConcerned;
    
    [Header("Tests")]
    [SerializeField] private Material sightIndicatorMaterial;
    [SerializeField] private Color passiveColor = Color.black;
    [SerializeField] private Color activeColor = Color.black;

    private bool _wasInSightInPreviousFrame;
    private bool _wasPerformingResearchInPreviousFrame;
    private bool _performingResearch;
    private bool _isInSight;
    private float _inSightTime;
    private bool _isSuspected;
    private bool _isConcerned;
    private bool _isPatrolling;
    private bool _isInMemory;

    private IEnumerator _searchAreaCoroutine;
    private IEnumerator _patrollingCoroutine;
    private IEnumerator _memoryCoroutine;

    private void Start()
    {
        StartCoroutine(CountSightTime());
    }

    private void Update()
    {
        PerformMindCycle();
    }

    private void PerformMindCycle()
    {
        EstablishInSightState();
        VisualizeInSightState();
        MainLogicCycle();
    }

    private void MainLogicCycle()
    {
        if (!_isInSight)
        {
            if(_wasInSightInPreviousFrame)
            {
                _wasInSightInPreviousFrame = false;
                StartMemory();
                ResetInSightTime();
            }
            if(!_isInMemory && !_performingResearch)
            {
                if(_wasPerformingResearchInPreviousFrame)
                {
                    _wasPerformingResearchInPreviousFrame = false;
                    GoPatrolling();
                }
                else
                {
                    if(!_isPatrolling)
                        GoPatrolling();
                }
            }
        }
        if (_isInSight && _isConcerned)
        {
            // itemHolderSlot.TryToUse(AISightTargets.Instance.GetMainTarget.gameObject);
        }
        
        if (_inSightTime >= timeBeforeConcerned)
        {
            StopPatrolling();
            ConcernAboutPosition(target.position);
            _isConcerned = true;
        }
        else if (_inSightTime >= timeBeforeSuspected)
        {
            StopPatrolling();
            SuspectPosition(target.position);
            _isSuspected = true;
        }
    }

    private void StartMemory()
    {
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

    private void SuspectPosition(Vector3 position)
    {
        _performingResearch = true;
        walkController.GoTo(target.position, () => LookAround(() => _performingResearch = false));
    }

    private void ConcernAboutPosition(Vector3 position)
    {
        _performingResearch = true;
        walkController.GoTo(target.position, () => SearchInArea(position, searchRadiusOnConcerned, 
            () => _performingResearch = false));
    }

    private void GoPatrolling()
    {
        _isPatrolling = true;
        _patrollingCoroutine = Patrolling();
        StartCoroutine(_patrollingCoroutine);
    }

    private IEnumerator Patrolling()
    {
        while (true)
        {
            bool reachedDestination = false;
            walkController.GoTo(patrolMind.GetCurrentTargetPoint.transform.position, () =>
            {
                patrolMind.RequestCurrentPatrolPointGoNext();
                reachedDestination = true;
            });

            while (!reachedDestination)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void StopPatrolling()
    {
        if(!_isPatrolling) return;
        
        if(_patrollingCoroutine != null)
            StopCoroutine(_patrollingCoroutine);
        _isPatrolling = false;
    }

    private void SearchInArea(Vector3 position, float radius, UnityAction onPerformed = null)
    {
        if(_searchAreaCoroutine != null)
            StopCoroutine(_searchAreaCoroutine);
        Debug.Log("Searching in area!");
        _searchAreaCoroutine = SearchAreaCoroutine(position, radius, onPerformed);
        StartCoroutine(_searchAreaCoroutine);
    }

    private IEnumerator SearchAreaCoroutine(Vector3 position, float radius, UnityAction onPerformed = null)
    {
        bool pathCompleted = false;
        walkController.GoTo(position, () =>
        {
            LookAround();
            pathCompleted = true;
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
                LookAround();
                pathCompleted = true;
            });
        }
        
        onPerformed?.Invoke();
    }

    private void LookAround(UnityAction onPerformed = null)
    {
        Debug.Log("Look around!");
        aiHead.LookAround(onPerformed);
    }

    private IEnumerator CountSightTime()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (_isInSight) _inSightTime += Time.deltaTime;
        }
    }

    private void ResetInSightTime()
    {
        _inSightTime = 0f;
        _isConcerned = false;
        _isSuspected = false;
    }
    
    private void EstablishInSightState()
    {
        _isInSight = fieldOfView.IsInView(target);
        if(_isInSight) _wasInSightInPreviousFrame = _isInSight;
    }

    private void VisualizeInSightState()
    {
        sightIndicatorMaterial.color = _isInSight ? activeColor : passiveColor;
    }
}
