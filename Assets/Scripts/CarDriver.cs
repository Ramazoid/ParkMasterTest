using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour
{
    public bool drive;
    public Vector3 pointB = -Vector3.one;
    private Queue<Vector3> points;

    void Start()
    {
        points = new Queue<Vector3>();
    }


    void Update()
    {
        if (drive)
        {


            transform.LookAt( pointB );
            transform.position = Vector3.Lerp( transform.position, pointB, 0.99f );

            if ( Vector3.Distance( transform.position, pointB ) < 0.1f )
            {
                if ( points.Count > 0 )
                {
                    pointB = points.Dequeue();
                }
                else
                    drive = false;
            }
        }

    }

    public void SetNextPoint(Vector3 nextpoint)
    {
        points.Enqueue(nextpoint);
        if ( points.Count == 1 )
            pointB = nextpoint;

        drive = true;
    }
    private void OnTriggerEnter(Collider col)
    {
        if ( col.gameObject.tag == "Сoin" )
        {
            SoundBank.Play( "coin" );
            Destroy( col.gameObject );

        }
        else if ( col.gameObject.tag == "Parking" )
        {

            EventBus.Fire( "Win" );
        }

    }
}
