using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
   
    GameObject[] pannels;

    private int seqIndex;
    private GameObject currentPanel;
    private float currentScaleTo;
    private bool showNext;
    private Queue<PanelShow> pannelSequence;

    void Start()
    {
        
        pannels = GameObject.FindGameObjectsWithTag( "UI" );
        foreach ( GameObject g in pannels )
            g.SetActive( false );

        EventBus.Subscribe( "CourtReady", StartRSG);
        showNext = false;
        EventBus.Subscribe( "ShowWin", ShowWin );
    }


    void Update()
    {
        if ( showNext && pannelSequence.Count > 0 )
        {

            PanelShow ps = pannelSequence.Dequeue();
            
            currentPanel = ShowPanel( ps.panelName );
            currentScaleTo = ps.ToScale;
            showNext = false;
            StartCoroutine( PannelWaitor( ps ) );


        }
        if ( currentPanel != null )
        {
            float d = currentScaleTo - currentPanel.transform.localScale.x;
            if ( d > 0.1f )
            {
                currentPanel.transform.localScale += Vector3.one / 10;
            }
        }
    }

    IEnumerator PannelWaitor( PanelShow ps )
    {
        yield return new WaitForSeconds( ps.panelTime );
        HidePanel( ps.panelName );
        if ( pannelSequence.Count > 0 )
            showNext = true;
        else
        {
            EventBus.Fire( "UIDone" );
        }
    }

    private void HidePanel( string panName )
    {
        GameObject pannel = Array.Find<GameObject>( pannels, (p) =>
        {
            return p.name == panName;
        } );

        if ( pannel != null )
        {

            pannel.SetActive( false);
            pannel.transform.localPosition = Vector3.right * 500;
        }
        else
            throw new Exception( "Pannel[" + panName + "] not found!!!!" );
    }

    private GameObject ShowPanel( string panName )
    {
        GameObject pannel = Array.Find<GameObject>( pannels, (p) =>
        {
            return p.name == panName;
        } );
        if ( pannel != null )
        {
            pannel.SetActive( true );
            pannel.transform.localPosition = Vector3.zero;
        }
        else
            throw new Exception( "Pannel[" + panName + "] not found!!!!" );
        return pannel;
    }

    void StartRSG(object ob = null)
    {
        pannelSequence = new Queue<PanelShow>();
        pannelSequence.Enqueue(new PanelShow( "Ready", 1.3f, 2) );
        pannelSequence.Enqueue(new PanelShow( "Steady", 1, 2) );
        pannelSequence.Enqueue(new PanelShow( "Go", 1.2f, 5) );

        showNext = true;
        SoundBank.Play( "RSG" );
    }

    private void ShowWin(object obj)
    {
        pannelSequence = new Queue<PanelShow>();
        pannelSequence.Enqueue(new PanelShow( "Win", 3f, 5) );
        SoundBank.Play("win");
        showNext = true;
    }
}
    struct PanelShow
    {
        public string panelName;
        public float panelTime;
        public float ToScale;

        public PanelShow( string panName, float panTime, float toScale ) : this()
        {
            this.panelName = panName;
            this.panelTime = panTime;
            this.ToScale = toScale;
        }
    }
