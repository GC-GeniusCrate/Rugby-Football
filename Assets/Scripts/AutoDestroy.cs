using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    float destroyTime = .5f;
    private void Awake()
    {
        Destroy(this.gameObject, destroyTime);
    }
}
