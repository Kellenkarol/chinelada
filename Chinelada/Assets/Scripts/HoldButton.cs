using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

	[HideInInspector] public bool isPressed;
	// private ChinelaControle CC;
	private Image fillFromGas;

    // Start is called before the first frame update
    void Start()
    {
        // CC = ChinelaControle.Instance;
        
        // CC = GameObject.Find("ChinelaControle").GetComponent<ChinelaControle>();

		if(gameObject.name == "GasButton")
		{
			fillFromGas = transform.GetChild(0).GetComponent<Image>();
		}
    }


    // Update is called once per frame
    void Update()
    {

    	if(isPressed)
    	{
			if(gameObject.name == "Up")
			{
				// print("Up");
				ChinelaControle.Instance.Up();
			}
			else if(gameObject.name == "Down")
			{
				// print("Down");
				ChinelaControle.Instance.Down();
			}
			else if(gameObject.name == "GasButton")
			{
				ChinelaControle.Instance.UseGas(fillFromGas);
			}
    	}
        
    }


	public void OnPointerDown(PointerEventData eventData)
	{
		isPressed = true;
	}
	

	public void OnPointerUp(PointerEventData eventData)
	{
		isPressed = false;
	}



}
