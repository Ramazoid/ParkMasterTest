using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoundBank : MonoBehaviour
{
    static public  Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
    static AudioSource player;
    static public List<AudioClip> cliplist = new List<AudioClip>();

    void Start()
    {

        string[] soundfils = new string[] { "RSG","coin","win" };
        
        foreach (string sname in soundfils)
        {
            AudioClip a = Resources.Load<AudioClip>( "Sounds/" + sname );
            sounds.Add( sname, a);
        }
       
        player = GetComponent<AudioSource>();
        
    }
    public static void Play( string sname )
    {
        if ( !sounds.ContainsKey( sname ) ) throw new Exception( "Sound[" + sname + "] not found!!!" );
        AudioClip a = sounds[ sname ];
       
        cliplist.Add( a );
        player.clip = a;
        player.Play();
    }
     
        
    
    internal static void Stop()
    {
        player.Stop();
    }
}
