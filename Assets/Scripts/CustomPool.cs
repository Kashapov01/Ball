using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CustomPool<T> where T : MonoBehaviour
{
    private IFactory<T> _factory;
    private List<T> _objects;

    public CustomPool(IFactory<T> factory, int countObjects)
    {
        _factory = factory;
        _objects = new List<T>();

        for (int i = 0; i < countObjects; i++)
        {
            var obj = _factory.Create();
            obj.gameObject.SetActive(false);
            _objects.Add(obj);
        }
    }

    public T Get()
    {
        var obj = _objects.FirstOrDefault(o => !o.isActiveAndEnabled);

        if (obj == null)
        {
            obj = _factory.Create();
            _objects.Add(obj);
        }
        
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false); 
    }
}
