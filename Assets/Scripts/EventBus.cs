using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    internal static Dictionary<string, Action<object>> events = new Dictionary<string, Action<object>>();

    internal static void Subscribe( string eventName, Action<object> act )
    {
        if ( events.ContainsKey( eventName ) )
            throw new Exception( "Duplicate Event Subscriber[ event:" + eventName + "]" );

        events.Add( eventName, act );
    }

    internal static void Fire( string eventName, object param = null )
    {
        if ( events.ContainsKey( eventName ) )
            events[eventName]( param );
        else
            throw new Exception( "Event[" + eventName + "] not found!!!" );

    }
}
