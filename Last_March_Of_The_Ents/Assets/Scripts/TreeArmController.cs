using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TreeArmController : MonoBehaviour
{
    [SerializeField] private Transform _leftArmControl;
    [SerializeField] private Transform _rightArmControl;

    private PlayerInput _playerInput;
    private InputAction _moveRightArm;
    private InputAction _moveLeftArm;
    private InputAction _digRightLeg;
    private InputAction _digLeftLeg;

    [SerializeField] private Rigidbody2D _leftArmRB;
    [SerializeField] private Rigidbody2D _rightArmRB;

    private int _leftMoveInput = 0;
    private int _rightMoveInput = 0;

    [SerializeField] private float _armMaxHeight;
    [SerializeField] private float _armMinHeight;
    private bool leftCanMove;
    private bool rightCanMove;

    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float maxVelocity = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveLeftArm = _playerInput.actions["LeftArm"];
        _moveRightArm = _playerInput.actions["RightArm"];
        _digLeftLeg = _playerInput.actions["LeftLeg"];
        _digRightLeg = _playerInput.actions["RightLeg"];

        _moveLeftArm.performed += MoveLeftArm_Performed;
        _moveLeftArm.canceled += MoveLeftArm_Cancelled;
        _moveRightArm.performed += MoveRightArm_Performed;
        _moveRightArm.canceled += MoveRightArm_Cancelled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPlayer()
    {
        leftCanMove = true;
        rightCanMove = true;
        _leftArmRB.velocity = Vector2.zero;
        _rightArmRB.velocity = Vector2.zero;
        _leftMoveInput = 0;
        _rightMoveInput = 0;
    }

    private void FixedUpdate()
    {
        if ( GameControllerScript.gameCanStart)
        {
            if (_leftArmRB.transform.position.y > _armMaxHeight && leftCanMove)
            {
                leftCanMove = false;
                _leftArmRB.velocity = Vector2.zero;
            }
            else
            {
                leftCanMove = true;
            }

            if(_rightArmRB.transform.position.y > _armMaxHeight && rightCanMove)
            {
                rightCanMove = false;
                _rightArmRB.velocity = Vector2.zero;
            }
            else
            {
                rightCanMove = true;
            }

            if (_leftMoveInput > 0 && leftCanMove)
            {
                _leftArmRB.AddForce(new Vector2(0, _leftMoveInput) * moveSpeed);
                _leftArmRB.velocity = Vector2.ClampMagnitude(_leftArmRB.velocity, maxVelocity);
                Debug.Log("add force");
            }

            if (_rightMoveInput > 0 && rightCanMove)
            {
                _rightArmRB.AddForce(new Vector2(0, _rightMoveInput) * moveSpeed);
                _rightArmRB.velocity = Vector2.ClampMagnitude(_rightArmRB.velocity, maxVelocity);
            }

        }
    }

    private void MoveLeftArm_Performed(InputAction.CallbackContext obj)
    {
        _leftMoveInput = 1;
    }

    private void MoveLeftArm_Cancelled(InputAction.CallbackContext obj)
    {
        _leftMoveInput = 0;
    }

    private void MoveRightArm_Performed(InputAction.CallbackContext obj)
    {
        _rightMoveInput = 1;
    }

    private void MoveRightArm_Cancelled(InputAction.CallbackContext obj)
    {
        _rightMoveInput = 0;
    }
}
