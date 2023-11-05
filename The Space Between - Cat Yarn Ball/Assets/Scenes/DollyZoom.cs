using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyZoom : MonoBehaviour
{
    public Transform target;

    Camera cam;
    float distance = 0f;
    float fov = 60;

    float viewWidth = 10f;


    public Camera mainCamera;
    public float transitionDuration = 2.0f;
    public CameraProjectionType startProjection = CameraProjectionType.Perspective;
    public CameraProjectionType endProjection = CameraProjectionType.Orthographic;

    public enum CameraProjectionType
    {
        Perspective,
        Orthographic
    }

    public bool isTransitioning = false;
    public float transitionStartTime;
    public float orthographicSize;
    public float fovSize = 60;
    public float minFovSize = 0.6f;
    public float zPos = -10;
    public float zPosPlus = -510;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 pos = target.transform.position;

        fov = cam.fieldOfView;
        distance = viewWidth / (2f * Mathf.Tan(0.5f * fov * Mathf.Deg2Rad));

        pos.z = -Mathf.Abs(distance);
        cam.transform.position = pos;


        if (Input.GetKeyDown(KeyCode.Q) && !isTransitioning)
        {
            StartCoroutine(TransitionCameraProjection());
        }
    }

    IEnumerator TransitionCameraProjection()
    {
        isTransitioning = true;
        transitionStartTime = Time.time;

        if (startProjection == CameraProjectionType.Orthographic)
        {
            mainCamera.orthographic = !mainCamera.orthographic;
        }

        while (Time.time - transitionStartTime < transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;

            if (startProjection == CameraProjectionType.Perspective && endProjection == CameraProjectionType.Orthographic)
            {
                cam.fieldOfView = Mathf.Lerp(fovSize, minFovSize, t);
            }
            else if (startProjection == CameraProjectionType.Orthographic && endProjection == CameraProjectionType.Perspective)
            {
                cam.fieldOfView = Mathf.Lerp(minFovSize, fovSize, t);
            }

            yield return null;
        }

        isTransitioning = false;

        // Switch the camera projection mode when the transition is complete
        if (startProjection == CameraProjectionType.Perspective)
        {
            mainCamera.orthographic = !mainCamera.orthographic;
        }

        CameraProjectionType temp = startProjection;
        startProjection = endProjection;
        endProjection = temp;

        mainCamera.fieldOfView = fovSize;
        mainCamera.transform.localPosition = Vector3.forward * zPos;


    }

}
