using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector2> OnMove = new();
    public UnityEvent OnJump = new();
    public UnityEvent OnDash = new();
    public UnityEvent OnAttack = new();
    public UnityEvent OnSettingsMenu = new();
    public bool isMovementDisabled = false; // Flag to disable movement
    private bool isSettingMenuOpen = false; // Flag to check if the settings menu is open

    void Start()
    {
        
    }

    void Update()
    {
        if (isMovementDisabled) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSettingMenuOpen = !isSettingMenuOpen; // Toggle the settings menu state
            OnSettingsMenu?.Invoke();
            Debug.Log("Escape pressed");
        }
        if (isSettingMenuOpen) return; // Ignore input if settings menu is open

        if (isMovementDisabled) return; // Ignore input if movement is disabled
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            input += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input += Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDash?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnAttack?.Invoke();
        }
        
        // we can access the game manager through the singleton instance
        // and then access the public read-only bool
        // which reflects the state of the game
        OnMove?.Invoke(input.normalized);
    }
}
