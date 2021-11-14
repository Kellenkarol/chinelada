using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Mathf;
using UnityEngine.UI;

public class ShowWhenOut : MonoBehaviour
{
	public float SafeArea = 1, distaceMultiply=2;
	private GameObject group, indicator;
	private ChinelaControle CC;
    private Vector3 wordPosMax, chinelaPos;
    private RectTransform rect;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        group 		= transform.GetChild(0).gameObject;
        rect 		= group.GetComponent<RectTransform>();
        CC 			= ChinelaControle.Instance;
        text 		= group.transform.GetChild(1).GetChild(0).GetComponent<Text>(); 
        indicator   = group.transform.GetChild(0).gameObject; 
    }


    // Update is called once per frame
    void Update()
    {
    	UpdateVariables();
    	if(!IsOutBottom())
    	{
			SetPosition();
			// LookAt();
			ShowDistance();
    	}
    	else
    	{
			group.SetActive(false);
    	}
    }


    private void UpdateVariables()
    {
    	wordPosMax 	= Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
    	chinelaPos 	= CC.GetChinelaPosition();
    }


    private float CalculateDistance()
    {
    	return Mathf.Pow(Mathf.Pow((chinelaPos.x-indicator.transform.position.x),2) + 
    		   Mathf.Pow((chinelaPos.y-indicator.transform.position.y),2), 0.5f); 
    }


    private void ShowDistance()
    {
    	text.text = ((int)(CalculateDistance()*distaceMultiply))+"m";
    }


    private bool IsOutUp()
    {
    	// UpdateVariables();
    	if(chinelaPos.y > wordPosMax.y-SafeArea)
    	{
    		return true;
    	}
    	// print("Screen.height = "+Screen.height);
    	return false;
    }


    private bool IsOutRight()
    {
    	// wordPosMax 		= Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
    	// chinelaPos 	= CC.GetChinelaPosition();
    	// print(chinelaPos.x +", "+ (wordPosMax.x+1));
    	if(chinelaPos.x > -wordPosMax.x-SafeArea)
    	{
    		return true;
    	}
    	// print("Screen.height = "+Screen.height);
    	return false;
    }


    private bool IsOutBottom()
    {
    	// wordPosMax 		= Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
    	// chinelaPos 	= CC.GetChinelaPosition();
    	// print(chinelaPos.y +", "+ (SafeArea));
    	if(chinelaPos.y < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y+SafeArea)
    	{
    		return true;
    	}
    	// print("Screen.height = "+Screen.height);
    	return false;
    }


    private void SetPosition()
    {


        // if(IsOutUp() && !IsOutRight())
    	if(IsOutUp() && !IsOutRight())
    	{
    		group.SetActive(true);
	    	group.transform.position = new Vector3(chinelaPos.x, wordPosMax.y-SafeArea, group.transform.position.z);
    	}
        else if(IsOutUp() && IsOutRight())
        {
			group.SetActive(false);
            CC.EndChinela();
            // group.transform.position = new Vector3(chinelaPos.x, wordPosMax.y-SafeArea, group.transform.position.z);
        }
    	// else if(!IsOutUp() && IsOutRight())
    	// {
	    // 	group.transform.position = new Vector3(-wordPosMax.x-SafeArea, chinelaPos.y, group.transform.position.z);
    	// }
    	// else if(IsOutUp() && IsOutRight())
    	// {
	    // 	// indicator.transform.position = new Vector3(-wordPosMax.x, wordPosMax.y, indicator.transform.position.z);
    	// }
    	else
    	{
            group.SetActive(false);
    	}

    }


    private void LookAt()
    {
    	Vector3 relative = transform.InverseTransformPoint(chinelaPos);
		float angle = Mathf.Atan2(chinelaPos.x - indicator.transform.position.x, chinelaPos.y - indicator.transform.position.y) * Mathf.Rad2Deg;
		// float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
		indicator.transform.rotation = Quaternion.Euler(0, 0, -angle);

		// Vector2 temp = (Vector2) Camera.main.WorldToScreenPoint(chinelaPos);
		// float angle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
		// Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
		// Quaternion rotation = Quaternion.Slerp(rect.rotation, q, Time.deltaTime * 2000);
		// group.transform.rotation = rotation;
    }

}
