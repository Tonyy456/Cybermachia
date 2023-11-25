using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarTargeting : MonoBehaviour
{
    [SerializeField] private List<Tilemap> tilemaps;
    [SerializeField] private bool findTileMaps;
    [SerializeField] private bool filterByTag;
    [SerializeField] private string tileMapTag;



    // Start is called before the first frame update
    void Start()
    {
        if(findTileMaps)
        {
            tilemaps = new List<Tilemap>(GameObject.FindObjectsOfType<Tilemap>());
            if (filterByTag) tilemaps = tilemaps.FindAll(x => x.tag == tileMapTag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Tilemap map = tilemaps[0];
    }
}
