using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
	Transform currentTransformInUse;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	currentTransformInUse = col.gameObject.transform;
    	if(col.gameObject.tag == "Chinela")
    	{
    		// col.gameObject.GetComponent<Chinela>().ResetChinela();
    		ChinelaControle.Instance.EndChinela();
    	}
    }
}
