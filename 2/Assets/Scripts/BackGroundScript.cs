using UnityEngine;

public class BackGroundScript : MonoBehaviour
{
    [Header("Background Layers")]
    [SerializeField] private GameObject[] backgrounds = new GameObject[3]; // Fon1, Fon2, Fon3
    [SerializeField] private float[] speeds = { 9f, 6f, 3f }; // Скорости слоёв

    [Header("Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float resetDistance = 40f; // Расстояние для перестановки

    private Vector3[] startPositions;
    private float[] backgroundWidths;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Инициализация массивов
        startPositions = new Vector3[backgrounds.Length];
        backgroundWidths = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i] != null)
            {
                startPositions[i] = backgrounds[i].transform.position;
                backgroundWidths[i] = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i] == null) continue;

            GameObject bg = backgrounds[i];

            // Двигаем фон влево
            bg.transform.Translate(Vector3.right * -speeds[i] * Time.deltaTime);

            // Проверяем, ушёл ли фон за экран
            float distanceFromStart = bg.transform.position.x - startPositions[i].x;

            if (distanceFromStart < -backgroundWidths[i] - resetDistance)
            {
                // ПЕРЕНОСИМ В КОНЕЦ
                bg.transform.position = new Vector3(
                    startPositions[i].x + backgroundWidths[i] * 2,
                    startPositions[i].y,
                    startPositions[i].z
                );
            }
        }
    }
}
