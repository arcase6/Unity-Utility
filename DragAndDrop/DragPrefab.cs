using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public enum MouseButton{
    Left = -1,
    Right = -2,
    Middle = -3
}

public class DragPrefab : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string IdentifierName;
    
    public GameObject PrefabToDrag;
    public PrefabInitializer prefabInit;

	public bool DragOnSurfaces = true;
    public MouseButton DragButton = MouseButton.Left;
    

    [HideInInspector]
    public GameObject DragObject;
    private RectTransform DragPlane;

    public bool DestroyOnDrop = false;

    public bool DropFinishedFlag = false;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.pointerId != (int)DragButton)
            return;
        var canvas = gameObject.FindInParents<Canvas>();
		if (canvas == null)
			return;

        this.DragObject = GameObject.Instantiate(PrefabToDrag,canvas.transform);
        ContentSizeFitter fitter = DragObject.GetComponent<ContentSizeFitter>();
        if(fitter)
            fitter.enabled = true;
        this.DragObject.name = PrefabToDrag.name;
        if(prefabInit)
            prefabInit.Process(this.DragObject);
        
		DragObject.transform.SetAsLastSibling();
        var group = DragObject.AddComponent<CanvasGroup>();
		group.blocksRaycasts = false;

        if(DragOnSurfaces)
            DragPlane = transform as RectTransform;
        else
            DragPlane = canvas.transform as RectTransform;

        SetDraggedPosition(eventData);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.pointerId == (int)DragButton && DragObject != null)
            SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.pointerId == (int)DragButton && DragObject != null)
            Destroy(DragObject);
        DragObject = null;

        if(DestroyOnDrop && DropFinishedFlag)
            Destroy(this.gameObject);
    }

    private void SetDraggedPosition(PointerEventData eventData)
	{
		//only executed conditionally when the pointer enters the rect area of a new selectable object
		//pointerEnter is a reference to the gameObject that was entered
		if (DragOnSurfaces && eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
			DragPlane = eventData.pointerEnter.transform as RectTransform;
		
		var rt = DragObject.GetComponent<RectTransform>();
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(DragPlane, eventData.position, eventData.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;
			rt.rotation = DragPlane.rotation;
		}
	}


}
