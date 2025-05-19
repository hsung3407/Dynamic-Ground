using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Dice : MonoBehaviour
{
    private MeshFilter _meshFilter;
    [SerializeField] private int[] numbers = { 1, 2, 3, 4, 5, 6 };
    
    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        foreach (var meshNormal in _meshFilter.mesh.normals) { Debug.Log(meshNormal); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(GetUpNumber());
        }
    }

    public int GetUpNumber()
    {
        int upNumber = 1;
        float max = 1;
        var normals = _meshFilter.mesh.normals;
        for (var i = 0; i < normals.Length; i++)
        {
            var dir = transform.rotation * normals[i];
            var number = numbers[i];

            float dot;
            if ((dot = Vector3.Dot(dir, Vector3.up)) > max)
            {
                max = dot;
                upNumber = number;
            }
        }

        return upNumber;
    }
}