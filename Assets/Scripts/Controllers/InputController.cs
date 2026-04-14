/*
    This is an abstract class that defines the interface for retrieving player input. 
    Different implementations of this class can be created to handle different input methods (keyboard, gamepad, etc).
*/

using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMovementInput(GameObject gameObject);
    public abstract float RetrieveVerticalInput(GameObject gameObject);
    public abstract bool RetrieveJumpInput(GameObject gameObject);
    public abstract bool RetrieveJumpHoldInput(GameObject gameObject);
    public abstract bool RetrieveRestartInput(GameObject gameObject);
    public abstract bool RetrieveDashInput(GameObject gameObject);
    public abstract bool RetrieveInteractInput(GameObject gameObject);
}
