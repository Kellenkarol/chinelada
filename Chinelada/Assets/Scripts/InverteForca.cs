using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class InverteForca : MonoBehaviour
{

	// public GameObject point;

	private Vector2 top, right, left, bottom, rangeX, rangeY;
	private List<Vector2> posSide = new List<Vector2>();
	private List<string> sidesName = new List<string>(){"Cima", "Baixo", "Esquerda", "Direita"};



    // Start is called before the first frame update
    void Start()
    {
    	// corrige a rotação para um grau múltiplo de 90
    	transform.eulerAngles = new Vector3(0,0,Mathf.Round(Mathf.Round(transform.eulerAngles.z/90)*90));

    	Vector2 size 	= transform.lossyScale;

    	BoxCollider2D myCol = GetComponent<BoxCollider2D>();

        Vector2 _top 			= transform.position + transform.up 	* size[1]/2 * myCol.size.y; 
        Vector2 _bottom 		= transform.position - transform.up 	* size[1]/2 * myCol.size.y; 
        Vector2 _left 			= transform.position - transform.right 	* size[0]/2 * myCol.size.x; 
        Vector2 _right 			= transform.position + transform.right	* size[0]/2 * myCol.size.x; 


		// update with rotation
        float rz = transform.eulerAngles.z;
		top 		= rz == 0 ? _top : rz == 90 ? _right : rz == 180 ? _bottom : _left;  
		bottom 		= rz == 0 ? _bottom : rz == 90 ? _left : rz == 180 ? _top : _right;  
		left 		= rz == 0 ? _left : rz == 90 ? _top : rz == 180 ? _right : _bottom;  
		right 		= rz == 0 ? _right : rz == 90 ? _bottom : rz == 180 ? _left : _top;  

        posSide.Add(top);
        posSide.Add(bottom);
        posSide.Add(left);
        posSide.Add(right);

        rangeX = new Vector2(left.x, right.x);
        rangeY = new Vector2(top.x, bottom.x);

        // test
        // GameObject a = Instantiate(point) as GameObject;
        // a.transform.position = top;
        // a.GetComponent<SpriteRenderer>().color = new Color(0,1,0);
        // // a = Instantiate(point) as GameObject;
        // a.transform.position = bottom;
        // a.GetComponent<SpriteRenderer>().color = new Color(0,0,1);
        // // a = Instantiate(point) as GameObject;
        // a.transform.position = left;
        // a.GetComponent<SpriteRenderer>().color = new Color(1,0,1);
        // // a = Instantiate(point) as GameObject;
        // a.transform.position = right;
        // a.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,1);
    }

    	
    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D col)
    {
    	Vector2 colPos = col.transform.position; 
    	// print("Reflect: " + Vector3.Reflect(colPos, Vector3.right));
    	int side = GetSideContact(colPos);
    	// print("Side: "+sidesName[side]);

		//front face center: transform.position + transform.forward * cubeSize/2;
		// back face center: trasnform.position - transform.forward * cubeSize/2;
		// left face center: transform.position - transform.right * cubeSize/2;
		// bottom face center: transform.position - transform.up * cubeSize/2;
	    

	    // RaycastHit hit;

	    // if (Physics.Raycast(transform.position, transform.forward, out hit))
	    // {
	        // Debug.Log("Point of contact: "+hit.point);
	    // }
    	Rigidbody2D colRB = col.GetComponent<Rigidbody2D>();
    	Vector2 vlc = colRB.velocity;
    	// print(vlc);
    	// side = 2;
    	// colRB.velocity = Vector3.Reflect(transform.position,  Vector3.right);
    	if(side == 0 || side == 1)
    	{
    		colRB.velocity = new Vector2(vlc.x, -vlc.y);
    	}
    	else
    	{
    		colRB.velocity = new Vector2(-vlc.x, vlc.y);
    	}

    	// if(vlc.x < vlc.y)
    	// {
    	// 	colRB.velocity = new Vector2(-vlc.x, vlc.y);
    	// }
    	// else
    	// {
    	// 	colRB.velocity = new Vector2(vlc.x, -vlc.y);
    	// }

    }


    float Magnitude(Vector2 a, Vector2 b)
    {
    	return Mathf.Pow(Mathf.Pow(b.x-a.x, 2) + Mathf.Pow(b.y-a.y, 2), 0.5f);
    }


   	// retorna a direção do contato
   	// side ->  0 = top, 1 = bottom, 2 = left, 3 = right 
    int GetSideContact(Vector2 pos)
    {
    	float magnitude, minMagnitude=0;
    	int cont=0, side=0; 


		if(rangeX[0] < pos.x && pos.x < rangeX[1])
		{
			if(Magnitude(pos, top) < Magnitude(pos, bottom))
			{
				return 0;
			}
			return 1;
		}
		else
		{
			if(Magnitude(pos, left) < Magnitude(pos, right))
			{
				return 2;
			}
			return 3;
		}


    	foreach(Vector2 p in posSide)
    	{
    		magnitude = Magnitude(pos, p); //distancia dos dois pontos em um float
    		
    		if(cont == 0)
    		{
    			minMagnitude = magnitude;
    		}


    		if(magnitude < minMagnitude)
    		{
    			minMagnitude = magnitude;
    			side = cont;
    		}

    		cont++;
    	}

    	return side;
    }
}
