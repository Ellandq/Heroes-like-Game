using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisablePathTile : MonoBehaviour
{
    void OnTriggerEnter (Collider collider)
    {
        if (collider.gameObject.tag == "WorldObject"){
            if (collider.GetComponentInParent<WorldObject>() is Army) Destroy(this.gameObject);
        }
    }
}
