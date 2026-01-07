using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCameraController : MonoBehaviour
{
    public float moveSpeed = 5f;      
    public float lookSensitivity = 2f; 
    private float rotationX = 0f;
    private float rotationY = 0f;

    public GameObject camera;
    
    private Vector2 moveInput;
    private Vector2 mouseInput;
    public bool Rotating;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotationX = rot.y;
        rotationY = rot.x;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        Move();
        if (Rotating)
        {
            Rotate(); 
        }
        //
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     GetComponent<PlayerInput>().SwitchCurrentActionMap("Battle");
        // }
        //
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     GetComponent<PlayerInput>().SwitchCurrentActionMap("StagingControl");
        // }
    }

    void Move()
    {
        Vector3 move =  new Vector3(moveInput.x, 0, moveInput.y).normalized * moveSpeed * Time.deltaTime;

        camera.transform.Translate(move, Space.World);
    }
    
    void Rotate()
    {
        Vector2 lookInput = mouseInput;

        rotationX += lookInput.x * lookSensitivity;
        rotationY -= lookInput.y * lookSensitivity;
        rotationY = Mathf.Clamp(rotationY, -80f, 80f);

        camera.transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
    }
    
    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    public void onLook(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }
    
    public void onRotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Rotating = true;
        }
        else
        {
            Rotating = false;
        }
    }
}