/*
    This is a simple controller that does nothing, used for when the players switch characters and we want to disable the previous character's input 
    without disabling the entire controller component.
*/

using UnityEngine;

[CreateAssetMenu(fileName = "EmptyController", menuName = "InputController/EmptyController", order = 0)]
public class EmptyController : InputController
{
    public override float RetrieveMovementInput(GameObject gameObject)
    {
        return 0;
    }

    public override bool RetrieveInteractInput(GameObject gameObject)
    {
        return false;
    }

    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        return false;
    }

    public override bool RetrieveJumpHoldInput(GameObject gameObject)
    {
        return false;
    }

    public override bool RetrieveRestartInput(GameObject gameObject)
    {
        return false;
    }

    public override bool RetrieveDashInput(GameObject gameObject)
    {
        return false;
    }

    public override float RetrieveVerticalInput(GameObject gameObject)
    {
        return 0;
    }
}
