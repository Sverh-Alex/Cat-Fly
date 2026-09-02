using System; // Подключает Serializable
using System.Collections; // Подключает IEnumerator и Coroutine
using UnityEngine; // Подключает Unity API

public class BackGroundScript : MonoBehaviour
{
    [Serializable] // Позволяет настраивать один слой фона в Inspector
    private class BackgroundLayer
    {
        [SerializeField] private SpriteRenderer background; // Единственный исходный SpriteRenderer слоя
        [SerializeField, Min(0f)] private float speed = 1f; // Максимальная скорость движения слоя
        [SerializeField, Min(0f)] private float accelerationDuration = 2f; // Продолжительность плавного разгона слоя
        [SerializeField, Min(1f)] private float heightMultiplier = 1.05f; // Запас размера фона по высоте камеры

        [NonSerialized] private SpriteRenderer runtimeCopy; // Копия фона, созданная во время игры
        [NonSerialized] private float currentSpeed; // Текущая скорость движения слоя

        public SpriteRenderer Background => background; // Возвращает исходный SpriteRenderer слоя
        public SpriteRenderer RuntimeCopy => runtimeCopy; // Возвращает копию SpriteRenderer слоя
        public float Speed => speed; // Возвращает максимальную скорость слоя
        public float AccelerationDuration => accelerationDuration; // Возвращает продолжительность разгона
        public float HeightMultiplier => heightMultiplier; // Возвращает множитель высоты слоя
        public float CurrentSpeed => currentSpeed; // Возвращает текущую скорость слоя

        public void SetRuntimeCopy(SpriteRenderer copy) // Сохраняет созданную копию фона
        {
            runtimeCopy = copy; // Присваивает копию внутреннему полю
        }

        public void ResetSpeed() // Сбрасывает текущую скорость слоя в ноль
        {
            currentSpeed = 0f; // Устанавливает начальную скорость слоя
        }

        public void UpdateSpeed(float elapsedTime) // Плавно увеличивает скорость слоя
        {
            if (accelerationDuration <= 0f) // Проверяет, отключён ли плавный разгон
            {
                currentSpeed = speed; // Сразу устанавливает максимальную скорость
                return; // Завершает метод
            }

            float progress = Mathf.Clamp01(elapsedTime / accelerationDuration); // Вычисляет прогресс разгона от нуля до единицы
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress); // Сглаживает начало и окончание разгона
            currentSpeed = Mathf.Lerp(0f, speed, smoothProgress); // Вычисляет текущую скорость от нуля до заданной
        }
    }

    [Header("Камера")]
    [SerializeField] private Camera gameCamera; // Игровая ортографическая камера

    [Header("Фоновые слои")]
    [SerializeField] private BackgroundLayer[] layers; // Массив слоёв фона

    [Header("Начальная позиция")]
    [SerializeField] private float startOffsetX = 0f; // Смещает все фоновые слои по X относительно центра камеры

    [Header("Запуск")]
    [SerializeField, Min(0f)] private float startDelay = 0f; // Задержка перед началом разгона фона

    private bool movementStarted; // Показывает, закончилась ли задержка перед разгоном
    private float accelerationElapsedTime; // Хранит время, прошедшее с начала разгона
    private float previousAspect; // Хранит прошлое соотношение сторон камеры
    private float previousOrthographicSize; // Хранит прошлый размер ортографической камеры

    private void Awake()
    {
        if (gameCamera == null) // Проверяет, назначена ли камера в Inspector
        {
            gameCamera = Camera.main; // Пытается найти камеру с тегом MainCamera
        }
    }

    private void Start()
    {
        if (gameCamera == null) // Проверяет, найдена ли камера
        {
            Debug.LogError("Не назначена игровая камера в BackGroundScript."); // Выводит ошибку в Console
            enabled = false; // Отключает скрипт
            return; // Завершает выполнение Start
        }

        if (!gameCamera.orthographic) // Проверяет, используется ли ортографическая камера
        {
            Debug.LogError("Для BackGroundScript камера должна быть в режиме Orthographic."); // Выводит ошибку в Console
            enabled = false; // Отключает скрипт
            return; // Завершает выполнение Start
        }

        CreateRuntimeCopies(); // Создаёт по одной копии для каждого слоя
        ResetAllLayerSpeeds(); // Устанавливает нулевую скорость всем слоям
        SetupAllLayers(); // Масштабирует и расставляет исходные фоны и копии
        StartCoroutine(StartMovementAfterDelay()); // Запускает ожидание перед разгоном
    }

    private IEnumerator StartMovementAfterDelay()
    {
        if (startDelay > 0f) // Проверяет, установлена ли задержка
        {
            yield return new WaitForSecondsRealtime(startDelay); // Ждёт указанное реальное время
        }

        accelerationElapsedTime = 0f; // Сбрасывает время разгона перед стартом
        movementStarted = true; // Разрешает начало плавного разгона
    }

    private void Update()
    {
        if (gameCamera == null) // Проверяет наличие игровой камеры
        {
            return; // Завершает Update
        }

        UpdateLayoutIfScreenChanged(); // Проверяет изменение размера экрана или камеры

        if (!movementStarted) // Проверяет, закончилась ли задержка
        {
            return; // Не двигает фон до окончания задержки
        }

        accelerationElapsedTime += Time.deltaTime; // Увеличивает время плавного разгона

        foreach (BackgroundLayer layer in layers) // Перебирает каждый слой фона
        {
            layer.UpdateSpeed(accelerationElapsedTime); // Плавно увеличивает скорость текущего слоя
            MoveLayer(layer); // Двигает и зацикливает текущий слой
        }
    }

    private void CreateRuntimeCopies()
    {
        foreach (BackgroundLayer layer in layers) // Перебирает все слои массива
        {
            if (!IsLayerValid(layer)) // Проверяет корректность исходного фона
            {
                continue; // Пропускает неверно настроенный слой
            }

            SpriteRenderer originalBackground = layer.Background; // Получает исходный SpriteRenderer
            GameObject copyObject = Instantiate(originalBackground.gameObject); // Создаёт копию исходного объекта

            copyObject.name = originalBackground.name + "_RuntimeCopy"; // Задаёт имя копии
            copyObject.transform.SetParent(originalBackground.transform.parent, true); // Помещает копию к тому же родителю

            SpriteRenderer copyRenderer = copyObject.GetComponent<SpriteRenderer>(); // Получает SpriteRenderer копии

            layer.SetRuntimeCopy(copyRenderer); // Сохраняет копию внутри текущего слоя
        }
    }

    private void ResetAllLayerSpeeds()
    {
        foreach (BackgroundLayer layer in layers) // Перебирает все слои
        {
            if (!IsLayerValid(layer)) // Проверяет корректность слоя
            {
                continue; // Пропускает неверно настроенный слой
            }

            layer.ResetSpeed(); // Устанавливает текущую скорость слоя в ноль
        }
    }

    private void SetupAllLayers()
    {
        previousAspect = gameCamera.aspect; // Сохраняет текущее соотношение сторон камеры
        previousOrthographicSize = gameCamera.orthographicSize; // Сохраняет текущий размер камеры

        foreach (BackgroundLayer layer in layers) // Перебирает каждый слой фона
        {
            SetupLayer(layer); // Масштабирует и размещает текущий слой
        }
    }

    private void SetupLayer(BackgroundLayer layer)
    {
        if (!IsRuntimeLayerValid(layer)) // Проверяет наличие исходного фона и копии
        {
            return; // Пропускает неверно настроенный слой
        }

        SpriteRenderer originalBackground = layer.Background; // Получает исходный фон
        SpriteRenderer copyBackground = layer.RuntimeCopy; // Получает копию исходного фона

        float targetHeight = gameCamera.orthographicSize * 2f * layer.HeightMultiplier; // Вычисляет требуемую высоту фона
        float spriteHeight = originalBackground.sprite.bounds.size.y; // Получает исходную высоту спрайта

        if (spriteHeight <= 0f) // Проверяет, корректна ли высота спрайта
        {
            Debug.LogError("У спрайта фона некорректная высота."); // Выводит ошибку в Console
            return; // Завершает настройку текущего слоя
        }

        float scale = targetHeight / spriteHeight; // Вычисляет равномерный масштаб фона
        Vector3 newScale = new Vector3(scale, scale, 1f); // Создаёт масштаб без искажения изображения

        originalBackground.transform.localScale = newScale; // Масштабирует исходный фон
        copyBackground.transform.localScale = newScale; // Масштабирует копию фона

        float tileWidth = GetTileWidth(originalBackground); // Получает ширину одного изображения
        float cameraX = gameCamera.transform.position.x; // Получает позицию камеры по X
        float cameraY = gameCamera.transform.position.y; // Получает позицию камеры по Y
        float backgroundStartX = cameraX + startOffsetX; // Вычисляет начальную позицию фона по X

        Vector3 originalPosition = originalBackground.transform.position; // Получает текущую мировую позицию исходного фона
        originalPosition.x = backgroundStartX; // Устанавливает начальную позицию исходного фона
        originalPosition.y = cameraY; // Выравнивает исходный фон по центру камеры по Y
        originalPosition.z = 0f; // Устанавливает мировой Z исходного фона
        originalBackground.transform.position = originalPosition; // Применяет позицию исходного фона

        Vector3 copyPosition = copyBackground.transform.position; // Получает текущую мировую позицию копии
        copyPosition.x = backgroundStartX + tileWidth; // Ставит копию справа от исходного фона
        copyPosition.y = cameraY; // Выравнивает копию по центру камеры по Y
        copyPosition.z = 0f; // Устанавливает мировой Z копии
        copyBackground.transform.position = copyPosition; // Применяет позицию копии
    }

    private void UpdateLayoutIfScreenChanged()
    {
        bool aspectChanged = !Mathf.Approximately(gameCamera.aspect, previousAspect); // Проверяет изменение пропорций экрана
        bool cameraSizeChanged = !Mathf.Approximately(gameCamera.orthographicSize, previousOrthographicSize); // Проверяет изменение размера камеры

        if (aspectChanged || cameraSizeChanged) // Проверяет, нужно ли пересчитать фон
        {
            SetupAllLayers(); // Повторно подгоняет фон под размер камеры
        }
    }

    private void MoveLayer(BackgroundLayer layer)
    {
        if (!IsRuntimeLayerValid(layer)) // Проверяет корректность текущего слоя
        {
            return; // Не двигает неверно настроенный слой
        }

        SpriteRenderer originalBackground = layer.Background; // Получает исходный фон
        SpriteRenderer copyBackground = layer.RuntimeCopy; // Получает копию фона

        float movementDistance = layer.CurrentSpeed * Time.smoothDeltaTime; // Вычисляет расстояние движения с учётом текущей скорости

        originalBackground.transform.position += Vector3.left * movementDistance; // Двигает исходный фон влево
        copyBackground.transform.position += Vector3.left * movementDistance; // Двигает копию влево

        float cameraLeftEdge = gameCamera.transform.position.x - GetCameraWidth() * 0.5f; // Вычисляет левую границу камеры

        if (originalBackground.bounds.max.x < cameraLeftEdge) // Проверяет, ушёл ли исходный фон за левый край
        {
            MoveTileToRight(originalBackground, copyBackground); // Переносит исходный фон вправо
        }

        if (copyBackground.bounds.max.x < cameraLeftEdge) // Проверяет, ушла ли копия за левый край
        {
            MoveTileToRight(copyBackground, originalBackground); // Переносит копию вправо
        }
    }

    private void MoveTileToRight(SpriteRenderer tileToMove, SpriteRenderer rightTile)
    {
        float movingTileHalfWidth = tileToMove.bounds.extents.x; // Получает половину ширины перемещаемого фона
        float rightTileRightEdge = rightTile.bounds.max.x; // Получает правый край второго фона

        Vector3 newPosition = tileToMove.transform.position; // Получает мировую позицию переносимого фона
        newPosition.x = rightTileRightEdge + movingTileHalfWidth; // Ставит фон ровно справа от второго
        tileToMove.transform.position = newPosition; // Применяет новую позицию фона
    }

    private float GetTileWidth(SpriteRenderer tile)
    {
        return tile.bounds.size.x; // Возвращает фактическую ширину фона
    }

    private float GetCameraWidth()
    {
        return gameCamera.orthographicSize * 2f * gameCamera.aspect; // Возвращает ширину области камеры
    }

    private bool IsLayerValid(BackgroundLayer layer)
    {
        return layer != null && layer.Background != null && layer.Background.sprite != null; // Проверяет исходный фон и спрайт
    }

    private bool IsRuntimeLayerValid(BackgroundLayer layer)
    {
        return IsLayerValid(layer) && layer.RuntimeCopy != null && layer.RuntimeCopy.sprite != null; // Проверяет исходный фон и копию
    }
}