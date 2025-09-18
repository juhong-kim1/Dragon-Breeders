using UnityEngine;

public class DragonTouch : MonoBehaviour
{
    private float perspectiveZoomSpeed = 0.1f;
    private Vector2 fingerTouchStartPosition;
    private bool isSwiping = false;
    private float minZoomSize = 20f;
    private float maxZoomSize = 80f;
    private float rotationSpeed = 5f;

    private float currentYRotation;

    private void Start()
    {
        currentYRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        CameraZoom();

        SwipeDragon();
    }

    private void CameraZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0Prev - touch1Prev).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;


            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


            Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoomSize, maxZoomSize);

        }
    }

    private void SwipeDragon()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    fingerTouchStartPosition = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    if (isSwiping)
                    {
                        Vector2 deltaPosition = touch.position - fingerTouchStartPosition;
                        float rotationDelta = deltaPosition.x * rotationSpeed * Time.deltaTime;
                        currentYRotation -= rotationDelta;
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentYRotation, transform.eulerAngles.z);

                        fingerTouchStartPosition = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                    isSwiping = false;
                    break;
            }


        }

    }

}

