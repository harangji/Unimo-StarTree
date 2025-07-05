using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class VectorMaker : MonoBehaviour
{
    public float m_Distance;
    public GameObject obj;
    public MeshCollider[] mesh;
    public List<VectorMaker_Cube> cubeList = new List<VectorMaker_Cube>();
    public List<Vector3> pos = new List<Vector3>();
    private void Start()
    {
        for (int c = 0; c < mesh.Length; c++)
        {
            int xValue = (int)mesh[c].bounds.size.x - 1;
            int zValue = (int)mesh[c].bounds.size.z - 1;
            float x = mesh[c].bounds.min.x + 1.0f;
            float z = mesh[c].bounds.min.z + 1.0f;
            float y = mesh[c].bounds.max.y - 0.2f;

            int valueCountingX = 0;
            int valueCountingZ = 0;
            for (int i = 0; i < xValue; i++)
            {
                var go = Instantiate(obj, transform);
                MakeObj(new Vector3(x + valueCountingX, y, z + valueCountingZ), go.GetComponent<BoxCollider>());
                valueCountingX++;

                for (int j = 0; j < zValue; j++)
                {
                    var goZ = Instantiate(obj, transform);
                    MakeObj(new Vector3(x + valueCountingX, y, z + valueCountingZ), goZ.GetComponent<BoxCollider>());
                    valueCountingZ++;
                }
                valueCountingZ = 0;
            }
        }
        Invoke("CollisionCheck", 1.0f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            for(int i = 0; i< cubeList.Count; i++)
            {
                if (cubeList[i] != null)
                {
                    pos.Add(cubeList[i].transform.position);
                }
            }
        }
    }
    private void CollisionCheck()
    {
        for(int i = 0; i < cubeList.Count; i++)
        {
            if (cubeList[i].GetCollision == false)
            {
                Destroy(cubeList[i].gameObject);
                cubeList[i] = null;
            }
        }
        cubeList.RemoveAll(x => x == null);
    }

    private void MakeObj(Vector3 pos, Collider collider)
    {
        collider.gameObject.transform.position = pos;
        cubeList.Add(collider.gameObject.GetComponent<VectorMaker_Cube>());
    }
}
