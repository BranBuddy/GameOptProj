/*
    Allows the player to minimize which can change stat values. Changing size can help traverse smaller walkways or clear certain obstacles.
    If when maximizing the player can clip into an GO, the player will be reset to the last checkpoint to prevent softlocks.
*/


using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Minimize : MonoBehaviour
    
{
    [SerializeField] private InputActionReference minimizeAction;
    private Vector3 originalScale;
    private bool isMinimized = false;

    private Jump jump;
    private Move move;
    private Dash dash;
    private WallInteractor wallInteractor;
    private Restart restart;

    public AudioClip minimizeSfx, maximizeSfx;
    private float originalJumpHeight, originalMaxSpeed, originalDashSpeed, originalWallSlideMaxSpeed, originalAirAcceleration, originalMaxAcceleration;

    private void Awake()
    {
        originalScale = transform.localScale;
        jump = GetComponent<Jump>();
        move = GetComponent<Move>();
        dash = GetComponent<Dash>();
        wallInteractor = GetComponent<WallInteractor>();
        restart = GetComponent<Restart>();
    }

    private void OnEnable()
    {
        minimizeAction.action.performed += OnMinimize;
    }

    private void OnDisable()
    {
        minimizeAction.action.performed -= OnMinimize;
    }

    private void OnMinimize(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.currentPlayer != this.gameObject) return;
        if (isMinimized)
        {
            isMinimized = false;
            SoundManager.Instance.sfxSource.PlayOneShot(maximizeSfx);
            StartCoroutine(MaximizeSize());
            ResetValues();
        }
        else
        {
            SoundManager.Instance.sfxSource.PlayOneShot(minimizeSfx);
            StartCoroutine(MinimizeSize());
            ChangeValuesOnMinimize();
            isMinimized = true;
        }
        
        
    }

    private void ChangeValuesOnMinimize()
    {
        float posMultiplier = 1.5f;
        float negMultiplier = 0.5f;
        jump._jumpHeight *= posMultiplier;
        move._maxSpeed *= posMultiplier;
        dash._dashSpeed *= posMultiplier;
        wallInteractor._wallSlideMaxSpeed *= negMultiplier;
        move._maxAirAcceleration *= negMultiplier;
        move._maxAcceleration *= negMultiplier;
    }

    private void Start()
    {
        originalJumpHeight = jump._jumpHeight;
        originalMaxSpeed = move._maxSpeed;
        originalDashSpeed = dash._dashSpeed;
        originalWallSlideMaxSpeed = wallInteractor._wallSlideMaxSpeed;
        originalAirAcceleration = move._maxAirAcceleration;
        originalMaxAcceleration = move._maxAcceleration;
    }

    private void ResetValues()
    {
        jump._jumpHeight = originalJumpHeight;
        move._maxSpeed = originalMaxSpeed;
        dash._dashSpeed = originalDashSpeed;
        wallInteractor._wallSlideMaxSpeed = originalWallSlideMaxSpeed;
        move._maxAirAcceleration = originalAirAcceleration;
        move._maxAcceleration = originalMaxAcceleration;
    }

    private IEnumerator MaximizeSize()
    {
        Vector3 targetScale = originalScale;
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Slab")))
            {
                restart.Reset();
                break;
            }

            Vector3 scale = Vector3.Lerp(transform.localScale, targetScale, elapsedTime / duration);
            transform.localScale = scale;
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        transform.localScale = targetScale;
    }

    private IEnumerator MinimizeSize()
    {
        Vector3 targetScale = originalScale * 0.5f;
        float duration = 0.5f;
        float elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            Vector3 scale = Vector3.Lerp(transform.localScale, targetScale, elapsedTime / duration);
            transform.localScale = scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
