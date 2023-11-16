using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;

public class CameraController : MonoBehaviour {
    private float zoom;
    private float zoomMultiplier = 10f;
    private float minZoom = 2f;
    private float maxZoom = 100f;
    public float velocity = 2f;
    public float smoothTime = 0.25f;

    [SerializeField] private Camera cam;

    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;

    private bool drag = false;




    // Start is called before the first frame update
    void Start()
    {
        zoom = cam.orthographicSize;
        ResetCamera = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
    }

    void HandleMovementInput() {
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);
       

    }

    private void LateUpdate() {
        if (Input.GetMouseButton(0)) {
            Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (drag == false) {
                drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

        } else {
            drag = false;
        }

        if (drag) {
            Camera.main.transform.position = Origin - Difference * 0.5f;
        }

        if (Input.GetMouseButton(1))
            Camera.main.transform.position = ResetCamera;

    }






}
