using UnityEngine;

// Перо, наследуется от ObjectsBaseMovable
public class Feather : ObjectsBaseMovable
{
    [SerializeField] private float scaleSize = 4f;      // коэффициент размера пера

    protected override void OnCustomInit(float scale)
    {
        // Размер пера зависит от коэффициента scaleSize
        transform.localScale = new Vector3(scale / scaleSize, scale / scaleSize, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("rainbow")) // если столкнулось с игроком
        {
            ReturnToPool();                             // возвращаем в пул
        }
    }
    protected override void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool("Feather", gameObject);
    }

}