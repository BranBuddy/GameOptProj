/*
    Allows player to head towards the ground at an increased speed.
    Mainly a QoL feature to help with slow wall slide speeds.
*/

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Slam : MonoBehaviour
{

    public bool IsSlamActive { get; set; }
    [SerializeField] private InputActionReference slamAction;


    private Coroutine slamCoroutine;

    private void OnEnable()
    {
        slamAction.action.performed += OnSlamStarted;
        slamAction.action.canceled += OnSlamCanceled;
    }

    private void OnDisable()
    {
        slamAction.action.performed -= OnSlamStarted;
        slamAction.action.canceled -= OnSlamCanceled;
    }

    private void OnSlamStarted(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.currentPlayer != this.gameObject) return;
        WallInteractor wallInteractor = GetComponent<WallInteractor>();
        if (wallInteractor != null) IsSlamActive = true;
        if (slamCoroutine == null)
        {
            slamCoroutine = StartCoroutine(ApplySlamForceWhileHeld());
        }
    }

    private void OnSlamCanceled(InputAction.CallbackContext context)
    {
        WallInteractor wallInteractor = GetComponent<WallInteractor>();
        if (wallInteractor != null) IsSlamActive = false;
        if (slamCoroutine != null)
        {
            StopCoroutine(slamCoroutine);
            slamCoroutine = null;
        }
    }

    private IEnumerator ApplySlamForceWhileHeld()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        while (true)
        {
            if (rb != null)
            {
                rb.AddForce(Vector2.down * 20f, ForceMode2D.Force); // Adjust force as needed
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
