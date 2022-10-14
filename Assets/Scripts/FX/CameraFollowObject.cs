using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public GameObject gameObjectToFollow = null;
    public float cameraZoom = 0;
    public float zoomSensitivity = 10f;
    public float CAMERA_ZOOM_MIN = 3f;
    public float CAMERA_ZOOM_MAX = 8f;
    private bool isCameraMovingTowardsTarget = false;
    private Vector3 movementStartPos;
    private Vector3 movementEndPos;
    private float animationTime = 0f;
    private float animationSpeed = 1.5f;
    private GameObject rocket;
    private GameObject probe;
    
    void Start()
    {
        if (cameraZoom == 0)
        {
            cameraZoom = 5;
        }
        gameObject.GetComponent<Camera>().orthographicSize = cameraZoom;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            ChangeCameraZoom(Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity);
        }

        if (isCameraMovingTowardsTarget)
        {
            MoveCameraTowardsTarget();
            return;
        }
        if (gameObjectToFollow == null) return;
        rocket = GameObject.FindGameObjectWithTag("Rocket");
        probe = GameObject.FindGameObjectWithTag("Probe");
        if (rocket != null)
        {
            gameObject.transform.position = rocket.transform.position + new Vector3(0f, 0f, -10f);
        }
        if (probe != null)
        {
            gameObject.transform.position = probe.transform.position + new Vector3(0f, 0f, -10f);
        }
        if (rocket == null && probe == null)
        {
            gameObject.transform.position = gameObjectToFollow.transform.position + new Vector3(0f, 0f, -10f);
        }
    }

    public void NewObjectToFollow(GameObject obj, Vector3 startPosition)
    {
        Vector3 flatObjectPosition = new Vector3(obj.transform.position.x,
            obj.transform.position.y, -10f);
        float mag1 = Vector3.Magnitude(startPosition);
        float mag2 = Vector3.Magnitude(flatObjectPosition);

        if (Mathf.Abs(mag1 - mag2) < 1f) return;

        if (startPosition == obj.transform.position) return;
        isCameraMovingTowardsTarget = true;
        gameObjectToFollow = obj;
        movementStartPos = startPosition;
    }

    void MoveCameraTowardsTarget()
    {
        GameObject.Find("PS_Planet_Earth").GetComponent<Controller>().canPlay = false;
        Vector3 endPosition = new Vector3(gameObjectToFollow.transform.position.x,
            gameObjectToFollow.transform.position.y, -10f);
        transform.position = Vector3.Lerp(movementStartPos,
            endPosition,
            animationTime);
        animationTime += animationSpeed * Time.deltaTime;
        if (animationTime >= 1f)
        {
            GameObject.Find("PS_Planet_Earth").GetComponent<Controller>().canPlay = true;
            animationTime = 0f;
            isCameraMovingTowardsTarget = false;
        }
    }

    void ChangeCameraZoom(float cameraZoomDelta)
    {
        if (!GameObject.Find("PS_Planet_Earth").GetComponent<Controller>().canPlay) return;
        float newCameraZoom = GetComponent<Camera>().orthographicSize += cameraZoomDelta;
        if (newCameraZoom < CAMERA_ZOOM_MIN) newCameraZoom = CAMERA_ZOOM_MIN;
        if (newCameraZoom > CAMERA_ZOOM_MAX) newCameraZoom = CAMERA_ZOOM_MAX;
        GetComponent<Camera>().orthographicSize = newCameraZoom;
    }
}
