using System; // Подключает Serializable
using System.Collections; // Подключает IEnumerator и Coroutine
using UnityEngine; // Подключает Unity API

public class BackGroundScript : MonoBehaviour
{
    [Serializable] // Позволяет настраивать один слой фона в Inspector
    private class BackgroundLayer
    {
        [SerializeField] private SpriteRenderer background; // Единственный исходный SpriteRenderer слоя
        [SerializeField, Min(0f)] private float speed = 1f; // Скорость движения слоя в мировых единицах в секунду
        [SerializeField, Min(1f)] private float heightMultiplier = 1.05f; // Запас размера фона по высоте камеры

        [NonSerialized] private SpriteRenderer runtimeCopy; // Копия фона, созданная во время игры

        public SpriteRenderer Background => background; // Возвращает исходный SpriteRenderer слоя
        public SpriteRenderer RuntimeCopy => runtimeCopy; // Возвращает копию SpriteRenderer, созданную в рантайме
        public float Speed => speed; // Возвращает скорость текущего слоя
        public float HeightMultiplier => heightMultiplier; // Возвращает запас размера слоя по высоте

        public void SetRuntimeCopy(SpriteRenderer copy) // Сохраняет созданную копию в текущем слое
        {
            runtimeCopy = copy; // Присваивает копию внутреннему полю
        }
    }

    [Header("Камера")]
    [SerializeField] private Camera gameCamera; // Игровая ортографическая камера

    [Header("Фоновые слои")]
    [SerializeField] private BackgroundLayer[] layers; // Массив слоёв фона

    [Header("Начальная позиция")]
    [SerializeField] private float startOffsetX = 0f; // Смещает все фоновые слои по X относительно центра камеры

    [Header("Запуск")]
    [SerializeField, Min(0f)] private float startDelay = 0f; // Задержка перед началом движения

    private bool movementStarted; // Показывает, разрешено ли движение фона
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
        if (gameCamera == null) // Проверяет, найдена ли игровая камера
        {
            Debug.LogError("Не назначена игровая камера в BackGroundScript."); // Выводит ошибку в Console
            enabled = false; // Отключает скрипт
            return; // Завершает выполнение Start
        }

        if (!gameCamera.orthographic) // Проверяет, является ли камера ортографической
        {
            Debug.LogError("Для BackGroundScript камера должна быть в режиме Orthographic."); // Выводит ошибку в Console
            enabled = false; // Отключает скрипт
            return; // Завершает выполнение Start
        }

        CreateRuntimeCopies(); // Создаёт по одной копии для каждого фонового слоя
        SetupAllLayers(); // Масштабирует и расставляет исходные фоны и их копии
        StartCoroutine(StartMovementAfterDelay()); // Запускает задержку перед началом движения
    }

    private IEnumerator StartMovementAfterDelay()
    {
        if (startDelay > 0f) // Проверяет, задана ли стартовая задержка
        {
            yield return new WaitForSecondsRealtime(startDelay); // Ждёт указанное реальное время
        }

        movementStarted = true; // Разрешает движение всех слоёв
    }

    private void Update()
    {
        if (gameCamera == null) // Защищает от ошибок при отсутствии камеры
        {
            return; // Завершает Update
        }

        UpdateLayoutIfScreenChanged(); // Проверяет изменение размера экрана или Orthographic Size камеры

        if (!movementStarted) // Проверяет, закончилась ли стартовая задержка
        {
            return; // Не двигает фон до окончания задержки
        }

        foreach (BackgroundLayer layer in layers) // Перебирает все слои фона
        {
            MoveLayer(layer); // Двигает и зацикливает текущий слой
        }
    }

    private void CreateRuntimeCopies()
    {
        foreach (BackgroundLayer layer in layers) // Перебирает все слои массива
        {
            if (!IsLayerValid(layer)) // Проверяет, назначен ли исходный фон
            {
                continue; // Пропускает пустой или неверно настроенный слой
            }

            SpriteRenderer originalBackground = layer.Background; // Получает исходный SpriteRenderer текущего слоя
            GameObject copyObject = Instantiate(originalBackground.gameObject); // Создаёт полную копию объекта фона

            copyObject.name = originalBackground.name + "_RuntimeCopy"; // Задаёт понятное имя созданной копии
            copyObject.transform.SetParent(originalBackground.transform.parent, true); // Помещает копию к тому же родителю без изменения мировой позиции

            SpriteRenderer copyRenderer = copyObject.GetComponent<SpriteRenderer>(); // Получает SpriteRenderer на созданной копии

            layer.SetRuntimeCopy(copyRenderer); // Сохраняет созданную копию в текущем слое
        }
    }

    private void SetupAllLayers()
    {
        previousAspect = gameCamera.aspect; // Сохраняет актуальное соотношение сторон камеры
        previousOrthographicSize = gameCamera.orthographicSize; // Сохраняет актуальный размер ортографической камеры

        foreach (BackgroundLayer layer in layers) // Перебирает все фоновые слои
        {
            SetupLayer(layer); // Подгоняет размер и начальные позиции слоя
        }
    }

    private void SetupLayer(BackgroundLayer layer)
    {
        if (!IsRuntimeLayerValid(layer)) // Проверяет наличие исходного фона и созданной копии
        {
            return; // Пропускает неверно настроенный слой
        }

        SpriteRenderer originalBackground = layer.Background; // Получает исходное изображение слоя
        SpriteRenderer copyBackground = layer.RuntimeCopy; // Получает созданную копию изображения

        float targetHeight = gameCamera.orthographicSize * 2f * layer.HeightMultiplier; // Вычисляет требуемую высоту фона
        float spriteHeight = originalBackground.sprite.bounds.size.y; // Получает исходную высоту спрайта в мировых единицах

        if (spriteHeight <= 0f) // Проверяет, корректна ли высота спрайта
        {
            Debug.LogError("У спрайта фона некорректная высота."); // Выводит ошибку в Console
            return; // Завершает настройку текущего слоя
        }

        float scale = targetHeight / spriteHeight; // Вычисляет масштаб для заполнения высоты камеры
        Vector3 newScale = new Vector3(scale, scale, 1f); // Создаёт равномерный масштаб без искажения изображения

        originalBackground.transform.localScale = newScale; // Масштабирует исходное изображение
        copyBackground.transform.localScale = newScale; // Масштабирует копию точно так же

        float tileWidth = GetTileWidth(originalBackground); // Получает ширину одного изображения после масштабирования
        float cameraX = gameCamera.transform.position.x; // Получает мировую позицию камеры по X
        float cameraY = gameCamera.transform.position.y; // Получает мировую позицию камеры по Y
        float backgroundStartX = cameraX + startOffsetX; // Вычисляет стартовую позицию фона с учётом смещения из Inspector

        Vector3 originalPosition = originalBackground.transform.position; // Получает текущую мировую позицию исходного фона
        originalPosition.x = backgroundStartX; // Ставит исходный фон с учётом Start Offset X
        originalPosition.y = cameraY; // Выравнивает исходный фон по центру камеры по Y
        originalPosition.z = 0f; // Устанавливает мировой Z исходного фона равным нулю
        originalBackground.transform.position = originalPosition; // Применяет новую мировую позицию исходного фона

        Vector3 copyPosition = copyBackground.transform.position; // Получает текущую мировую позицию копии
        copyPosition.x = backgroundStartX + tileWidth; // Ставит копию вплотную справа от исходного фона
        copyPosition.y = cameraY; // Выравнивает копию по центру камеры по Y
        copyPosition.z = 0f; // Устанавливает мировой Z копии равным нулю
        copyBackground.transform.position = copyPosition; // Применяет новую мировую позицию копии
    }

    private void UpdateLayoutIfScreenChanged()
    {
        bool aspectChanged = !Mathf.Approximately(gameCamera.aspect, previousAspect); // Проверяет изменение пропорций Game View
        bool orthographicSizeChanged = !Mathf.Approximately(gameCamera.orthographicSize, previousOrthographicSize); // Проверяет изменение масштаба камеры

        if (aspectChanged || orthographicSizeChanged) // Проверяет, нужен ли пересчёт фона
        {
            SetupAllLayers(); // Повторно масштабирует и расставляет фоны под новый размер экрана
        }
    }

    private void MoveLayer(BackgroundLayer layer)
    {
        if (!IsRuntimeLayerValid(layer)) // Проверяет, настроен ли слой
        {
            return; // Не двигает некорректный слой
        }

        SpriteRenderer originalBackground = layer.Background; // Получает исходное изображение слоя
        SpriteRenderer copyBackground = layer.RuntimeCopy; // Получает копию изображения слоя

        float movementDistance = layer.Speed * Time.deltaTime; // Вычисляет расстояние движения за текущий кадр

        originalBackground.transform.position += Vector3.left * movementDistance; // Двигает исходное изображение влево
        copyBackground.transform.position += Vector3.left * movementDistance; // Двигает созданную копию влево

        float cameraLeftEdge = gameCamera.transform.position.x - GetCameraWidth() * 0.5f; // Вычисляет левую границу камеры в мировых координатах

        if (originalBackground.bounds.max.x < cameraLeftEdge) // Проверяет, ушёл ли исходный фон полностью за левый край экрана
        {
            MoveTileToRight(originalBackground, copyBackground); // Переносит исходный фон вправо за копию
        }

        if (copyBackground.bounds.max.x < cameraLeftEdge) // Проверяет, ушла ли копия полностью за левый край экрана
        {
            MoveTileToRight(copyBackground, originalBackground); // Переносит копию вправо за исходный фон
        }
    }

    private void MoveTileToRight(SpriteRenderer tileToMove, SpriteRenderer rightTile)
    {
        float movingTileHalfWidth = tileToMove.bounds.extents.x; // Получает половину ширины перемещаемого изображения
        float rightTileRightEdge = rightTile.bounds.max.x; // Получает правую границу второго изображения

        Vector3 newPosition = tileToMove.transform.position; // Получает текущую мировую позицию переносимого изображения
        newPosition.x = rightTileRightEdge + movingTileHalfWidth; // Ставит переносимое изображение сразу справа от второго
        tileToMove.transform.position = newPosition; // Применяет новую мировую позицию
    }

    private float GetTileWidth(SpriteRenderer tile)
    {
        return tile.bounds.size.x; // Возвращает фактическую ширину изображения с учётом масштаба
    }

    private float GetCameraWidth()
    {
        return gameCamera.orthographicSize * 2f * gameCamera.aspect; // Возвращает ширину области, видимой ортографической камерой
    }

    private bool IsLayerValid(BackgroundLayer layer)
    {
        return layer != null && layer.Background != null && layer.Background.sprite != null; // Проверяет наличие слоя, SpriteRenderer и назначенного спрайта
    }

    private bool IsRuntimeLayerValid(BackgroundLayer layer)
    {
        return IsLayerValid(layer) && layer.RuntimeCopy != null && layer.RuntimeCopy.sprite != null; // Проверяет наличие исходного фона и созданной копии
    }
}