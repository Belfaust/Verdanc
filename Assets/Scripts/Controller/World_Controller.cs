using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class World_Controller : MonoBehaviour
{
    public static World_Controller _Instance{get;protected set;}
    public Texture GroundTexture;
    public int Money = 250,Substance = 25;
    public World World{get;protected set;}
    GameObject[,] ChunkList;
     void Start() {

        if(_Instance != null){
        Debug.Log("Err there are 2 instances of World Controllers");
        }
        else
        {   _Instance = this;}
        CreateNewWorld();
    }
    public void CreateNewWorld()
    {
        World = new World();
        ChunkList = new GameObject[World.Width,World.Height];       //Generating the World with Tile types
            for (int ChunkX = 0; ChunkX < World.Width;  ChunkX += World.ChunkSize)
            {
                for (int ChunkY = 0; ChunkY < World.Height; ChunkY += World.ChunkSize)
                {
                    GameObject tile_GO = new GameObject();          // GO is a shortcut for GameObject

                    ChunkList[ChunkX/World.ChunkSize,ChunkY/World.ChunkSize] = tile_GO;
                    tile_GO.name = "Chunk_"+ChunkX+"_"+ChunkY;

                    tile_GO.transform.position = new Vector3(ChunkX,ChunkY,0);
                    tile_GO.transform.SetParent(this.transform,true);

                    tile_GO.AddComponent<MeshFilter>();
                    tile_GO.AddComponent<MeshRenderer>();
                    tile_GO.AddComponent<MeshCollider>();

                    tile_GO.GetComponent<MeshRenderer>().material.mainTexture = GroundTexture;

                    StartCoroutine(GenerateMeshes(ChunkX,ChunkY));
                }
            }
    }
    IEnumerator GenerateMeshes(int ChunkX,int ChunkY)
    {
        for (int x = ChunkX; x < ChunkX + World.ChunkSize; x++)                       //Starting to Generate Visual models for the World
            {
                for (int y = ChunkY; y < ChunkY + World.ChunkSize; y++)
                {
                    for (int z = 0; z < World.Depth; z++)
                    {
                        Tile tile_data = World.GetTileAt(x,y,z);
                        OnTileTypeChange(tile_data);                // Executing a callback and adding it to the tile
                        tile_data.RegisterTileTypeChange( OnTileTypeChange );
                    }
                }
            }
        yield return new WaitForSeconds(.1f);
    }
    void OnTileTypeChange(Tile tile_data)
    {
        GameObject tile_GO = GetTileGameObject(tile_data);
        Mesh tile_mesh = tile_GO.GetComponent<MeshFilter>().mesh;
        MeshCollider collider = tile_GO.GetComponent<MeshCollider>();
        Vector2[] uv = new Vector2[4];
        if(GetTileGameObject(tile_data)==false)
        {
            Debug.LogError("TileGameobjectMap doesn't contain the tile data");
            return;
        }
        if(tile_GO == null)
        {
            Debug.LogError("TilegameobjectMap's returned Gameobject is null");
            return;
        }
        void TextureChange(Tile tile,TileType tileType,float originTexturePointX,float originTexturePointY)
        {
            if(tile.Type == tileType)
            {
            uv[0] = new Vector2(originTexturePointX,originTexturePointY);
            uv[1] = new Vector2(originTexturePointX,originTexturePointY+0.5f);
            uv[2] = new Vector2(originTexturePointX+0.5f,originTexturePointY);
            uv[3] = new Vector2(originTexturePointX+0.5f,originTexturePointY+0.5f);
            }
        }
        TextureChange(tile_data,TileType.Grass,0,0.5f);

        TextureChange(tile_data,TileType.Water,0,0);
        
        TextureChange(tile_data,TileType.Dirt,0.5f,0);

        TextureChange(tile_data,TileType.Road,0.5f,0.5f);
        
        if(tile_data.Type != TileType.Empty)
        {
            collider.sharedMesh = null;
            TileMeshChange(tile_data,tile_mesh, uv);
            collider.sharedMesh = tile_mesh;
        }
    }
    
    public void TileMeshChange(Tile tile_data,Mesh tile_mesh,Vector2[] uv)
    {
        List<Vector3> vertices = new List<Vector3>(tile_mesh.vertices);
        List<int> triangles = new List<int>(tile_mesh.triangles);               // Making lists to keep track of the vertices and triangles in this specific mesh
        List<Vector2> uvs = new List<Vector2>(tile_mesh.uv);
        for (int i = 0; i < 6; i++)                             //Checking all sides of the voxel by changing the int of FaceDirections enum
        {
            if(GetNeighbour(tile_data,(FaceDirections)i) != null&&GetNeighbour(tile_data,(FaceDirections)i).Type == TileType.Empty)
            {
                MakeFace((FaceDirections)i,tile_data,vertices,triangles);
                uvs.AddRange(uv);
            }
            else if(GetNeighbour(tile_data,(FaceDirections)i) == null)
            {
                MakeFace((FaceDirections)i,tile_data,vertices,triangles);
                uvs.AddRange(uv);
            }
        }
        tile_mesh.SetVertices(vertices);
        tile_mesh.SetTriangles(triangles.ToArray(),0);
        tile_mesh.uv = uvs.ToArray();
        tile_mesh.Optimize();
    }
    void MakeFace(FaceDirections dir,Tile tile_data,List<Vector3> vertices,List<int> triangles)
    {
            vertices.AddRange (CubeMeshData.faceVertices(dir,new Vector3(tile_data.X-GetTileGameObject(tile_data).transform.position.x,tile_data.Y-GetTileGameObject(tile_data).transform.position.y,tile_data.Z)));
            int vCount = vertices.Count;
            

            triangles.Add(vCount -4);
            triangles.Add(vCount -4 + 1);
            triangles.Add(vCount -4 + 2);
            triangles.Add(vCount -4);
            triangles.Add(vCount -4 + 2);
            triangles.Add(vCount -4 + 3);
    }
       Vector3[] offsets =                  //Offsets to check the position of Tiles and their GameObjects .Not the position of the vertices
    {
        new Vector3(0,0,1),
        new Vector3(1,0,0),
        new Vector3(0,0,-1),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,-1,0)
    };
    public Tile GetNeighbour(Tile originTile,FaceDirections dir)
    {
        Vector3 offsetToCheck = offsets[(int)dir];
        Vector3 neigbourCoord = new Vector3(originTile.X + offsetToCheck.x,originTile.Y + offsetToCheck.y,originTile.Z + offsetToCheck.z);
        if(neigbourCoord.x>World.Width-1||neigbourCoord.x<0||neigbourCoord.y>World.Height-1||neigbourCoord.y<0||neigbourCoord.z>World.Depth-1||neigbourCoord.z<0)
        {
            return null;
        }
        else
        {
        return GetTileAtWorldCoord(neigbourCoord);
        }
    }
    public GameObject GetTileGameObject(Tile tile)
    {
       return ChunkList[tile.X/World.ChunkSize,tile.Y/World.ChunkSize];
    }
    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);
        int z = Mathf.FloorToInt(coord.z);

        return World.GetTileAt(x , y , z);
    }
}
public enum FaceDirections{
    North,
    East,
    South,
    West,
    Up,
    Down
}
