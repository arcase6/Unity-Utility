using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameobject : MonoBehaviour
{


    public void Delete(GameObject objectToDelete){
        GameObject.Destroy(objectToDelete);
    }
}
