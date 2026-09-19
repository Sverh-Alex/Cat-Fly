using System.Collections.Generic;
using UnityEngine;

// Менеджер пулов объектов
public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;          // тег пула (например, "Cloud")
        public GameObject prefab;   // префаб объекта
        public int size = 10;       // количество объектов в пуле
    }

    public static ObjectPoolManager Instance; // синглтон для доступа из любого места

    [SerializeField] private List<Pool> pools = new List<Pool>(); // список пулов

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> poolPrefabs = new Dictionary<string, GameObject>(); // храним префабы для расширения

    private void Awake()
    {
        // Проверяем, что есть пулы
        if (pools == null || pools.Count == 0)
        {
            Debug.LogWarning("ObjectPoolManager: no pools assigned!");
        }

        // Создаём синглтон
        if (Instance == null)
        {
            Instance = this;        // сохраняем ссылку на первый экземпляр
        }
        else
        {
            Destroy(gameObject);    // если уже есть Instance, удаляем дубль
            return;
        }

        // Создаём пулы
        foreach (Pool pool in pools)
        {
            // Проверяем, что префаб задан
            if (pool.prefab == null)
            {
                Debug.LogWarning("ObjectPoolManager: pool with tag " + pool.tag + " has no prefab!");
                continue;
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab); // создаём объект
                obj.SetActive(false);                      // сразу выключаем
                objectPool.Enqueue(obj);                   // добавляем в очередь пула
            }

            poolDictionary.Add(pool.tag, objectPool);      // добавляем пул в словарь
            poolPrefabs.Add(pool.tag, pool.prefab);        // сохраняем префаб для расширения
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, float speed)
    {
        // Проверяем, есть ли такой пул
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist!");
            return null;
        }

        GameObject objectToSpawn;

        // Если в пуле есть объекты — берём из пула
        if (poolDictionary[tag].Count > 0)
        {
            objectToSpawn = poolDictionary[tag].Dequeue(); // берём объект из пула

            // Проверяем, не уничтожен ли объект
            if (objectToSpawn == null)
            {
                // Если объект уничтожен — создаём новый
                Debug.LogWarning("Pooled object with tag " + tag + " was destroyed! Creating new object.");
                objectToSpawn = Instantiate(poolPrefabs[tag]);
                objectToSpawn.SetActive(false);
            }
        }
        else
        {
            // Если пул пуст — создаём новый объект (авто-расширение пула)
            //Debug.LogWarning("Pool with tag " + tag + " is empty! Creating new object dynamically.");
            objectToSpawn = Instantiate(poolPrefabs[tag]); // создаём новый объект из префаба
            objectToSpawn.SetActive(false);                // сразу выключаем (потом включим)
        }

        // Ещё раз проверяем, что объект существует
        if (objectToSpawn == null)
        {
            Debug.LogError("Failed to spawn object with tag " + tag + "! Prefab might be missing.");
            return null;
        }

        objectToSpawn.SetActive(true);                    // включаем объект
        objectToSpawn.transform.position = position;      // ставим на позицию
        objectToSpawn.transform.rotation = rotation;      // ставим поворот

        // Инициализируем скорость через метод Init (есть у Cloud/Feather/Gift/Slipper)
        ObjectsBaseMovable movable = objectToSpawn.GetComponent<ObjectsBaseMovable>();
        if (movable != null)
        {
            movable.Init(speed);                          // вызываем Init с переданной скоростью
        }

        // НЕ возвращаем объект в пул сразу! Он вернётся сам через ReturnToPool()
        // poolDictionary[tag].Enqueue(objectToSpawn); // <-- ЭТА СТРОКА БЫЛА ОШИБКОЙ

        return objectToSpawn;
    }

    // Метод для возврата объекта в пул (вызывается из ReturnToPool() в объектах)
    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist! Cannot return object.");
            return;
        }

        obj.SetActive(false);                             // выключаем объект
        poolDictionary[tag].Enqueue(obj);                 // возвращаем в пул
    }
}