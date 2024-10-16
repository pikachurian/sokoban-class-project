using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Object[] gridObjects;

    public static GridManager reference;

    private void OnEnable()
    {
        reference = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGridObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadGridObjects()
    {
        gridObjects = GameObject.FindObjectsOfType(typeof(GridObject));
    }

    public bool IsSpaceFree(Vector2 space)
    {
        foreach(Object obj in gridObjects)
        {
            if(obj.GetComponent<GridObject>().gridPosition == space && obj.GetComponent<Transform>().CompareTag("Wall"))
                return false;
        }
        return true;
    }

    public string GetSpaceTag(Vector2 space)
    {
        foreach (Object obj in gridObjects)
        {
            if (obj.GetComponent<GridObject>().gridPosition == space)
                return obj.GetComponent<Transform>().tag;
        }
        return null;
    }

    public Object GetSpaceObject(Vector2 space)
    {
        foreach (Object obj in gridObjects)
        {
            if (obj.GetComponent<GridObject>().gridPosition == space)
                return obj;
        }
        return null;
    }
}
