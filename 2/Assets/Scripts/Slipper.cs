using UnityEngine;

// Тапок, наследуется от ObjectsBaseMovable
public class Slipper : ObjectsBaseMovable
{
    [SerializeField] private int koff = 8;              // коэффициент размера тапка
    [SerializeField] public int damage = -1;            // урон игроку при столкновении

    protected override void OnCustomInit(float scale)
    {
        // Размер тапка зависит от коэффициента koff
        transform.localScale = new Vector3(scale / koff, scale / koff, 1f);
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
        ObjectPoolManager.Instance.ReturnToPool("Slipper", gameObject);
    }
}