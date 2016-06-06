﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
[AddComponentMenu("Frosty-Rendering/TiledSprite")]
public class FrostyTiledSprite : MonoBehaviour
{
    public enum TilingType
    {
        [Header("Single Quad, Texture Scaling Enabled")]
        SingleQuadChangeScaling,
        [Header("Multiple Quads, No Texture Scaling, Combining Possible")]
        MultipleQuadsNoScaleChange
    }

    [Header("Rendering Settings")]
    public Shader baseShader;
    [Tooltip("Leave empty to create a specific material for this object")]
    public Material sharedMaterial;
    public Sprite sprite;

    [Header("Tile Settings")]
    public Vector2 numberOfTiles = Vector2.one;
    public TilingType tilingType = TilingType.SingleQuadChangeScaling;

    private Shader innerShader;
    private Vector2 innerSize;
    private Sprite innerSprite;
    private TilingType innerTilingType;
    private Material innerSharedMaterial;
    public Vector2 Scale { get; private set; }

    [SerializeField]
    [HideInInspector]
    private int instanceID = 0;
    private bool shouldRecreateMaterial;
    private bool forceUpdate;

    void GenerateQuads()
    {
        if (!shouldRecreateMaterial &&
            !forceUpdate &&
            innerSize == numberOfTiles &&
            innerSprite == sprite &&
            innerShader == baseShader &&
            innerSharedMaterial == sharedMaterial &&
            innerTilingType == tilingType
            ) return;

        Vector2 spriteScale = sprite.bounds.size;
        this.Scale = spriteScale;

        innerSize = numberOfTiles;
        innerSprite = sprite;
        innerTilingType = tilingType;
        innerShader = baseShader;
        forceUpdate = false;

        if (baseShader == null) return;

        MeshRenderer mr = this.gameObject.GetComponent<MeshRenderer>();
        if (mr == null) { mr = this.gameObject.AddComponent<MeshRenderer>(); }
        mr.hideFlags = HideFlags.NotEditable;
        var mat = sharedMaterial.GetInstanceID() != 0 ? sharedMaterial : (mr.sharedMaterial == innerSharedMaterial ? null : mr.sharedMaterial);
        innerSharedMaterial = sharedMaterial;

        if (mat == null || (shouldRecreateMaterial)) { mat = new Material(baseShader); }

        shouldRecreateMaterial = false;

        mat.mainTexture = innerSprite.texture;
        mat.mainTexture.wrapMode = TextureWrapMode.Repeat;

        if (innerTilingType == TilingType.SingleQuadChangeScaling)
        {
            mat.mainTextureScale = numberOfTiles;
        }

        if (mat != innerSharedMaterial)
        {
            mat.name = "Autogenerated Material";
        }

        mr.sharedMaterial = mat;
        mr.hideFlags = HideFlags.NotEditable;

        MeshFilter mf = this.gameObject.GetComponent<MeshFilter>();
        if (mf == null) { mf = this.gameObject.AddComponent<MeshFilter>(); }
        var mesh = tilingType == TilingType.SingleQuadChangeScaling ? GenerateSingleQuad() : GenerateMultipleQuads();
        mf.mesh = mesh;
        mf.hideFlags = HideFlags.NotEditable;
    }

    Mesh GenerateSingleQuad()
    {
        var mesh = new Mesh();
        mesh.name = "Autogenerated Quads (Single)";

        int columns = Mathf.CeilToInt(Mathf.Abs(numberOfTiles.x));
        int rows = Mathf.CeilToInt(Mathf.Abs(numberOfTiles.y));

        Vector3[] vertices = new Vector3[4];
        Vector3 globalOffset = new Vector3(columns * innerSprite.bounds.size.x / 2, rows * innerSprite.bounds.size.y / 2, 0);

        vertices[0] = new Vector3(0, 0, 0) - globalOffset; ;
        vertices[1] = new Vector3(innerSprite.bounds.size.x * numberOfTiles.x, 0, 0) - globalOffset; ;
        vertices[2] = new Vector3(0, innerSprite.bounds.size.y * numberOfTiles.y, 0) - globalOffset; ;
        vertices[3] = new Vector3(innerSprite.bounds.size.x * numberOfTiles.x, innerSprite.bounds.size.y * numberOfTiles.y, 0) - globalOffset; ;
        mesh.vertices = vertices;

        int[] tri = new int[vertices.Length * 6 / 4];
        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;

        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;
        mesh.SetTriangles(tri, 0);
        Vector3[] normals = new Vector3[vertices.Length];

        for (int n = 0; n < normals.Length; n++)
        {
            normals[n] = -Vector3.forward;
        }

        mesh.normals = normals;

        Vector2[] uv = new Vector2[vertices.Length];

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        mesh.uv = uv;
        return mesh;
    }

    Mesh GenerateMultipleQuads()
    {
        var mesh = new Mesh();
        mesh.name = "Autogenerated Quads (Multiple)";

        int columns = Mathf.CeilToInt(Mathf.Abs(numberOfTiles.x));
        int rows = Mathf.CeilToInt(Mathf.Abs(numberOfTiles.y));

        Vector3[] vertices = new Vector3[rows * columns * 4];
        Vector3 globalOffset = new Vector3(columns * innerSprite.bounds.size.x / 2, rows * innerSprite.bounds.size.y / 2, 0);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                var bottomleft = new Vector3(c * innerSprite.bounds.size.x, r * innerSprite.bounds.size.y, 0);
                var topleft = new Vector3(c * innerSprite.bounds.size.x, r * innerSprite.bounds.size.y + innerSprite.bounds.size.y, 0);
                var bottomright = new Vector3(c * innerSprite.bounds.size.x + innerSprite.bounds.size.x, r * innerSprite.bounds.size.y, 0);
                var topright = new Vector3(c * innerSprite.bounds.size.x + innerSprite.bounds.size.x, r * innerSprite.bounds.size.y + innerSprite.bounds.size.y, 0);

                vertices[c * 4 + r * columns * 4] = bottomleft - globalOffset;
                vertices[c * 4 + r * columns * 4 + 1] = bottomright - globalOffset;
                vertices[c * 4 + r * columns * 4 + 2] = topleft - globalOffset;
                vertices[c * 4 + r * columns * 4 + 3] = topright - globalOffset;
            }
        }

        mesh.vertices = vertices;

        int[] tri = new int[vertices.Length * 6 / 4];

        for (int t = 0; t < tri.Length; t += 6)
        {
            int value = t / 3 * 2;
            tri[t] = value;
            tri[t + 1] = value + 2;
            tri[t + 2] = value + 1;
            tri[t + 3] = value + 2;
            tri[t + 4] = value + 3;
            tri[t + 5] = value + 1;
        }

        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;

        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;

        mesh.SetTriangles(tri, 0);

        Vector3[] normals = new Vector3[vertices.Length];

        for (int n = 0; n < normals.Length; n++)
        {
            normals[n] = -Vector3.forward;
        }
        mesh.normals = normals;

        Vector2[] uv = new Vector2[vertices.Length];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                uv[c * 4 + r * columns * 4] = new Vector3(c, r, 0);
                uv[c * 4 + r * columns * 4 + 1] = new Vector3(c + 1, r, 0);
                uv[c * 4 + r * columns * 4 + 2] = new Vector3(c, r + 1, 0);
                uv[c * 4 + r * columns * 4 + 3] = new Vector3(c + 1, r + 1, 0);
            }
        }

        mesh.uv = uv;
        return mesh;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            GenerateQuads();

            // Recreates material if object is a copy
            if (instanceID != GetInstanceID())
            {
                forceUpdate = true;
                if (instanceID == 0)
                {
                    instanceID = GetInstanceID();
                }
                else
                {
                    instanceID = GetInstanceID();
                    if (instanceID < 0)
                    {
                        shouldRecreateMaterial = true;
                    }
                }
            }
        }
#endif
    }

}