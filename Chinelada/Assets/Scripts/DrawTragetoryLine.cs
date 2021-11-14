using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

// ISSO FOI APENAS UM TESTE
 

public class DrawTragetoryLine : MonoBehaviour
{
	[SerializeField]
	private LineRenderer _lineRenderer;

	[SerializeField]
	[Range(3,30)]
	private int _lineSegmentCount = 20;

	private List<Vector3> _linePoints = new List<Vector3>();

	#region Singleton

	public static DrawTragetoryLine Instance;

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    void Awake()
    {
        Instance = this;
    }

    #endregion

    public void UpdateTrajectory(Vector3 forceVector, Rigidbody2D rigidBody, Vector2 startingPoint)
    {
    	print("Working!");
    	Vector3 velocity = (forceVector / rigidBody.mass) * Time.fixedDeltaTime;

    	float FlightDuration = (2*velocity.y) / Physics.gravity.y;

    	float stepTime = FlightDuration/ _lineSegmentCount;

    	_linePoints.Clear();

    	for (int i=0;i<_lineSegmentCount; i++)
    	{
    		float stepTimePassed = stepTime * i;

    		Vector2 MovementVector = new Vector3(
				velocity.x * stepTimePassed,
				velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed 
    		);

    		_linePoints.Add(-MovementVector+startingPoint);

    	}

    	_lineRenderer.positionCount = _linePoints.Count;
    	_lineRenderer.SetPositions(_linePoints.ToArray());

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideLine()
    {
    	_lineRenderer.positionCount = 0;
    }
}
