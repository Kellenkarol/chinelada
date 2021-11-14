using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedManager : MonoBehaviour
{

    ChinelaControle cc;

    void Start()
    {
        cc = GameObject.Find("ChinelaControle").GetComponent<ChinelaControle>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.tag == "Chinela")
    	{
    		// Do something
            cc.GameWin();
    	}
    }

}
