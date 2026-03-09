// SkinChanger.cs: Повесьте на персонажа. Меняет всё по skinIndex из PlayerPrefs.
// Оптимально: Загрузка один раз в Awake, pooling-ready.
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    [Header("Компоненты (назначьте в инспекторе)")]
    public SpriteRenderer spriteRenderer;     // SpriteRenderer персонажа
    public Animator animator;                 // Animator
    public BoxCollider2D boxCollider;         // BoxCollider2D

    [Header("Данные скинов (drag all SO сюда)")]
    public SkinData[] allSkins;               // Массив SO (неограниченно добавляйте)

    [Header("Сохранение")]
    public string skinPrefsKey = "skin";      // Ключ PlayerPrefs
    private int currentSkinIndex = 0;         // Текущий скин (0 по умолчанию)

    void Awake()
    {
        // Получаем индекс скина из PlayerPrefs (если нет - 0).
        currentSkinIndex = PlayerPrefs.GetInt(skinPrefsKey, 0);
        ApplySkin(currentSkinIndex); // Применяем сразу при загрузке сцены
    }

    // Публичный метод: Меняем скин (вызывайте из UI/магазина).
    public void ChangeSkin(int newSkinIndex)
    {
        if (newSkinIndex < 0 || newSkinIndex >= allSkins.Length)
        {
            Debug.LogWarning("Неверный skinIndex: " + newSkinIndex + ". Доступно: 0-" + (allSkins.Length - 1));
            return;
        }

        currentSkinIndex = newSkinIndex;
        ApplySkin(currentSkinIndex);
        PlayerPrefs.SetInt(skinPrefsKey, currentSkinIndex); // Сохраняем
        PlayerPrefs.Save(); // Физическое сохранение на диск
    }

    // Применяем данные скина: Быстро, без GC.
    private void ApplySkin(int index)
    {
        SkinData skin = allSkins[index];

        // Меняем спрайт (если в Sprite Atlas - батчинг!).
        if (spriteRenderer != null && skin.skinSprite != null)
            spriteRenderer.sprite = skin.skinSprite;

        // Меняем аниматор (AOC для разных анимаций).
        if (animator != null && skin.animatorController != null)
            animator.runtimeAnimatorController = skin.animatorController;

        // Меняем коллайдер (offset/size под скин, physics обновится автоматически).
        if (boxCollider != null)
        {
            boxCollider.offset = skin.colliderOffset;
            boxCollider.size = skin.colliderSize;
        }

        Debug.Log("Скин изменён на: " + index + " (" + skin.name + ")");
    }

    // Геттер для UI (текущий индекс).
    public int GetCurrentSkinIndex() => currentSkinIndex;
}
