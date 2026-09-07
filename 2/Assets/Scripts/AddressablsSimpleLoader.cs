using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressblsSimpleLoader : MonoBehaviour
{
    [Header("Список адресов префабов/ассетов для загрузки")]
    [SerializeField] private List<string> addresses = new List<string>();

    [Header("Родитель для спавна (опционально)")]
    [SerializeField] private Transform container;

    private List<AsyncOperationHandle<GameObject>> handles = new List<AsyncOperationHandle<GameObject>>();

    // Загрузить все ассеты из списка
    public void LoadAll()
    {
        UnloadAll();

        foreach (var address in addresses)
        {
            if (string.IsNullOrEmpty(address)) continue;

            Debug.Log($"[Loader] Загружаю: {address}");
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            handles.Add(handle);
            handle.Completed += OnAssetLoaded;
        }
    }

    private void OnAssetLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"[Loader] Успешно: {handle.Result.name}");

            if (container != null)
                Instantiate(handle.Result, container);
            else
                Instantiate(handle.Result);
        }
        else
        {
            Debug.LogError($"[Loader] Ошибка: {handle.OperationException}");
        }
    }

    // Выгрузить всё
    public void UnloadAll()
    {
        foreach (var handle in handles)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
        handles.Clear();
        Debug.Log("[Loader] Всё выгружено.");
    }
}