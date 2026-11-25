using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //private UIManager _uiManager;
    //private DialogueManager _dialogueManager;

    public float _moveSpeed = 5f;
    public Vector2 _moveInput;

    private Rigidbody2D _playerRb;

    public GameObject _attackCollider;
    private bool _isAttacking = false;

    //[Header("Interaction")]
    //[SerializeField] private GameObject _debugCurrentInteractable;
    //public Image _fullCircle;
    //private IInteractable _targetInteractable;
    //private float _holdDuration = 1f;
    //private float _holdTimer = 0f;
    //private bool _isHolding = false;
    //private bool _canHold = true;

    //public bool _hasKey = false;

    //private LevelSwitch _levelSwitch;

    private Animator _animController;
    private float _animationSpeed = 1f;

    private int _isMovingHash = Animator.StringToHash("IsMoving");
    private int _isAttackingAnim = Animator.StringToHash("IsAttacking");
    private int _moveXHash = Animator.StringToHash("MoveX");
    private int _moveYHash = Animator.StringToHash("MoveY");
    private int _lastMoveXHash = Animator.StringToHash("LastMoveX");
    private int _lastMoveYHash = Animator.StringToHash("LastMoveY");

    private void Start()
    {
        //_uiManager = ServiceHub.Instance.UIManager;
        //_dialogueManager = ServiceHub.Instance.DialogueManager;
        _playerRb = GetComponent<Rigidbody2D>();
        if (_playerRb == null) Debug.LogError("Rigidbody2D component not found on the player object.");
        _animController = GetComponent<Animator>();

        //_canHold = false;
        //_uiManager.HideInteractionPrompt();
        _attackCollider.SetActive(false);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        //if (_dialogueManager.IsInDialogue) return;

        HandlePlayerAnimations();
        
        _attackCollider.transform.position = transform.position + new Vector3(_animController.GetFloat(_moveXHash), _animController.GetFloat(_moveYHash), 0).normalized * 0.5f;

        //if (_canHold && _isHolding)
        //{
        //    _holdTimer += Time.deltaTime;
        //    _fullCircle.fillAmount = _holdTimer / _holdDuration;
        //    if (_holdTimer >= _holdDuration)
        //    {
        //        Debug.Log("Hold action completed.");
        //        if (_targetInteractable != null)
        //        {
        //            _targetInteractable.Interact();
        //        }
        //        else
        //        {
        //            Debug.LogWarning("Hold completed but no LevelSwitch reference is set.");
        //        }
        //        ResetHold();
        //    }
        //}
    }

    private void FixedUpdate()
    {
        //if (_dialogueManager.IsInDialogue) return;
        HandlePlayerMovement();
    }

    private void HandlePlayerMovement()
    {
        _playerRb.MovePosition(_playerRb.position + _moveInput * _moveSpeed * Time.fixedDeltaTime);
    }

    //public void OnHold(InputAction.CallbackContext context)
    //{
    //    if (context.started)
    //    {
    //        _isHolding = true;
    //        Debug.Log("Hold action started.");
    //    }
    //    else if (context.canceled)
    //    {
    //        Debug.Log("Hold action canceled.");
    //        ResetHold();
    //    }
    //}

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _moveSpeed *= 1.5f; // Increase speed by 50% when sprinting
            _animationSpeed *= 1.5f; // Increase animation speed by 50% when sprinting
        }
        else if (context.canceled)
        {
            _moveSpeed /= 1.5f; // Reset to normal speed when not sprinting
            _animationSpeed /= 1.5f; // Reset to normal animation speed when not sprinting
        }
    }

    //private void ResetHold()
    //{
    //    _isHolding = false;
    //    _holdTimer = 0f;
    //    _fullCircle.fillAmount = 0f;
    //}

    public void OnAttack()
    {
        //if (_dialogueManager.IsInDialogue) return;

        if (!_isAttacking)
            StartCoroutine(AttackCycle());
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.TryGetComponent(out IInteractable foundInteractable))
    //    {
    //        _canHold = true;
    //        _levelSwitch = other.GetComponent<LevelSwitch>();

    //        _targetInteractable = foundInteractable;
    //        _debugCurrentInteractable = other.gameObject;

    //        //_uiManager.ShowInteractionPrompt();

    //    }
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.TryGetComponent(out IInteractable foundInteractable))
    //    {
    //        _canHold = false;
    //        ResetHold();
    //        if (_levelSwitch != null && _levelSwitch.gameObject == other.gameObject)
    //            _levelSwitch = null;

    //        _targetInteractable = null;
    //        _debugCurrentInteractable = null;

    //        //_uiManager.HideInteractionPrompt();
    //    }
    //}

    //public void OnInteract(InputAction.CallbackContext context)
    //{
    //    if (_dialogueManager.IsInDialogue) return;

    //    if (context.performed)
    //    {
    //        if (_targetInteractable != null)
    //        {
    //            //_uiManager.HideInteractionPrompt();

    //            _targetInteractable.Interact();
    //        }
    //    }
    //}

    private void HandlePlayerAnimations()
    {
        _animController.speed = _animationSpeed;

        if (_moveInput != Vector2.zero)
        {
            _animController.SetFloat(_lastMoveXHash, _animController.GetFloat(_moveXHash));
            _animController.SetFloat(_lastMoveYHash, _animController.GetFloat(_moveYHash));

            _animController.SetFloat(_moveXHash, _moveInput.x);
            _animController.SetFloat(_moveYHash, _moveInput.y);

            _animController.SetBool(_isMovingHash, true);
        }
        else
        {
            _animController.SetBool(_isMovingHash, false);
        }

        if (_isAttacking)
        {
            _animController.SetBool(_isAttackingAnim, true);
        }
        else
        {
            _animController.SetBool(_isAttackingAnim, false);
        }
    }

    public IEnumerator AttackCycle()
    {
        _attackCollider.SetActive(true);
        _isAttacking = true;
        Debug.Log("Attack started.");
        yield return new WaitForSeconds(1.017f);
        _attackCollider.SetActive(false);
        _isAttacking = false;
        Debug.Log("Attack ended.");
    }
}