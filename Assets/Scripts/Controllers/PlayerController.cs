using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController", order = 1)]
public class PlayerController : InputController
{
    // Reference to the input actions asset
    private InputSystem_Actions inputActions;

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();
        }
        inputActions.Enable();
    }
    private void OnDisable()
    {
        // Defensive: ensure inputActions is initialized before disabling
        if (inputActions == null)
        {
            return;
        }
        inputActions.Disable();
    }

    public override bool RetrieveRestartInput(GameObject gameObject)
    {
        return inputActions.Player.Restart.triggered;
    }

    public override float RetrieveMovementInput(GameObject gameObject)
    {
        // Assuming you have a "Move" action set up as a Vector2
        Vector2 move = inputActions.Player.Move.ReadValue<Vector2>();
        return move.x;
    }

    public override float RetrieveVerticalInput(GameObject gameObject)
    {
        Vector2 move = inputActions.Player.Move.ReadValue<Vector2>();
        return move.y;
    }

    public override bool RetrieveDashInput(GameObject gameObject)
    {
        return inputActions.Player.Dash.triggered;
    }

    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        // Assuming you have a "Jump" action set up as a Button
        return inputActions.Player.Jump.triggered;
    }

    public override bool RetrieveJumpHoldInput(GameObject gameObject)
    {
        // Assuming you have a "Jump" action set up as a Button
        return inputActions.Player.Jump.ReadValue<float>() > 0;
    }
}
