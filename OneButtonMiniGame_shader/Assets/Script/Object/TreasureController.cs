using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
public class TreasureController : TreasureAnimator
{
    BoxCollider2D treasure_collider;
    public bool treasure_open {get;set;}
    private Transform transform;

    [SerializeField]public float init_treasure_position_x;
    [SerializeField]public float init_treasure_position_y;
    public CancellationTokenSource cts = new CancellationTokenSource();
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        
        transform.position = new Vector2(init_treasure_position_x,init_treasure_position_y);
        var token = cts.Token;
        treasure_open = false; 
        treasure_collider = gameObject.AddComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // OnTriggerEnter2D(treasure_collider);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            // Debug.Log("宝ゲット");
            // treasure_open = true;
            TreasureOpen();
        }
    }
    public async UniTask TreasureOpen()
    {
        //token.ThrowIfCancellationRequested();
        Debug.Log("宝ゲット");
        treasure_open = true;
    }
}
