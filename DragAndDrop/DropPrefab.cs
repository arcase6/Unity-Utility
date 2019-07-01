using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropPrefab : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject GhostData;
    private Camera mainCamera;
    public float PreviewAlphaValue = .5f;

    public DropPrefabFilter filter;
    public GameObject DropDestination = null;

    public int MaxChildren = 0;
    
    public bool IsListHorizontal = true;

    private DropCallback[] CallbackHandlers;
    


    private void Reset()
    {
        filter = GetComponent<DropPrefabFilter>();
        DropDestination = gameObject;
        MaxChildren = 0;
    }

    private void Awake()
    {
        CallbackHandlers = GetComponents<DropCallback>();
        mainCamera = Camera.main;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        DropPrefab[] children = GetComponentsInChildren<DropPrefab>();
        foreach (DropPrefab child in children)
        {
            if (child.GhostData != null)
                return;
        }

        //Debug.Log(name);
        //add some sort of ghost data
        GameObject dragObject = GetDragObject(eventData);
        if (dragObject == null) return;
        GhostData = GameObject.Instantiate(dragObject, DropDestination.transform);
        CanvasGroup group = GhostData.GetComponent<CanvasGroup>();
        if (group == null) group = GhostData.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;
        group.alpha = PreviewAlphaValue;
        foreach (DropCallback handler in CallbackHandlers)
        {
            handler.OnTempPlacement(GhostData);
        }


        if (DropDestination.GetComponent<HorizontalLayoutGroup>() || DropDestination.GetComponent<VerticalLayoutGroup>())
        {
            //StartCoroutine(InvokeNextFrame(UpdateHorizontalLayout));
            DeactivateContentSizeFitter(); //content size fitters should only be used when not being controlled by a parent layout
        }


        //position the copied element correctly
        int childIndex = GetChildIndex(eventData);
        if(childIndex < DropDestination.transform.childCount){
            GhostData.transform.SetSiblingIndex(childIndex);
        }

        DropPrefab parentDP = transform.parent.GetComponent<DropPrefab>();
        if (parentDP != null)
        {
            parentDP.OnPointerExit(eventData);
        }
    }


    private void DeactivateContentSizeFitter()
    {
        ContentSizeFitter fitter = GhostData.GetComponent<ContentSizeFitter>();
        fitter.enabled = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GhostData == null)
        {
            DropPrefab parentDP = GameobjectHelper.FindInParents<DropPrefab>(this.transform.parent?.gameObject);
            //if(parentDP != null) parentDP.OnDrop(eventData);
            return;
        }
        Destroy(GhostData.GetComponent<CanvasGroup>());
        foreach (DropCallback handler in CallbackHandlers)
        {
            handler.OnPlacement(GhostData);
        }
        GhostData = null;
        DragPrefab source = eventData.pointerDrag.GetComponent<DragPrefab>();
        if(source.DestroyOnDrop){
            source.DropFinishedFlag = true;
            source.OnEndDrag(eventData);
        }
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        if (GhostData == null) return;
        Destroy(GhostData);
        GhostData = null;

        foreach (DropCallback handler in CallbackHandlers)
        {
            handler.OnPointerExit();
        }

        DropPrefab parentDP = transform.parent.GetComponent<DropPrefab>();
        if (parentDP != null)
        {
            RectTransform parentTransform = parentDP.transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(parentTransform,eventData.position,mainCamera))
                parentDP.OnPointerEnter(eventData);
        }
    }


    public GameObject GetDragObject(PointerEventData eventData)
    {
        //pointerDrag gets the gameobject that is being dragged (a.k.a the dragme object)
        GameObject originalObj = eventData.pointerDrag;
        if (originalObj == null)
            return null;

        DragPrefab dragPrefab = originalObj.GetComponent<DragPrefab>();
        if (dragPrefab == null)
            return null;

        if (this.filter && !this.filter.isAllowed(dragPrefab?.IdentifierName))
            return null;
        if (IsChildLimitReached(dragPrefab))
            return null;
        return dragPrefab.DragObject; //get the object being dragged around
    }


    public bool IsChildLimitReached(DragPrefab dragPrefab)
    {
        if (this.MaxChildren <= 0 || this.DropDestination.transform.childCount < MaxChildren)
            return false;

        if (dragPrefab.transform.parent.gameObject == this.DropDestination) //case where reordering is being done
            return false;
        return true;

    }


    public void UpdateHorizontalLayout()
    {
        ApplyShiftToLayoutGroup(.001f);
        StartCoroutine(InvokeNextFrame(() => ApplyShiftToLayoutGroup(-.001f)));
    }

    private void ApplyShiftToLayoutGroup(float shiftAmmount)
    {
        LayoutGroup group = DropDestination.GetComponent<HorizontalLayoutGroup>();
        if (group)
            ((HorizontalLayoutGroup)group).spacing += shiftAmmount;
        group = DropDestination.GetComponent<VerticalLayoutGroup>();
        if (group)
            ((VerticalLayoutGroup)group).spacing += shiftAmmount;
    }

    public IEnumerator InvokeNextFrame(System.Action action)
    {

        yield return null;
        action();
    }



    public int GetChildIndex(PointerEventData eventData){
        if(DropDestination.transform.childCount == 0)
            return 0;
        Vector2 mousePosition = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(DropDestination.transform as RectTransform,eventData.position,mainCamera,out mousePosition);
        float mousePosition1D = IsListHorizontal ? mousePosition.x : mousePosition.y;

        for(int i =0; i < DropDestination.transform.childCount; i++){
            RectTransform childTransform = DropDestination.transform.GetChild(i) as RectTransform;
            bool greaterThanPrevious;
            if(IsListHorizontal){
                float midPoint = childTransform.localPosition.x + (childTransform.sizeDelta.x / 2f);
                greaterThanPrevious = mousePosition1D > midPoint;
            }
            else{
                float midPoint = childTransform.localPosition.y - (childTransform.sizeDelta.y / 2f);
                greaterThanPrevious = mousePosition1D < midPoint;
            }
            if(greaterThanPrevious)
                continue;
            else
                return i;
                
        }

        
        return DropDestination.transform.childCount;

    }
}
