using UnityEngine;

// Облако, наследуется от ObjectsBaseMovable
public class Cloud : ObjectsBaseMovable
{
    private SpriteRenderer spriteRenderer;                // ссылка на спрайт-рендерер

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // кэшируем SpriteRenderer при старте объекта
    }

    protected override void OnCustomInit(float scale)
    {
        // Цвет в зависимости от скорости (чем быстрее, тем менее прозрачное)
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;           // берём текущий цвет
            color.a = (currentSpeed / 10f) + 0.1f;        // вычисляем прозрачность
            spriteRenderer.color = color;                 // применяем новый цвет
        }

        // Размер облака зависит от скорости
        transform.localScale = new Vector3(scale * currentSpeed / 10f, scale * currentSpeed / 10f, 1f);

        // Глубина (Z) зависит от скорости (чем быстрее, тем ближе к камере)
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            (-currentSpeed * 2f) + 10f
        );
    }
    protected override void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool("Cloud", gameObject); // возвращаем в пул с тегом "Cloud"
    }
}