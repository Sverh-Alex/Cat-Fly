// SkinData.cs: Хранит данные одного скина. Создавайте новые файлы SO для каждого скина.
// Неограниченно: Просто создайте SkinData_Nовый.asset и заполните.
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkinData", menuName = "Game/SkinData", order = 1)]
public class SkinData : ScriptableObject
{
    [Header("Визуал")]
    public Sprite skinSprite; // Спрайт для SpriteRenderer

    [Header("Анимация")]
    public RuntimeAnimatorController animatorController; // AOC или Controller для скина

    [Header("Коллайдер (адаптирован под скин)")]
    public Vector2 colliderOffset; // Offset для BoxCollider2D
    public Vector2 colliderSize;   // Size для BoxCollider2D
}
