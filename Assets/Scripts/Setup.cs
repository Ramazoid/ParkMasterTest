using UnityEngine;
using Random = UnityEngine.Random;

public class Setup : MonoBehaviour
{
    private  Texture2D courtTex;
    public GameObject courtOb;
    private Renderer courtRend;
    private Pointer pointer;
    public GameObject car;
    private CarDriver cardriver;


    void Start()
    {
        EventBus.Subscribe("UIDone", PutCar);
        EventBus.Subscribe("Win", Win);
    }

    private void Win(object obj)
    {
       
        EventBus.Fire("ShowWin");
        EventBus.Fire("StopDrawing");
    }

    public void Begin()
    {
        courtTex = new Texture2D(500, 500);
        for (int x = 0; x < 500; x++)
            for (int y = 0; y < 500; y++)
            {
                courtTex.SetPixel(x, y, Color.black);
            }

        courtTex.Apply();
        courtOb = GameObject.Find("Court");
        courtOb.transform.position = Vector3.zero;
        courtRend = courtOb.GetComponent<Renderer>();
        courtRend.material.mainTexture = courtTex;

        pointer = GetComponent<Pointer>();
        pointer.car = car;
        pointer.court = courtOb.transform;
        pointer.texture = courtTex;

        PutCoins();

        EventBus.Fire("CourtReady");
    }

    private void PutCoins()
    {
        GameObject coin = Resources.Load<GameObject>("Prefabs/Coin");
        for(var i=0; i<20;i++)
        {
            Vector2 rpos = Random.insideUnitCircle*50;
            Vector3 pos = new Vector3(rpos.x, 0, rpos.y);
            Instantiate(coin, pos, Quaternion.Euler(0, 90, 90));

        }
    }

    private void PutCar(object ob = null)
    {
        Vector3 carPosition = new Vector3(Random.Range(-40, 10), 3.5f, -40);
        car.transform.position = carPosition;
        car.transform.rotation = Quaternion.Euler(0, 0, 0);
        cardriver = car.AddComponent<CarDriver>();

        EventBus.Fire("StartDrawing",cardriver);
    }
 
}
