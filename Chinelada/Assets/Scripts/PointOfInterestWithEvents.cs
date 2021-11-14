using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestWithEvents : MonoBehaviour
{

	public static event Action<string> OnPointOfInterestEntered;

	[SerializeField] private string _poiName;

	public string PoiName { get { return _poiName; } }

    private void OnTriggerEnter(Collider col)
    {
    	if(OnPointOfInterestEntered != null)
    		OnPointOfInterestEntered(this._poiName);
    }

}
