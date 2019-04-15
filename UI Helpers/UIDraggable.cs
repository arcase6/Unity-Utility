using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDraggable : BindingSourceMonobehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 StartingPosition;
    public FloatVariable DiscreteStep;
    public bool UseX = true;

    public float BaseDragSpeed = .01f;
    public float ShiftModifier = 5f;
    public bool ShiftModifierChanged = false;

    Plane DragPlane;

    float UIValue;
    float UIValueOnClick;
    private bool IsDragging = false;

    public override BindingMode PrefferedMode => BindingMode.OffsetFromSource;

    public override bool LockBindingMode => true;

    #region IBindingSource overrides
    public override int getValueInteger()
    {
        throw new System.InvalidOperationException();
    }

    public override float getValueFloat()
    {
        return UIValue;
    }

    public override double getValueDouble()
    {
        throw new System.InvalidOperationException();
    }

    public override string getValueString()
    {
        return UIValue.ToString("0.##");
    }

    public override void setFromValueString(string valueString)
    {
        UIValue = float.Parse(valueString);
    }
    
    #endregion
    
    #region unity magic methods
      public void OnBeginDrag(PointerEventData eventData)
    {
        //not syncing with the field starting value
        StartingPosition = eventData.position;
        UIValueOnClick = UIValue;
        IsDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ShiftModifierChanged)
        {
            OnBeginDrag(eventData);
            ShiftModifierChanged = false;
        }
        float UIValueOnDragStart = UIValue;
        Vector2 OffsetVector = eventData.position - StartingPosition;
        float dragSpeed = BaseDragSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            dragSpeed *= ShiftModifier;
        OffsetVector = OffsetVector * dragSpeed;
        if (UseX)
            UIValue = OffsetVector.x + UIValueOnClick;
        else
            UIValue = OffsetVector.y + UIValueOnClick;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            UIValue = (float)Mathf.RoundToInt(UIValue / DiscreteStep.Value) * DiscreteStep.Value;
        }
        if (UIValueOnDragStart != UIValue)
            NotifyChange();

    }



    private void Update()
    {
        if (IsDragging)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                ShiftModifierChanged = true;
            else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                ShiftModifierChanged = true;
        }
    }

    private void Awake()
    {
        this.SourceType = VariableType.Float;
    }

    public override decimal getValueDecimal()
    {
        throw new System.NotImplementedException();
    }

    public override bool getValueBoolean()
    {
        throw new System.InvalidOperationException();
    }

    public override Vector4 getValueVector4()
    {
        throw new System.InvalidOperationException();
    }

    public override Vector3 getValueVector3()
    {
        throw new System.InvalidOperationException();
    }

    public override Vector2 getValueVector2()
    {
        throw new System.InvalidOperationException();
    }

    public override Vector3Int getValueVector3Int()
    {
        throw new System.InvalidOperationException();
    }

    public override Vector2Int getValueVector2Int()
    {
        throw new System.InvalidOperationException();
    }

    public override object getValueAsObject()
    {
        return this.getValueFloat();
    }

    public override void setFromObject(object value)
    {
        float f = VariableUtilities.getValueFloat(value);
        UIValue = f;
    }

    #endregion
}
