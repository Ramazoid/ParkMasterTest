using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;


public class KeyPresser : MonoBehaviour
{

   void Start()
    {


    }
    
    void Update()
    {

        print("Horizontal=" + Input.GetAxis("Horizontal") + "  :: Vertical=" + Input.GetAxis("Vertical"));
    }
}
