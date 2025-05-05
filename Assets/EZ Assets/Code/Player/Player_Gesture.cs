using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Gesture : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private InputAction touchPress;
    private InputAction touchPosition;
    private InputAction doubleTouch;
    private InputAction longTouch;

    private Vector2 startTouchPos;
    private Vector2 currentTouchPos;
    private Vector2 joystickOutput;
    [SerializeField] private bool swipeDetected = false;
    [SerializeField] private bool isTouching = false;
    [SerializeField] private float threshold = 100f;
    [SerializeField] private bool isHoldingLong = false;

    void Awake()
    {
        touchPress = inputActions.FindAction("TouchPress");
        touchPosition = inputActions.FindAction("TouchPosition");
        doubleTouch = inputActions.FindAction("DoubleTouch");
        longTouch = inputActions.FindAction("LongTouch");

        touchPress?.Enable();
        touchPosition?.Enable();
        doubleTouch?.Enable();
        longTouch?.Enable();

        touchPress.performed += ctx => StartTouch();
        touchPress.canceled += ctx => EndTouch();

        longTouch.started += ctx =>
        {
            isHoldingLong = false;
        };
        longTouch.performed += ctx =>
        {
            isHoldingLong = true;
        };
        longTouch.canceled += ctx =>
        {
            isHoldingLong = false;
        };
    }

    void StartTouch()
    {
        isTouching = true;
        startTouchPos = touchPosition.ReadValue<Vector2>();
    }

    void EndTouch()
    {
        isTouching = false;
        joystickOutput = Vector2.zero;
        swipeDetected = false;
    }

    void OnDisable()
    {
        touchPress?.Disable();
        touchPosition?.Disable();
        doubleTouch?.Disable();
        longTouch?.Disable();
    }

    public void OnDisableDoubleTouch()
    {
        doubleTouch?.Disable();
    }
    public void OnDisableLongTouch()
    {
        longTouch?.Disable();
    }
    public void OnEnableDoubleTouch()
    {
        doubleTouch?.Enable();
    }
    public void OnEnableLongTouch()
    {
        longTouch?.Enable();
    }
    public void OnDisableTouchPress()
    {
        touchPress?.Disable();
    }
    public void OnEnableTouchPress()
    {
        touchPress?.Enable();
    }
    public void OnDisableMoveControle()
    {
        touchPosition?.Disable();
    }
    public void OnEnableMoveControle()
    {
        touchPosition?.Enable();
    }
    void Update()
    {
        if (isTouching)
        {
            currentTouchPos = touchPosition.ReadValue<Vector2>();
            Vector2 rawDelta = currentTouchPos - startTouchPos;

            if (float.IsNaN(rawDelta.x) || float.IsNaN(rawDelta.y) || rawDelta.magnitude < threshold)
            {
                joystickOutput = Vector2.zero;
                return;
            }

            float maxRadius = 100f;
            joystickOutput = Vector2.ClampMagnitude(rawDelta / maxRadius, 1f);
        }
        else
        {
            joystickOutput = Vector2.zero;
        }

    }

    public Vector2 MoveControle()
    {
        return joystickOutput;
    }

    public bool IsTouching()
    {
        return isTouching;
    }

    public bool IsSwipeUp()
    {
        if (swipeDetected) return false;

        Vector2 delta = currentTouchPos - startTouchPos;
        if (delta.y > threshold && Mathf.Abs(delta.x) < threshold)
        {
            Debug.Log("Swipe Up");
            swipeDetected = true;
            return true;
        }
        return false;
    }
    public bool IsSwipeDown()
    {
        if (swipeDetected) return false;

        Vector2 delta = currentTouchPos - startTouchPos;
        if (delta.y < -threshold && Mathf.Abs(delta.x) < threshold)
        {
            Debug.Log("Swipe Down");
            swipeDetected = true;
            return true;
        }
        return false;
    }
    public bool IsSwipeLeft()
    {
        if (swipeDetected) return false;

        Vector2 delta = currentTouchPos - startTouchPos;
        if (delta.x < -threshold && Mathf.Abs(delta.y) < threshold)
        {
            Debug.Log("Swipe Left");
            swipeDetected = true;
            return true;
        }
        return false;
    }
    public bool IsSwipeRight()
    {
        if (swipeDetected) return false;

        Vector2 delta = currentTouchPos - startTouchPos;
        if (delta.x > threshold && Mathf.Abs(delta.y) < threshold)
        {
            Debug.Log("Swipe Right");
            swipeDetected = true;
            return true;
        }
        return false;


    }
    public bool IsDoubleTap()
    {
        if (doubleTouch.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsLongPress()
    {

        return isHoldingLong;
    }
}