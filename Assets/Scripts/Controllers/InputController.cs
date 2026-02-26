using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMovementInput(GameObject gameObject);
    public abstract bool RetrieveJumpInput(GameObject gameObject);
    public abstract bool RetrieveJumpHoldInput(GameObject gameObject);
}
