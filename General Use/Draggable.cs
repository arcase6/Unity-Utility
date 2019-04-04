using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Draggable : MonoBehaviour
{
    public FloatVariable DiscreteStep;
    public static bool dragSemaphore = true;
    public bool isDragging = false;
    public bool DragX = true;
    public bool DragY = true;
    Vector3 CastVector;

    public Transform TransformToDrag;

    public UnityEvent BeginDrag;

    private Camera MainCamera;
    Plane DragPlane;
    Vector3 offsetFromMouse;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
    }

    
    public Vector3 calculateMousePosition(){
        Ray cameraToMouseRay = MainCamera.ScreenPointToRay(Input.mousePosition);
        float distanceAlongRay = -1f;
        if(DragPlane.Raycast(cameraToMouseRay,out distanceAlongRay)){
            //Vector3 worldPosition = cameraToMouseRay.origin + cameraToMouseRay.direction * distanceAlongRay;
            Vector3 worldPosition = cameraToMouseRay.GetPoint(distanceAlongRay);
            if( (!DragX || !DragY) && (DragX || DragY))
                worldPosition = Vector3.Project(worldPosition,CastVector);
            return worldPosition;
        }
        
        return TransformToDrag.position - offsetFromMouse; //fallback value (should not hit under normal circumstances)
    }

    private void OnMouseDown()
    {
        if (!dragSemaphore)
            return;
        dragSemaphore = false;
        isDragging = true;
        InitializeStartingValues();
        BeginDrag?.Invoke();
    }

    private void InitializeStartingValues()
    {
        TransformToDrag = TransformToDrag == null ? transform : TransformToDrag;
        DragPlane = new Plane(MainCamera.transform.forward * -1, TransformToDrag.position);
        CastVector = DragY ? MainCamera.transform.up : MainCamera.transform.right;

        Vector3 mousePosition = calculateMousePosition();
        offsetFromMouse = TransformToDrag.position - mousePosition;
    }

    private void OnMouseDrag() {
        if(!isDragging)
            return; //object was clicked but another object is being dragged
        Vector3 mousePosition = calculateMousePosition();
        Vector3 newPosition = mousePosition + offsetFromMouse;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) && DiscreteStep != null){
            newPosition.x = MakeDiscrete(newPosition.x);
            newPosition.y = MakeDiscrete(newPosition.y);
            newPosition.z = MakeDiscrete(newPosition.z);
        }
        TransformToDrag.position = newPosition;
        
    }

    private float MakeDiscrete(float continuousValue)
    {
        return Mathf.RoundToInt(continuousValue / DiscreteStep.Value) * DiscreteStep.Value;

    }

    private void OnMouseUp() {
        if(isDragging)
            dragSemaphore = true;
        isDragging = false;
    }
}
