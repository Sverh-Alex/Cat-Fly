using System.Collections.Generic;
using UnityEngine;

public class PlayerChanger : MonoBehaviour
{
    public List<GameObject> skins;
    public Vector3 position;
    public int skinsId;
    void Start()
    {
        skinsId = PlayerPrefs.GetInt("skin");
        Instantiate(skins[skinsId], position, Quaternion.identity);
    }

    // Update is called once per frame


}
