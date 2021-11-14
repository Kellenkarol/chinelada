using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{

	public int Count;
	public Image image;

	public static TimeCount Instance;

    // Start is called before the first frame update
    void Start()
    {
    	Instance = this; // Use 'TimeCount.Instance' em qualquer script

    	if(!image)
    	{
	        // image = Resources.Load("Prefabs/ShootCountBlock.prefab", typeof(GameObject)) as GameObject;
    	}

    	if(!image)
    	{
    		GameObject aux = Resources.Load<GameObject>("Prefabs/TimeCountImage");

	        // GameObject aux = Resources.Load("Prefabs/TimeCountImage.prefab", typeof(GameObject)) as GameObject;
	        // GameObject aux = Resources.Load("Prefabs/TimeCountImage.prefab", typeof(GameObject)) as GameObject;
	        image = Instantiate(aux, transform).GetComponent<Image>();
    	}

    	StartCoroutine("StartCount");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartCount()
    {
    	float CountAux = Count;
    	while(CountAux > 0)
    	{
    		CountAux -= Time.deltaTime;
    		image.fillAmount = CountAux/Count;
    		yield return null;
    	}
    }


}
