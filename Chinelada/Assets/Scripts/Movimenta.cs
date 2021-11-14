using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//
//  Para criar um caminho basta criar objetos filho no gameObject que vai se mover.
//  Também é possível ignorar os primeiros filhos com 'ignoreFirstsChildren', 
//  onde 0 = não ignora, 1 = ignora o primeiro filho, 2 = ignora os 2 primeiros filhos...
//



public class Movimenta : MonoBehaviour
{
	//Test
	// public Transform test;
	// public Vector2 showTest;


    public float speed;
	public int ignoreFirstsChildren; // necessario para ignorar o 'ColisorSolido' no tile de magnetismo | primeiro filho = 1
	public bool loop, desactiveOnTrigger; // estou usando 'desactiveOnTrigger' para não dá conflito com o script 'Magnetismo'
	// public bool moveX, moveY, useChildPos;


	private Vector2[] way;
	private int[] dir;
	private bool increment=true;
    private Rigidbody2D m_rb;


	float currentDistance;
	int currentWayIdx = 1;
    bool showGizmos=true, started;

    // desenhar o caminho - Desenvolvedor
    void OnDrawGizmos()
    {
        float arrowHeadAngle = 25, arrowHeadLength = 0.4f, minDistance=0.2f;

        if(showGizmos)
        {
            int size = transform.childCount-ignoreFirstsChildren-1;
            if(started)
            {
                size = way.Length-1;
            }
            for(int c=0; c<size; c++)
            {
                Vector3 pos = transform.GetChild(c+ignoreFirstsChildren).position;
                Vector3 pos2 = transform.GetChild(c+ignoreFirstsChildren+1).position;
                
                if(!started)
                {
                    pos = transform.GetChild(c+ignoreFirstsChildren).position;
                    pos2 = transform.GetChild(c+ignoreFirstsChildren+1).position;
                }
                else
                {
                    pos = way[c];
                    pos2 = way[c+1];
                    // Vector3 pos2 = transform.GetChild(c+ignoreFirstsChildren+1).position;
                }
                Vector3 direction = new Vector3(pos2.x-pos.x, pos2.y-pos.y, 0);
                float d = CalculateDistance(pos, pos2);
                direction = direction * new Vector2( 1 - minDistance / d ,1 - minDistance / d);
                Gizmos.color = Color.green;     
                Gizmos.DrawRay(pos, direction);
                float length = 180+arrowHeadAngle;
                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(direction.x == 0 ? 0:length, direction.x != 0 ? 0:length,0) * new Vector3(0,0,1);
                length = 180-arrowHeadAngle;
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(direction.x == 0 ? 0:length, direction.x != 0 ? 0:length,0) * new Vector3(0,0,1);
                Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
            }

            if(transform.childCount-ignoreFirstsChildren > 2 && loop)
            {
                Vector3 pos = transform.GetChild(transform.childCount-1).position;
                Vector3 pos2 = transform.GetChild(ignoreFirstsChildren).position;
                if(!started)
                {
                    pos = transform.GetChild(transform.childCount-1).position;
                    pos2 = transform.GetChild(ignoreFirstsChildren).position;
                }
                else
                {
                    pos = way[size];
                    pos2 = way[0];
                    // Vector3 pos2 = transform.GetChild(c+ignoreFirstsChildren+1).position;
                }
                // Vector3 pos = transform.GetChild(transform.childCount-1).position;
                // Vector3 pos2 = transform.GetChild(ignoreFirstsChildren).position;
                Vector3 direction = new Vector3(pos2.x-pos.x, pos2.y-pos.y, 0);
                float d = CalculateDistance(pos, pos2);
                direction = direction * new Vector2( 1 - minDistance / d ,1 - minDistance / d);
                Gizmos.DrawRay(pos, direction);
                float length = 180+arrowHeadAngle;
                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(direction.x == 0 ? 0:length, direction.x != 0 ? 0:length,0) * new Vector3(0,0,1);
                length = 180-arrowHeadAngle;
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(direction.x == 0 ? 0:length, direction.x != 0 ? 0:length,0) * new Vector3(0,0,1);
                Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
            }

            Vector3 _pos = transform.GetChild(ignoreFirstsChildren).position;
            if(!started)
                _pos = transform.GetChild(ignoreFirstsChildren).position;
            else
                _pos = way[0];

            Gizmos.color = Color.red; 
            Gizmos.DrawSphere(_pos, 0.15f);   // mostra o ponto de partida  
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        started = true;
        m_rb = GetComponent<Rigidbody2D>();
        // showGizmos=false; // desliga os Gizmos
    	way = new Vector2[transform.childCount-ignoreFirstsChildren]; // tamanho do caminho
        
    	for(int c=0; c<transform.childCount-ignoreFirstsChildren; c++)
    	{
    		way[c] = (Vector2) transform.GetChild(c+ignoreFirstsChildren).transform.position; // salva cada posição do caminho
    		transform.GetChild(c+ignoreFirstsChildren).gameObject.SetActive(false); // ganho de perfomance? sei lá
    	}
    	
        m_rb.position = way[0]; // Ponto de partida
    }

    void FixedUpdate()
    {
		currentDistance = CalculateDistance(m_rb.position, way[currentWayIdx]);

		if(currentDistance > 0.05f)
		{
            Vector3 m_Input = (Vector3) CalculateMovement(m_rb.position, way[currentWayIdx]);
            m_rb.velocity = m_Input * Time.fixedDeltaTime * speed * 40;
		}
		else
		{
			if(currentWayIdx == way.Length-1 || currentWayIdx == 0)
			{
				increment = !increment;
			}

			if(increment)
			{
				currentWayIdx++;
			}
			else if(!loop)
			{
				currentWayIdx--;
			}
			else
			{
				currentWayIdx=0;
			}

            m_rb.velocity = Vector2.zero;
		}

    	// showTest = CalculateMovement(m_rb.position, test.position);

    }


    public static float CalculateDistance(Vector2 a, Vector2 b)
    {
    	return Mathf.Pow(Mathf.Pow(b.x-a.x, 2)+Mathf.Pow(b.y-a.y, 2), 0.5f);
    }


    Vector2 CalculateMovement(Vector2 a, Vector2 b)
    {
    	Vector2 d = new Vector2(b.x-a.x, b.y-a.y);
    	// print("d: "+d);
    	if(Mathf.Abs(d.x) > Mathf.Abs(d.y))
    	{
    		// print("maior");
    		d = new Vector2(d.x/Mathf.Abs(d.x), d.y/Mathf.Abs(d.x));
    	}
    	else
    	{
    		d = new Vector2(d.x/Mathf.Abs(d.y), d.y/Mathf.Abs(d.y));
    		// d = new Vector2(d.x/d.y, 1);
    	}
    	// print("d2: "+d);

    	return d / Mathf.Pow(d.x*d.x+d.y*d.y, 0.5f);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Chinela" && !desactiveOnTrigger)
        {
            // destroi/resta a chinela
            ChinelaControle.Instance.EndChinela();
        }
    
    }


}
