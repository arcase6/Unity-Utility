using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
    //public float panSpeed = 1250.0f;
    public Vector2 panSpeed = new Vector2(215f,125f);

    public float panModifier = 1f;

    public BooleanVariable isDraggingObject;
    private Vector3 mouseOrigin;

    private Camera MainCamera;


    void Start()
    {
        MainCamera = GetComponent<Camera>();
        panSpeed = panSpeed * panModifier;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(isDraggingObject.Value)
            return;
        if (Input.GetMouseButtonDown(1))
        {
            mouseOrigin = Input.mousePosition;
        }
        if (!Input.GetMouseButton(1))
        {
            return;
        }
        Vector3 offset = MainCamera.ScreenToViewportPoint(Input.mousePosition - mouseOrigin) * Time.deltaTime * -1;
        //offset = new Vector3(offset.x, offset.y, 0) * -1;
        offset = MainCamera.transform.right * offset.x * panSpeed.x + MainCamera.transform.up * offset.y * panSpeed.y;
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
            //Debug.Log("shift");
            offset *= 4;
        }
        offset *= MainCamera.orthographicSize;
        mouseOrigin = Input.mousePosition;
        Camera.main.transform.Translate(offset, Space.World);

    }

}
