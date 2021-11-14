using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetismo : MonoBehaviour
{
	public bool _On, _Off;
	public float force;
	public Transform _pointTest, _pointTest2;

	Rigidbody2D rb;
	bool isOn=true, stopMovement;

	bool showGizmos = true;
	Rigidbody2D my_rb;

    void OnDrawGizmos()
    {
    	float dis = GetComponent<CircleCollider2D>().radius*transform.localScale.x;
        if(showGizmos)
        {
        	int increment = 10;
        	bool draw = true;
	        for(float c=0; c<360-increment; c+=increment)
	        {
				if(draw)
				{
				    Vector2 dirStart = GetDirOfAngle(c); 
				    Vector2 dirEnd = GetDirOfAngle(c+increment); 

				    dirStart = dirStart / CalculateDistance(dirStart, new Vector2(0,0)) * dis;
				    dirEnd = dirEnd / CalculateDistance(dirEnd, new Vector2(0,0)) * dis;

				    // float distance = CalculateDistance(dirStart, dirEnd);
				    Vector2 dir = new Vector2(dirEnd.x-dirStart.x, dirEnd.y-dirStart.y);
				    // print("distance: "+distance);	
					// print("------>  "+);
				    // float newAngle = Vector2.Angle(dirStart, dirEnd);

			        Gizmos.color = Color.blue;  
					Gizmos.DrawRay(dirStart+(Vector2) transform.position, dir);
				}
				draw = !draw;
	        }
        }
    }


    // transfoma um angulio de 0 a 360 em uma direção
    public static Vector2 GetDirOfAngle(float angle)
    {
    	float ang = angle;
		float angleCalc = ang/90;
		float y=0;
		float x=0;

		if(angleCalc >= 0 && angleCalc < 1)
		{
			float rest = angleCalc - (int) angleCalc;
			y = 1-rest;
			x = rest;
		}
		else if(angleCalc >= 1 && angleCalc < 2)
		{
			float rest = angleCalc - (int) angleCalc;
			x = 1-rest;
			y = -rest;
		}
		else if(angleCalc >= 2 && angleCalc < 3)
		{
			float rest = angleCalc - (int) angleCalc;
			x = -rest;
			y = -1+rest;
		}
		else if(angleCalc >= 3)
		{
			float rest = angleCalc - (int) angleCalc;
			x = -1+rest;
			y = rest;
		}

		return new Vector2(x,y);
    }


    // Start is called before the first frame update
    void Start()
    {
    	my_rb = GetComponent<Rigidbody2D>();
        force*=1000; // para que não seja necessário colocar valores exorbitantes no inspector
    }


    // Update is called once per frame
    void Update()
    {
    	// test ----------
    		
	    	if(_On)
	    	{
	    		_On = false;
	    		OnMagnetismo();
	    	}
	    	if(_Off)
	    	{
	    		_Off = false;
	    		OffMagnetismo();
	    	}
	    	
    	// ---------------
    	

    	bool click = CheckIfClick();
    	
    	if(click)
    	{
    		if(isOn)
    			OffMagnetismo();
    		else
    			OnMagnetismo();
    	}	
    }


    void FixedUpdate()
    {

    	// começa quando há um corpo e está ativo
    	if(rb && isOn)
    	{

	        // hit de transform.position para rb.transform.position
    		Vector2 dir = GetDirectionUnitary(rb.transform.position, transform.position);
	        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, dir, Mathf.Infinity, LayerMask.GetMask("Imã"));


	        // hit de rb.transform.position para transform.position
    		dir = GetDirectionUnitary(transform.position, rb.transform.position);
	        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, LayerMask.GetMask("ChinelaMetal"));

	        float dis = CalculateDistance(hit.point, hit2.point);
	        
    		if(dis > 0.1f)
    		{
	    		// rb.MovePosition(rb.transform.position + ((Vector3) dir *  * Time.fixedDeltaTime));
	    		rb.AddForce(GetDirectionUnitary(rb.transform.position, transform.position) * force / dis * Time.fixedDeltaTime);
    		}
    		else
    		{
    			// stopMovement = true;
    			// rb.velocity = Vector3.zero;
    			// ChinelaControle.Instance.SetThrowed(false);
    			// ChinelaControle.Instance.SetIsInMagnet(true);
    			rb.simulated = false;
    			rb.transform.SetParent(transform);
    			
    			// rb.constraints = RigidbodyConstraints2D.FreezePosition;
    			// rb.transform.position = transform.position + hit2;
    		}
    	}
    	else
    	{
    		// print("IsNull");
    	}
    }

    // segundo método (tentativa)
    // void OnTriggerStay2D(Collider2D col)
    // {
    // 	if(isOn)
    // 	{
	   //  	if(col.gameObject.layer == 10)
	   //  	{
	   //  		if(!rb) // provavelmente é melhor que ficar pegando o 'Rigidbody2D' toda vez
	   //  		{
		  //   		rb = col.GetComponent<Rigidbody2D>();
	   //  		}

	   //  		Vector2 dir = GetDirectionUnitary(rb.position, my_rb.position);
		  //       RaycastHit2D hit = Physics2D.Raycast(rb.position, dir, Mathf.Infinity, LayerMask.GetMask("Imã"));

	   //  		dir = GetDirectionUnitary(my_rb.position, rb.position);
		  //       RaycastHit2D hit2 = Physics2D.Raycast(my_rb.position, dir, Mathf.Infinity, LayerMask.GetMask("ChinelaMetal"));


	   //  		float dis = CalculateDistance(hit.point, hit2.point);

	   //  		if(dis <= 0.2f)
	   //  		{
	   //  			stopMovement = true;
	   //  		}

	   //  		if(!stopMovement)
	   //  		{
		  //   		rb.AddForce(GetDirectionUnitary(rb.position, my_rb.position) * force / dis * Time.fixedDeltaTime);
	   //  		}
	   //  		else
	   //  		{
	   //  			rb.position += my_rb.position;
	    			
	   //  		}


	   //  	}
    // 	}
    // }




    void OnTriggerStay2D(Collider2D col)
    {
    	if(col.gameObject.tag == "Chinela")
    	{
	    	if(col.gameObject.layer != 10) // diferente de chinela de metal
	    	{
	    		// print("LayerMask.LayerToName(col.gameObject.layer)):   "+LayerMask.LayerToName(col.gameObject.layer));
		        // hit de transform.position para col.gameObject.transform.position
	    		Vector2 dir = GetDirectionUnitary(col.gameObject.transform.position, transform.position);
		        RaycastHit2D hit = Physics2D.Raycast(col.gameObject.transform.position, dir, Mathf.Infinity, LayerMask.GetMask("Imã"));


		        // hit de col.gameObject.transform.position para transform.position
	    		dir = GetDirectionUnitary(transform.position, col.gameObject.transform.position);
		        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, LayerMask.GetMask(LayerMask.LayerToName(col.gameObject.layer)));

		        float dis = CalculateDistance(hit.point, hit2.point);


		        // é necessário para verificar colisão com 'ColisorSolido'
		        // sem isso a detecção acontece logo na área de força do Magnetismo
		        if(dis <= 0.3f)
		    		ChinelaControle.Instance.EndChinela();

	    	}
	    	// else
	    	// {
	    		// ChinelaControle.Instance.EndChinela();
	    	// }
    	}
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(isOn && col.gameObject.tag == "Chinela")
    	{
	    	if(col.gameObject.layer == 10) // chinela de metal
	    	{
	    		rb = col.GetComponent<Rigidbody2D>();
	    	}
	    	// else
	    	// {
	    		// ChinelaControle.Instance.EndChinela();
	    	// }
    	}
    }

    void OnTriggerExit2D(Collider2D col)
    {
    	if(col.gameObject.GetComponent<Rigidbody2D>() == rb)
    	{
	    	if(rb.simulated)
	    	{
		    	rb = null;
		    	// print("col.gameObject.GetComponent<Rigidbody2D>(): "+(col.gameObject.GetComponent<Rigidbody2D>() == null));
	    	}
    	}
    	// print("EXIT");
    }


    public void OnMagnetismo()
    {
    	isOn = true;
    }


    public void OffMagnetismo()
    {
    	isOn = false;
    	if(rb)
    	{
	    	rb.transform.SetParent(null);
			// rb.constraints 	= RigidbodyConstraints2D.None;



	    	// é necessário verificar se 'Movimenta' existe antes de tentar acessa-lo
			Movimenta script =  GetComponent<Movimenta>(); 
			rb.velocity 	= GetComponent<Rigidbody2D>().velocity * (script ? script.speed : 1); 
			ChinelaControle.Instance.SetThrowed(false);
			rb.simulated 	= true;
			// print("rb.simulated: "+rb.simulated);
	    	// rb = null;
    	}

		// rb.simulated = true;
    	// print(rb != null);
    }


    // retorna a distancia (Magnitude) entre dois pontos
    public static float CalculateDistance(Vector2 a, Vector2 b)
    {
    	// formula -----------------------------------------------
    	// raiz quadrada de ( ( x'' - x' ) ^ 2 + ( y'' - y' ) ^ 2)
    	return Mathf.Pow(Mathf.Pow(b.x-a.x, 2)+Mathf.Pow(b.y-a.y, 2), 0.5f);
    }


    // muda valores de uma direção para valores entre 0 e 1
    Vector2 GetDirectionUnitary(Vector2 a, Vector2 b)
    {
    	// calcula a direção
    	Vector2 dir = new Vector2(b.x-a.x, b.y-a.y);

    	// se certifica que a distancia a partir do centro é a mesma para qualquer direção
    	// serve para impedir que distancias na diagonal seja maior que na vertical/horizontal
    	// (está dividindo pela magnitude a partir do ponto (0,0))
    	Vector2 CorrectDirection = dir / Mathf.Pow(dir.x*dir.x+dir.y*dir.y, 0.5f);

    	return CorrectDirection;
    }


    // verifica se há clique no filho 'ColisorSolido'
	public bool CheckIfClick()
	{		
		if(Input.GetMouseButtonDown(0))
		{
		    Vector3 mousePos 	= Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    Vector2 mousePos2D 	= new Vector2(mousePos.x, mousePos.y);
		    RaycastHit2D hit 	= Physics2D.Raycast(mousePos2D, Vector2.zero, 
		    						Mathf.Infinity, LayerMask.GetMask("Imã")); // Detecta apenas o filho 'ColisorSolido'

		    // checa se houve alguma colisão
		    if (hit.collider != null) {
			    // Debug.Log("Something was clicked!");

			    // se certifica que o clique foi no filho 'ColisorSolido' e não em um clone qualquer
		    	if(hit.collider.gameObject.layer == 13 && hit.collider.gameObject == transform.GetChild(0).gameObject)
		    	{
		    		return true; // houve clique
		    		// print("and is it was me 'imã'");
		    	}
			}
		}

		return false;// não houve clique
	}
	
}
