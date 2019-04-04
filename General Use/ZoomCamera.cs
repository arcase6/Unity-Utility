using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    
    public float minSize = 2f;
    
    public float maxSize = 30f;
    public float Sensitivity = 1000f;
    private Camera MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float size = MainCamera.orthographicSize;
        float offset = Input.GetAxis("Mouse ScrollWheel") * Sensitivity * Time.deltaTime;
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            offset *= 4;
        size -= offset;
        size = Mathf.Clamp(size,minSize,maxSize);
        MainCamera.orthographicSize = size;
    }
}
