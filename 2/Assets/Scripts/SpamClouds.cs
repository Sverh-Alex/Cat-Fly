using System.Collections.Generic;
using UnityEngine;

// Универсальный спавнер объектов через пул
public class SpamClouds : MonoBehaviour
{
    [System.Serializable]
    public class SpawnRule
    {
        public string poolTag;          // тег пула (должен совпадать с тегом в ObjectPoolManager)
        public float spamInterval = 1f; // интервал спавна (секунды)
        public float maxSpeed = 5f;     // максимальная скорость спавна

        [System.NonSerialized] public float timer; // таймер для отсчёта времени до следующего спавна
    }

    [SerializeField] private GameObject[] spawnPoints;  // общие точки спавна для всех объектов
    [SerializeField] private List<SpawnRule> spawnRules = new List<SpawnRule>(); // список правил спавна

    void Start()
    {
        // Проверяем, что есть точки спавна
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpamClouds: no spawn points assigned!");
        }

        // Инициализируем таймеры для каждого правила
        foreach (var rule in spawnRules)
        {
            rule.timer = 0f;
        }
    }

    void Update()
    {
        // Если нет точек спавна или правил — ничего не делаем
        if (spawnPoints == null || spawnPoints.Length == 0 || spawnRules == null || spawnRules.Count == 0)
        {
            return;
        }

        // Для каждого правила проверяем, пришло ли время спавна
        foreach (var rule in spawnRules)
        {
            rule.timer += Time.deltaTime; // увеличиваем таймер

            if (rule.timer >= rule.spamInterval) // если пришло время спавна
            {
                SpawnFromRule(rule); // спавним объект
                rule.timer = 0f; // сбрасываем таймер
            }
        }
    }

    private void SpawnFromRule(SpawnRule rule)
    {
        // Выбираем случайную точку спавна из общего массива
        int index = Random.Range(0, spawnPoints.Length);
        Vector3 position = spawnPoints[index].transform.position;

        // Выбираем случайную скорость от 0 до максимума
        float speed = Random.Range(0f, rule.maxSpeed);

        // Спавним объект из пула
        ObjectPoolManager.Instance.SpawnFromPool(rule.poolTag, position, Quaternion.identity, speed);
    }
}