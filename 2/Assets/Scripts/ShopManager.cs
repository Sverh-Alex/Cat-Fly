using UnityEngine;
using UnityEngine.UI;

public class ButtonToggleManager : MonoBehaviour
{
    public Button[] buttons; // Все кнопки на сцене
    public int id;
    public int selectId = 0;

    void Start()
    {
        // Загружаем индекс отключённой кнопки из памяти (если есть)
        selectId = PlayerPrefs.GetInt("SelectButton", 0);
        UpdateButtons();
    }

    // Этот метод вызывается при нажатии на кнопку
    public void OnButtonClick(int buttonIndex)
    {
        selectId = buttonIndex;  // Запоминаем, какую кнопку отключить
        PlayerPrefs.SetInt("SelectButton", selectId);  // Сохраняем в память
        PlayerPrefs.Save();

        UpdateButtons();  // Обновляем состояние кнопок
    }

    // Включаем все кнопки, кроме отключённой
    void UpdateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(i != selectId);
        }
    }
}
