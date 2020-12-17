using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectiles : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
