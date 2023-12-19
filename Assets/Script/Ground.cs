using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    private Texture2D baseTexture;

    private Texture2D cloneTexture;

    public GameObject fire;

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

    Vector2Int World2Pixel(Vector2 pos)
    {
        Vector2Int v = Vector2Int.zero;
        float dx = (pos.x - transform.position.x);
        float dy = (pos.y - transform.position.y);

        v.x = Mathf.RoundToInt(0.5f * WidthPixel + dx * (WidthPixel / WidthWorld));
        v.y = Mathf.RoundToInt(0.5f * HeightPixel + dy * (HeightPixel / HeightWorld));

        return v;
    }

    void MakeAHole(CircleCollider2D col)
    {
        Vector2Int c = World2Pixel(col.bounds.center);

        int r = Mathf.RoundToInt((col.bounds.size.x * WidthPixel / WidthWorld) * 2);

        int px, nx, py, ny, d;

        for (int i = 0; i <= r; i++)
        {
            d = Mathf.RoundToInt(Mathf.Sqrt(r * r - i * r));
            for (int j = 0; j <= d; j++)
            {
                px = c.x + i;
                nx = c.x - i;
                py = c.y + j;
                ny = c.y - j;

                cloneTexture.SetPixel(px, py, Color.clear);
                cloneTexture.SetPixel(nx, py, Color.clear);
                cloneTexture.SetPixel(px, ny, Color.clear);
                cloneTexture.SetPixel(nx, ny, Color.clear);
            }
        }

        cloneTexture.Apply();
        UpdateTexture();

        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;
        if (!collision.GetComponent<CircleCollider2D>())
            return;

        MakeAHole(collision.GetComponent<CircleCollider2D>());
        GameObject temp = Instantiate(fire, collision.transform.position, collision.transform.rotation);
        temp.transform.position = new Vector3(temp.transform.position.x,temp.transform.position.y, -10);
        temp.transform.localScale = new Vector3(0.01f,0.01f,1);
        Destroy(temp, 1f);
        Destroy(collision.gameObject, 0.1f);
        
    }

}


