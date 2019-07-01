using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnDrop : DropCallback
{
    CanvasGroup group;
    private void Awake() {
        group = GetComponent<CanvasGroup>();
    }


    // Start is called before the first frame update
    public override void OnPlacement(GameObject placedObject)
    {
        group.alpha = 1;
        GameObject.Destroy(placedObject);
    }

    public override void OnTempPlacement(GameObject ghostData)
    {
        group.alpha = .5f;
        ghostData.SetActive(false);
    }

    public override void OnPointerExit()
    {
        group.alpha = 1;
    }
}
