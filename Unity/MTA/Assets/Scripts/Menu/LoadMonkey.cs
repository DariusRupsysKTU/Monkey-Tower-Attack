using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMonkey : MonoBehaviour
{
    public GameObject[] monkeyPrefabs;
    public Transform spawnPoint;

    void Start()
    {
        int selectedMonkey = PlayerPrefs.GetInt(nameof(selectedMonkey));
        GameObject prefab = monkeyPrefabs[selectedMonkey];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
