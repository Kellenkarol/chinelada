using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShotsCount : MonoBehaviour
{

	public int MaxShots;

	public GameObject image, imageEmpty;

	public static ShotsCount Instance;
	
	[HideInInspector]
	public int Shots;

    // Start is called before the first frame update
    void Start()
    {
    	Instance = this; // Use 'ShotCount.Instance' em qualquer script

    	if(!image)
    	{
    		image = Resources.Load<GameObject>("Prefabs/ShotCountBlock");
	        // image = Resources.Load("Prefabs/ShotCountBlock.prefab", typeof(GameObject)) as GameObject;
    	}
    	if(!imageEmpty)
    	{
    		imageEmpty = Resources.Load<GameObject>("Prefabs/ShotCountBlockEmpty");
	        // imageEmpty = Resources.Load("Prefabs/ShotCountBlockEmpty.prefab", typeof(GameObject)) as GameObject;
    	}

    	_Instantiate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _Instantiate()
    {
    	for(int c=0; c<MaxShots; c++)
    	{
            // print("_Instantiate:   "+c);
            // Instantiate(image);
    		Instantiate(image, transform.GetChild(0));
    	}
    }


    public void RemoveLast()
    {	
    	if(Shots < MaxShots)
    	{
    		Transform _ = transform.GetChild(0).GetChild(0).transform;
    		_.SetParent(null);
	    	Destroy(_.gameObject);
			Instantiate(imageEmpty, transform.GetChild(0));
			Shots++;
			print("Removed");
    	}
    }

    public int RemainingShots()
    {
        return MaxShots - Shots;
    }

    public void Reset()
    {

    	// print("transform.GetChild(0).childCount = "+transform.GetChild(0).childCount);
		// Destroy(transform.GetChild(0).GetChild(0).gameObject);
    	// print("transform.GetChild(0).childCount = "+transform.GetChild(0).childCount);
    	while(transform.GetChild(0).childCount != 0)
    	{
			Transform _ = transform.GetChild(0).GetChild(0).transform;
			_.SetParent(null);
	    	Destroy(_.gameObject);
    	}	
    }

}
