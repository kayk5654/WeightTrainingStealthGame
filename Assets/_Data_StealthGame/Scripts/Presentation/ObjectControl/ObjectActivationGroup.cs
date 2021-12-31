using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivationGroup : MonoBehaviour {

    [SerializeField]
    private List<GameObject> _objects = new List<GameObject>();

    [SerializeField, Tooltip("whether all referred objects are disabled when this object is set active")]
    private bool _disableObjectsOnEnable = true;

    /// <summary>
    /// reset selection when this gameobject is enableds
    /// </summary>
    private void OnEnable()
    {
        if (!_disableObjectsOnEnable) { return; }
        SetActive(false);
    }

    public void SetActive(bool state)
    {
        _objects.ForEach(obj => obj.SetActive(state));
    }

    public void SetSelectedObjActive(int id)
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            if (i == id)
            {
                _objects[i].SetActive(true);
                continue;
            }
            _objects[i].SetActive(false);
        }
    }
}
