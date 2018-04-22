using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMouseEvents : MonoBehaviour {

    // Hack to get mouse events to propagate up the hierarchy
    void OnMouseEnter() {
        var parentTC = GetComponentInParent<TileController>();
        if (parentTC) parentTC.OnMouseEnter();
    }

    void OnMouseOver() {
        var parentTC = GetComponentInParent<TileController>();
        if (parentTC) parentTC.OnMouseOver();
    }

    void OnMouseExit() {
        var parentTC = GetComponentInParent<TileController>();
        if (parentTC) parentTC.OnMouseExit();
    }
	
}
