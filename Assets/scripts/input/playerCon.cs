using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "playerCon")]
public class playerCon : ScriptableObject, InputActions.IPlayerConActions
{
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction stopMove = delegate { };
    public event UnityAction onJump = delegate { };
    public event UnityAction stopJump = delegate { };
    public event UnityAction onRoll = delegate { };
    public event UnityAction stopRoll = delegate { };
    public event UnityAction onAttack = delegate { };
    public event UnityAction stopAttack = delegate { };
    public event UnityAction onDefend = delegate { };
    public event UnityAction stopDefend = delegate { };



    InputActions input_;
    void OnEnable()
    {
        input_ = new InputActions();
        input_.playerCon.SetCallbacks(this);
        EnableInput();
    }
    public void EnableInput()
    {
        input_.playerCon.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void DisableInput()
    {
        input_.playerCon.Disable();
    }
    void OnDisable()
    {
        DisableInput();
    }




    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            onMove.Invoke(context.ReadValue<Vector2>());
        if (context.canceled)
            stopMove.Invoke();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            onJump.Invoke();
        if (context.canceled)
            stopJump.Invoke();
    }
    public void OnPlayerRoll(InputAction.CallbackContext context)
    {
        if (context.performed)
            onRoll.Invoke();
        if (context.canceled)
            stopRoll.Invoke();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            onAttack.Invoke();
        if (context.canceled)
            stopAttack.Invoke();
    }
    public void OnDefend(InputAction.CallbackContext context)
    {
        if (context.performed)
            onDefend.Invoke();
        if (context.canceled)
            stopDefend.Invoke();
    }

}
