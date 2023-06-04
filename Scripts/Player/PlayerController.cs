using UnityEngine;
using UnityEngine.Events;

public class PlayerController : AToggleable
{
    public event UnityAction OnMoved;
    
    public bool IsMoving => IsEnabled && _isMoving;
    public bool IsRunning => _isRunning;
    public Vector3 GetPosition => characterController.transform.position;
    
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform rotationContent;
    [SerializeField] private float rotationOffset;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float gravity;
    
    [Space(10f)]
    [SerializeField] private string playerRightGameInputName;
    [SerializeField] private string playerLeftGameInputName;
    [SerializeField] private string playerForwardGameInputName;
    [SerializeField] private string playerBackwardGameInputName;
    [SerializeField] private string playerRunningInputName;

    private Vector2 _movementDelta;
    private Vector3 _moveVector;
    private Vector3 _rotationVector;
    private bool _isRunning;
    private bool _isMoving;

    public void RotateInDirection(Vector3 direction)
    {
        _rotationVector = direction;
        _rotationVector.y = 0;
    }

    
    private protected override void Start()
    {
        base.Start();
        InputDetector.Instance.AddInputListener(playerRightGameInputName, () => AddMovementDelta(Vector2.right));
        InputDetector.Instance.AddInputListener(playerLeftGameInputName, () => AddMovementDelta(Vector2.left));
        InputDetector.Instance.AddInputListener(playerForwardGameInputName, () => AddMovementDelta(Vector2.up));
        InputDetector.Instance.AddInputListener(playerBackwardGameInputName, () => AddMovementDelta(Vector2.down));
        InputDetector.Instance.AddInputListener(playerRunningInputName, () => _isRunning = true, () => _isRunning = false);
    }

    private void Update()
    {
        if (IsEnabled)
        {
            CountMoveAndRotationVector();
            Move();
            if (_moveVector.x != 0 || _moveVector.z != 0)
            {
                OnMoved?.Invoke();
            }
        }
        Rotate();
        ResetMovementDelta();
    }

    private void AddMovementDelta(Vector2 delta)
    {
        _movementDelta += delta;
    }

    private void ResetMovementDelta()
    {
        _movementDelta = Vector2.zero;
    }

    private void CountMoveAndRotationVector()
    {
        _moveVector = new Vector3(_movementDelta.x, 0f, _movementDelta.y);
        if (_moveVector.x != 0 || _moveVector.z != 0)
            _rotationVector = _moveVector;
        _isMoving = _moveVector.x != 0 || _moveVector.z != 0;
        _moveVector = _moveVector.normalized;
        _moveVector.y = characterController.isGrounded ? 0f : -gravity;
    }

    private void Move()
    {
        characterController.Move(_moveVector * (_isRunning ? runSpeed : walkSpeed) * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 targetLookRotation = Quaternion.AngleAxis(rotationOffset, Vector3.up) * _rotationVector;
        
        if(targetLookRotation == Vector3.zero) return;
        
        Quaternion targetRotation =
            Quaternion.LookRotation(targetLookRotation, Vector3.up);
        rotationContent.rotation = Quaternion.Lerp(rotationContent.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
    }
}
