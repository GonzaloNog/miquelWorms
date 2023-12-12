using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    private Texture2D baseTexture;

    private Texture2D cloneTexture;

    private SpriteRenderer spriteRenderer;

    private float _widthWorld, _heightWorld;
    private float _widthPixel, _heightPixel;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        cloneTexture = Instantiate(baseTexture);
        cloneTexture.alphaIsTransparency = true;

        UpdateTexture();

        gameObject.AddComponent<PolygonCollider2D>();
    }

    public float WidthWorld
    {
        get
        {
            if (_widthWorld == 0)
                _widthWorld = spriteRenderer.bounds.size.x;
            return _widthWorld;
        }
    }

    public float HeightWorld
    {
        get
        {
            if (_heightWorld == 0)
                _heightWorld = spriteRenderer.bounds.size.y;
            return _heightWorld;
        }
    }

    public float WidthPixel
    {
        get
        {
            if (_widthPixel == 0)
                _widthPixel = spriteRenderer.sprite.texture.width;
            return _widthPixel;
        }
    }

    public float HeightPixel
    {
        get
        {
            if (_heightPixel == 0)
                _heightPixel = spriteRenderer.sprite.texture.height;
            return _heightPixel;
        }
    }

    void UpdateTexture()
    {
        spriteRenderer.sprite = Sprite.Create(cloneTexture,
            new Rect(0, 0, cloneTexture.width, cloneTexture.height),
            new Vector2(0.5f, 0.5f), 50f);
    }
}


