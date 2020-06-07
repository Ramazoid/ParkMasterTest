using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    Queue<Vector3> track = new Queue<Vector3>();

    public Texture2D brush;
    public GameObject car;
    public Transform court;
    public bool drawAllowed;
    public bool startfromCar;

    internal Texture2D texture;

    private GameObject ball;
    
    private Vector3 currentPoint = -Vector3.one;
    private bool drawtrack;
    private Vector3 pointA;
    private Vector3 pointB;
    private CarDriver cardriver;
    private bool hitCourt;
    

    void Start()
    {
        ball = GameObject.CreatePrimitive( PrimitiveType.Sphere );
        ball.transform.localScale = new Vector3( 0.1f, 5, 0.1f );

        EventBus.Subscribe( "StartDrawing", StartDrawing );
        EventBus.Subscribe( "StopDrawing", StopDrawing );

    }

    private void StopDrawing( object obj )
    {
        drawAllowed = false;
    }

    private void StartDrawing(object ob )
    {
        drawAllowed=true;
        cardriver = ob as CarDriver;
    }

    
    void Update()
    {
        if ( !drawAllowed )
            return;
        
        if ( Input.GetMouseButton( 0 ) )
        {

            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;

            if ( Physics.Raycast( ray, out hit, float.PositiveInfinity, LayerMask.GetMask( "Car" ) ) )
            {
                startfromCar = true;    
            }
            else
            if ( startfromCar )
            {
                if ( Physics.Raycast( ray, out hit, float.PositiveInfinity, LayerMask.GetMask( "Court" ) ) )
                {
                        track.Enqueue( hit.point ); 
                }
            }
        }

        if( Input.GetMouseButtonUp( 0 ) )
        {
            drawtrack = true;
            if( track.Count > 2 )
            {
                currentPoint = track.Dequeue();
                pointB = track.Dequeue();
                cardriver.SetNextPoint( pointB );
            }
            else
            {
                drawtrack = false;
            }
        }


    }
    private void FixedUpdate()
    {
        if (drawtrack)
        {
            if ( currentPoint == -Vector3.one )
            {
                currentPoint = car.transform.position;

                pointB = track.Dequeue();
                cardriver.SetNextPoint( pointB );
            }
            for ( int i = 0; i < 5; i++ )
            {
                currentPoint = Vector3.Lerp( currentPoint, pointB, 0.2f );
                PutBrush( currentPoint );
                if (Vector3.Distance( currentPoint, pointB ) < 1)
                {
                    if ( track.Count > 0 )
                    {
                        pointB = track.Dequeue();
                        cardriver.SetNextPoint( pointB );
                    }
                    else
                    {
                        drawtrack = false;
                    }
                }
            }
        }

    }

    private void PutBrush( Vector3 point )
    {
        
        Vector2 brpoint = court.InverseTransformPoint( point );


        float x = 250 - point.x * 5 - 25;
        float y = 250 - point.z * 5 - 25;
        DrawBrush((int)(x), (int)(y), 10, Color.white);
        texture.Apply();

    }

    private void DrawBrush(int x, int y,int d,  Color white )
    {
        if( x>440) x=440;
        if( y>440) y=440;
        if ( x < 50 ) x = 50;
        if ( y < 50 ) y = 50;
 
        Color32[] colsb = brush.GetPixels32();
        Color[] colst = texture.GetPixels( x, y, brush.width, brush.height );
        Color32[] colto = new Color32[ colsb.Length ];

        for ( int i = 0; i < colsb.Length; i++ )
        {
            Color сolor = Color.Lerp( colst[i], Color.yellow, colsb[i].a );

            colto[i] = сolor;
        }

        texture.SetPixels32( x, y, brush.width, brush.height, colto );
        
        texture.Apply();
    }
}
