using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FechaComporta : MonoBehaviour
{

	public float speed, movementDistance, delayToOpen, delayToClose;
	private Rigidbody2D door1, door2;

	float auxMovement, auxDelay, edgeDistance;
	bool isClose, isMoving;


    // Gizmos - desenha o caminho - Desenvolvimento
    void OnDrawGizmos()
    {
        float lineDistance = 0.8f;

        Vector3 porta1Position = transform.GetChild(0).position, porta2Position = transform.GetChild(1).position;

        Gizmos.color = Color.blue;     
        // desenha a linha central
        Gizmos.DrawRay(transform.position - transform.right * lineDistance, 
        	           transform.right * lineDistance * 2);

        // desenha a linha superior
        Gizmos.DrawRay(transform.position - transform.right * lineDistance + 
        	           transform.up * movementDistance, transform.right * lineDistance * 2);


        // desenha a linha inferior
        Gizmos.DrawRay(transform.position - transform.right * lineDistance - 
        	           transform.up * movementDistance, transform.right * lineDistance * 2);


        Gizmos.DrawRay(transform.position - transform.right * lineDistance - 
        	           transform.up * movementDistance, transform.right * lineDistance * 2);


        Transform child 	= transform.GetChild(0);
        float edge 			= child.gameObject.GetComponent<BoxCollider2D>().size.y * child.transform.lossyScale.y; // suponho que os dois portões tenham as mesmas dimensões

        DrawArrow(transform.position + transform.up * (edge+0.2f), transform.position + transform.up * movementDistance , Color.green);
        DrawArrow(transform.position - transform.up * (edge+0.2f), transform.position - transform.up * movementDistance , Color.green);

    }


    // Gizmos - desenha flechas - Desenvolvimento
    void DrawArrow(Vector2 startPoint, Vector2 endPoint, Color color, float minDistance=0.2f, float arrowHeadAngle=25, float arrowHeadLength=0.4f)
    {
		// inicia as variáveis
		Vector3 direction, right, left;
		float d, length;
		
        Gizmos.color= color; 

		direction 	= new Vector3(endPoint.x-startPoint.x, endPoint.y-startPoint.y, 0);
		d 			= Movimenta.CalculateDistance(startPoint, endPoint);
		direction 	= direction * new Vector2( 1 - minDistance / d ,1 - minDistance / d);
		length 		= 180+arrowHeadAngle;
		right 		= Quaternion.LookRotation(direction) * Quaternion.Euler(direction.x == 0 ? 0:length, direction.x != 0 ? 0:length,0) * new Vector3(0,0,1);
		length 				= 180-arrowHeadAngle;
		left 		= Quaternion.LookRotation(direction) * Quaternion.Euler(direction.x == 0 ? 0:length, direction.x != 0 ? 0:length,0) * new Vector3(0,0,1);
		
		Gizmos.DrawRay(startPoint, direction); // desenha a linha
		Gizmos.DrawRay(startPoint + (Vector2) direction, right * arrowHeadLength); // desenha o lado direito da flecha
		Gizmos.DrawRay(startPoint + (Vector2) direction, left * arrowHeadLength); // desenha o lado esquerdo da flecha

    }


    // Start is called before the first frame update
    void Start()
    {
        door1 		= transform.GetChild(0).GetComponent<Rigidbody2D>();
        door2 		= transform.GetChild(1).GetComponent<Rigidbody2D>();
        auxMovement = 0;
        isClose 	= true;
        edgeDistance= transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().size.y * 
                      transform.GetChild(0).transform.lossyScale.y; // suponho que os dois portões tenham as mesmas dimensões
    
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }


    void FixedUpdate()
    {
    	Move();
    }

    // controla o movimento das portas
    void Move()
    {
    	auxDelay += Time.deltaTime;

		if(isClose && auxDelay >= delayToOpen)
		{
			// OpenDoor(); // obsoleto
			OpenAndCloseDoors();
		}
    	else if(!isClose && auxDelay >= delayToClose)
		{
			// CloseDoor(); // obsoleto
			OpenAndCloseDoors();
		}

    }


    // obsoleto
    // abre as portas
    void OpenDoor()
    {
    	float mov = Time.deltaTime * speed;  // movimento atual
    	auxMovement += mov;  // somatório de movimentos

    	if(auxMovement <= movementDistance - edgeDistance)
    	{
    		// faz o movimento
	    	door1.position += (Vector2) transform.up * mov;
	    	door2.position -= (Vector2) transform.up * mov;
    	}
    	else
	    {
	    	// para impedir que as portas parem antes ou depois do limite
	    	door1.position += (Vector2) transform.up * (movementDistance - edgeDistance - auxMovement + mov);
	    	door2.position -= (Vector2) transform.up * (movementDistance - edgeDistance - auxMovement + mov);

	    	auxMovement = 0;
			auxDelay = 0;
			isClose = false;
	    }
    
    }


    // obsoleto
    // fecha as portas
    void CloseDoor()
    {
    	float mov = Time.deltaTime * speed;  // movimento atual
    	auxMovement += mov;  // somatório de movimentos

    	if(auxMovement <= movementDistance - edgeDistance)
    	{
    		// faz o movimento
	    	door1.position -= (Vector2) transform.up * mov;
	    	door2.position += (Vector2) transform.up * mov;
    	}
    	else
	    {
	    	// para impedir que as portas parem antes ou depois do limite
	    	door1.position -= (Vector2) transform.up * (movementDistance - edgeDistance - auxMovement + mov);
	    	door2.position += (Vector2) transform.up * (movementDistance - edgeDistance - auxMovement + mov);

	    	auxMovement = 0;
			auxDelay = 0;
			isClose = true;
	    }
    
    }


    // simplificação de OpenDoor() e CloseDoor() 
    void OpenAndCloseDoors()
    {

    	float mov = Time.deltaTime * speed;  // movimento atual
    	auxMovement += mov;  // somatório de movimentos

    	if(auxMovement <= movementDistance - edgeDistance) // está se movendo
    	{
    		// faz o movimento
	    	door1.position += (Vector2) transform.up * mov * (isClose ? +1 : -1);
	    	door2.position += (Vector2) transform.up * mov * (isClose ? -1 : +1);
    	}
    	else // movimento chegou no final
	    {
	    	// para impedir que as portas parem antes ou depois do limite
	    	door1.position += (Vector2) transform.up * (movementDistance - edgeDistance - auxMovement + mov) * (isClose ? +1 : -1);
	    	door2.position += (Vector2) transform.up * (movementDistance - edgeDistance - auxMovement + mov) * (isClose ? -1 : +1);

	    	auxMovement = 0;
			auxDelay = 0;
			isClose = !isClose;
	    }
    
    }


    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.tag == "Chinela")
    	{
			// destroi/reseta a chinela
			ChinelaControle.Instance.EndChinela();
    	}
    
    }

}
