using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform followCharacter = null;
    [SerializeField] float followSpeed = 0.2f;
    [SerializeField] float maxCameraDifferent = 2f;
    [SerializeField] float mouseFollowSpeed = 1f;
 
    private Vector3 cameraDistance;
    private CameraInput mouseInput;
    private Camera cam;
    private Vector3 lastDelta, targetPos;

    private void Awake()
    {
        mouseInput = new CameraInput();
        cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        mouseInput.Enable();
    }

    private void OnDisable()
    {
        mouseInput.Disable();
    }

    private void Start()
    {
        cameraDistance = new Vector3(transform.position.x - followCharacter.position.x,
                                      transform.position.y - followCharacter.position.y,
                                       transform.position.z - followCharacter.position.z);
    }

    private void Update()
    {
        // Get mouse position Input
        Vector2 mousePositionValue = mouseInput.Camera.MousePosition.ReadValue<Vector2>();

        Vector3 mouseWorldPosition = GetWorldPosition(new Vector2(mousePositionValue.x - Screen.width /2f, mousePositionValue.y - Screen.height / 2f));
        Vector3 screenCenterWorldPosition = GetWorldPosition(new Vector2(0.5f, 0.5f));
        Vector3 delta = mouseWorldPosition - screenCenterWorldPosition;
        

        // limitation
        delta = Vector3.ClampMagnitude(delta, maxCameraDifferent);

        // lerp delta
        lastDelta = Vector3.MoveTowards(lastDelta, delta, mouseFollowSpeed * Time.deltaTime);

        // Follow Target GameObject
        targetPos = lastDelta + followCharacter.position;

        // Set Camera
        transform.DOMove(new Vector3(targetPos.x + cameraDistance.x, transform.position.y, targetPos.z + cameraDistance.z), followSpeed, false);
    }
    
    private Vector3 GetWorldPosition(Vector2 mouseValue)
    {
        Ray camRay = cam.ScreenPointToRay(mouseValue);
        float t = (followCharacter.position.y - camRay.origin.y) / camRay.direction.y;
        return camRay.origin + camRay.direction * t;
    }

    public Vector3 GetMousePositionInWorld()
    {
        // Get mouse position Input
        Vector2 mousePositionValue = mouseInput.Camera.MousePosition.ReadValue<Vector2>();

        Vector3 mouseWorldPosition = GetWorldPosition(new Vector2(mousePositionValue.x - Screen.width / 2f, mousePositionValue.y - Screen.height / 2f));
        Vector3 screenCenterWorldPosition = GetWorldPosition(new Vector2(0.5f, 0.5f));
        Vector3 delta = mouseWorldPosition - screenCenterWorldPosition;

        return delta + followCharacter.position ;
    }

    public Vector2 GetMousePosition()
    {
        return mouseInput.Camera.MousePosition.ReadValue<Vector2>();
    }

    public Ray MousePositionPointToRay()
    {
        return cam.ScreenPointToRay(mouseInput.Camera.MousePosition.ReadValue<Vector2>());
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
