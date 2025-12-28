using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [Header("Scroll Settings")]
    [Range(0f, 5f)]
    public float scrollSpeed = 1f;  // Скорость ВЛЕВО (положительное значение)

    private Renderer rend;
    private Material mat;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            mat = new Material(rend.sharedMaterial); // КОПИЯ материала
            rend.material = mat;
        }
    }

    void Update()
    {
        if (mat == null) return;

        // ДВИЖЕНИЕ ВЛЕВО: offset.x УБАВЛЯЕТСЯ
        mat.mainTextureOffset += Vector2.right * scrollSpeed * Time.deltaTime * -1f;
    }
}
