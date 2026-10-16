using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : Component
{
    private T prefab;
    private Transform parent;
    private Stack<T> inactive;

    public Pool(T prefab, Transform parent, int initialCount)
    {
        this.prefab = prefab;
        this.parent = parent;
        inactive = new Stack<T>(initialCount);
        for(int i = 0; i < initialCount; i++)
        {
            CreateObj();
        }
    }

    public T Get()
    {
        if (inactive.Count <= 0) CreateObj();

        T obj = inactive.Pop();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        inactive.Push(obj);
    }

    private void CreateObj()
    {
        T obj = UnityEngine.Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        inactive.Push(obj);
    }

}
