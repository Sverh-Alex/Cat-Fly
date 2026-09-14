using UnityEngine;

// Подарок, наследуется от ObjectsBaseMovable
public class Gift : ObjectsBaseMovable
{
    [SerializeField] private int koff = 6;              // коэффициент размера подарка

    public int upBullet = 1;                              // сколько пуль добавить при подборе (используется в ScoreManager)

    protected override void OnCustomInit(float scale)
    {
        // Размер подарка зависит от коэффициента koff
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
        ObjectPoolManager.Instance.ReturnToPool("Gift", gameObject);
    }
}