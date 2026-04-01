using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMovementInput(GameObject gameObject);
    public abstract float RetrieveVerticalInput(GameObject gameObject);
    public abstract bool RetrieveJumpInput(GameObject gameObject);
    public abstract bool RetrieveJumpHoldInput(GameObject gameObject);
    public abstract bool RetrieveRestartInput(GameObject gameObject);
    public abstract bool RetrieveDashInput(GameObject gameObject);
}
