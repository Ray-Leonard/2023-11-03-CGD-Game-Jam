using System.Collections;
using UnityEngine;

public class CameraZoomTransition : MonoBehaviour
{
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
    public float minFovSize = 1;
    public float zPos = -10;
    public float zPosPlus = -510;

    public bool ManualSetting = false;

    [Range(0.0f, 1.0f)]
    public float slider = 0;


    public AnimationCurve aCurveFov;
    public AnimationCurve aCurve;

    void Start()
    {
        mainCamera = mainCamera != null ? mainCamera : Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No camera specified for the transition.");
            return;
        }

        if (startProjection == CameraProjectionType.Orthographic)
        {
            mainCamera.orthographic = true;
            orthographicSize = mainCamera.orthographicSize;
        }
        else
        {
            mainCamera.orthographic = false;
        }
    }

    void Update()
    {
        if (ManualSetting)
        {
            mainCamera.fieldOfView = Mathf.Lerp(fovSize, minFovSize, aCurveFov.Evaluate( slider));
            mainCamera.transform.localPosition = Vector3.Slerp(Vector3.forward * zPosPlus, Vector3.forward * zPos, aCurve.Evaluate(1 - slider));

            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isTransitioning)
        {
            StartCoroutine(TransitionCameraProjection());
        }
    }

    IEnumerator TransitionCameraProjection()
    {
        isTransitioning = true;
        transitionStartTime = Time.time;

        if(startProjection == CameraProjectionType.Orthographic)
        {
            mainCamera.orthographic = !mainCamera.orthographic;
        }

        while (Time.time - transitionStartTime < transitionDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionDuration;

            if (startProjection == CameraProjectionType.Perspective && endProjection == CameraProjectionType.Orthographic)
            {
                //mainCamera.orthographicSize = 5;// Mathf.Lerp(0, orthographicSize, t);
                mainCamera.fieldOfView = Mathf.Lerp(fovSize, minFovSize, t);
                //mainCamera.transform.localPosition = Vector3.forward *  Mathf.Lerp( zPos, zPosPlus, t); 
                //mainCamera.transform.localPosition = Vector3.Slerp(Vector3.forward * zPos, Vector3.forward * zPosPlus, aCurve.Evaluate( 1- t));
                mainCamera.transform.localPosition = Vector3.Slerp(Vector3.forward * zPosPlus, Vector3.forward * zPos, aCurve.Evaluate(1-t));

            }
            else if (startProjection == CameraProjectionType.Orthographic && endProjection == CameraProjectionType.Perspective)
            {
                //mainCamera.orthographicSize = 5;// Mathf.Lerp(orthographicSize, 0, t);
                mainCamera.fieldOfView = Mathf.Lerp(minFovSize, fovSize, t);
                mainCamera.transform.localPosition =  Vector3.Slerp(Vector3.forward * zPosPlus, Vector3.forward * zPos, aCurve.Evaluate(t)); //Vector3.forward * Mathf.le(zPosPlus, zPos, t);
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
