using UnityEngine;
using UnityEngine.InputSystem;

public class Interactions : MonoBehaviour
{
    private Vector2 mouseInput;

    private GameObject camera;
    
    private Vector3 aimPoint;   // World-space aim target
    private Vector3 fireDirection; // Direction for firing projectile
    private Color lineColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onLook(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }
    
    public void onClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        var control = context.control;

        if (control == Mouse.current.leftButton)
        {
            LeftClick(context);
        }
        else if (control == Mouse.current.rightButton)
        {
            RightClick(context);
        }
    }

    void LeftClick(InputAction.CallbackContext context)
    {
        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(mouseInput);

        // Default: aim at far plane in ray direction
        aimPoint = ray.origin + ray.direction * 10000;
        fireDirection = ray.direction;

        Debug.DrawRay(ray.origin, ray.direction * 10000, Color.red, 0.02f);
    }
    
    void RightClick(InputAction.CallbackContext context)
    {
        
    }
}
