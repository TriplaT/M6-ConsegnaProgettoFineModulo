using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [Header("Prefab e Pool Size")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;

    private List<GameObject> pool;

    private void Awake()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        GameObject newObj = Instantiate(prefab, transform);
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
