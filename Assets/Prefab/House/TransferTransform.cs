using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TransferTransform : MonoBehaviour
{

    Transform from;
    public Transform to;
    private void Update()
    {
        from = this.transform;
       
        if(!from && !to)
        {
            Debug.Log("Fill transform");
        }
        else
        {
           // from.localScale = to.localScale;
            to.position = from.position;
            to.rotation = from.rotation;
           // DestroyImmediate(this);
        }
    }
}
