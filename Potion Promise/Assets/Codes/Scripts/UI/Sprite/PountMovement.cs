using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PountMovement : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float minYPosition = 0f; // Minimum Y boundary
    [SerializeField] private float maxYPosition = 2f; // Maximum Y boundary
    [SerializeField] private float fixedXPosition = 0f; // Fixed X position to lock horizontal movement

    [SerializeField] private MortarHandler mortarHandler;

    private Vector3 offset;
    private bool isDragging = false;
    private float zDistanceToCamera;
    private bool directionChanged = false;
    private bool movedDown = false;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        // Cache the initial Z distance from the camera
        zDistanceToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        mortarHandler.PlayerEventSO.Event.OnSmashedMaterialDragging += HandleSmashedMaterialDragging;
    }

    private void OnDisable()
    {
        mortarHandler.PlayerEventSO.Event.OnSmashedMaterialDragging -= HandleSmashedMaterialDragging;

    }

    private void Update()
    {
        if (isDragging)
        {
            DragObject();
        }
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseUp()
    {
        StopDragging();
    }

    private void HandleSmashedMaterialDragging(Vector3 smashedMaterialPosition)
    {
        float distanceBetween = Vector3.Distance(transform.position, smashedMaterialPosition) / 5;
        spriteRenderer.DOFade(distanceBetween - 0.3f, 0);
    }

    /// <summary>
    /// Initiates dragging by calculating the offset.
    /// </summary>
    private void StartDragging()
    {
        Vector3 worldMousePosition = GetWorldMousePosition();
        offset = transform.position - worldMousePosition;
        isDragging = true;
    }

    /// <summary>
    /// Stops dragging.
    /// </summary>
    private void StopDragging()
    {
        isDragging = false;
    }

    /// <summary>
    /// Handles the dragging behavior, clamping movement to specified boundaries.
    /// </summary>
    private void DragObject()
    {
        Vector3 worldMousePosition = GetWorldMousePosition();
        Vector3 desiredPosition = worldMousePosition + offset;

        // Clamp the Y position and fix the X position
        desiredPosition.x = fixedXPosition; // Lock X position to the specified value
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minYPosition, maxYPosition);

        // Smoothly move the object to the desired position
        transform.position = desiredPosition;

        HandlePoundSmashMovement(desiredPosition.y);
    }

    private void HandlePoundSmashMovement(float desiredPosition)
    {
        if (desiredPosition <= minYPosition && !directionChanged)
        {
            directionChanged = true;

            if (movedDown)
            {
                mortarHandler.SmashMaterial();
                movedDown = false;
            }
        }
        else if (desiredPosition >= maxYPosition && directionChanged)
        {
            directionChanged = false;
            movedDown = true;
        }
    }

    /// <summary>
    /// Gets the mouse position in world space.
    /// </summary>
    /// <returns>Mouse position in world coordinates.</returns>
    private Vector3 GetWorldMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zDistanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
