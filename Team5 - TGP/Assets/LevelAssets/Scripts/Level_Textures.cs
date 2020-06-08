using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level_Textures : MonoBehaviour
{
    enum TileDirection
    {
        UP,
        LEFT,
        RIGHT,
        DOWN,
        ULC,
        URC,
        LLC,
        LRC,
        TWOS,
        TWOU,
        THREEUP,
        THREERIGHT,
        THREELEFT,
        THREEDOWN,
        FULL,
        SOLO,
    }

    class tileData
    {
        public TileBase tile;
        public int posX;
        public int posY;
    }

    [SerializeField]
    Tile[] tileSet = new Tile[16];

    private BoundsInt bounds;
    private Tilemap tilemap;
    private List<tileData> allTiles = new List<tileData>();
        
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        FillAllTiles();

        for (int i = 0; i < allTiles.Count; i++)
        {
            TileDirection direction = CalculateNearby(allTiles[i]);
            Vector3Int pos = new Vector3Int(allTiles[i].posX, allTiles[i].posY, 0);

            if (direction == TileDirection.UP)
                tilemap.SetTile(pos, tileSet[1]);
            else if (direction == TileDirection.LEFT)
                tilemap.SetTile(pos, tileSet[3]);
            else if (direction == TileDirection.RIGHT)
                tilemap.SetTile(pos, tileSet[5]);
            else if (direction == TileDirection.DOWN)
                tilemap.SetTile(pos, tileSet[7]);
            else if (direction == TileDirection.ULC)
                tilemap.SetTile(pos, tileSet[0]);
            else if (direction == TileDirection.URC)
                tilemap.SetTile(pos, tileSet[2]);
            else if (direction == TileDirection.LLC)
                tilemap.SetTile(pos, tileSet[6]);
            else if (direction == TileDirection.LRC)
                tilemap.SetTile(pos, tileSet[8]);
            else if(direction == TileDirection.TWOS)
                tilemap.SetTile(pos, tileSet[14]);
            else if (direction == TileDirection.TWOU)
                tilemap.SetTile(pos, tileSet[13]);
            else if (direction == TileDirection.THREEUP)
                tilemap.SetTile(pos, tileSet[9]);
            else if (direction == TileDirection.THREELEFT)
                tilemap.SetTile(pos, tileSet[12]);
            else if (direction == TileDirection.THREERIGHT)
                tilemap.SetTile(pos, tileSet[10]);
            else if (direction == TileDirection.THREEDOWN)
                tilemap.SetTile(pos, tileSet[11]);
            else if (direction == TileDirection.FULL)
                tilemap.SetTile(pos, tileSet[15]);
            else
                tilemap.SetTile(pos, tileSet[4]);
        }
            
    }

    void FillAllTiles()
    {
        bounds = tilemap.cellBounds;
        TileBase[] tileBounds = tilemap.GetTilesBlock(bounds);


        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = tileBounds[x + y * bounds.size.x];
                if (tile != null)
                {
                    tileData tileData = new tileData();

                    tileData.tile = tile;
                    tileData.posX = x + bounds.x;
                    tileData.posY = y + bounds.y;
                    allTiles.Add(tileData);
                }
            }
        }
    }

    TileDirection CalculateNearby(tileData tile)
    {
        TileDirection direction = TileDirection.SOLO;
        List<int> values = new List<int>();

        if (tilemap.GetTile(new Vector3Int(tile.posX, tile.posY + 1, 0)) == null)
            values.Add(1);
        if (tilemap.GetTile(new Vector3Int(tile.posX + 1, tile.posY, 0)) == null)
            values.Add(2);
        if (tilemap.GetTile(new Vector3Int(tile.posX, tile.posY - 1, 0)) == null)
            values.Add(3);
        if (tilemap.GetTile(new Vector3Int(tile.posX - 1, tile.posY, 0)) == null)
            values.Add(4);

        if (values.Count == 1)
        {
            if (values.Contains(1))
                direction = TileDirection.UP;
            else if (values.Contains(2))
                direction = TileDirection.RIGHT;
            else if (values.Contains(3))
                direction = TileDirection.DOWN;
            else if (values.Contains(4))
                direction = TileDirection.LEFT;
        }
        else if (values.Count == 2)
        {
            if (values.Contains(1) && values.Contains(2))
                direction = TileDirection.URC;
            else if (values.Contains(2) && values.Contains(3))
                direction = TileDirection.LRC;
            else if (values.Contains(3) && values.Contains(4))
                direction = TileDirection.LLC;
            else if (values.Contains(4) && values.Contains(1))
                direction = TileDirection.ULC;
            else if (values.Contains(1) && values.Contains(3))
                direction = TileDirection.TWOS;
            else if (values.Contains(2) && values.Contains(4))
                direction = TileDirection.TWOU;
        }
        else if (values.Count == 3)
        {
            if (values.Contains(1) && values.Contains(2) && values.Contains(4))
                direction = TileDirection.THREEUP;
            else if (values.Contains(1) && values.Contains(2) && values.Contains(3))
                direction = TileDirection.THREERIGHT;
            else if (values.Contains(2) && values.Contains(3) && values.Contains(4))
                direction = TileDirection.THREEDOWN;
            else if (values.Contains(3) && values.Contains(4) && values.Contains(1))
                direction = TileDirection.THREELEFT;
        }
        else if (values.Count == 4)
            direction = TileDirection.FULL;
        return direction;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
