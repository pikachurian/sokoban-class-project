using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    private GridObject gridObject;
    private Movable movable;

    private UnityEngine.Object[] adjacentBlocks = new UnityEngine.Object[] { null, null, null, null};

    private UnityEngine.Object[] lastAdjacentBlocks = new UnityEngine.Object[] { null, null, null, null };

    private void Start()
    {
        gridObject = GetComponent<GridObject>();
        movable = GetComponent<Movable>();
    }

    private void LateUpdate()
    {
        adjacentBlocks = GetAdjacentBlocks(gridObject.gridPosition);

        for (int i = 0; i < adjacentBlocks.Length; i++)
        {
            if (adjacentBlocks[i] == null && lastAdjacentBlocks[i] != null)
            {
                Vector2Int lastPos = new Vector2Int(gridObject.gridPosition.x, gridObject.gridPosition.y);
                switch(i)
                {
                    case 0: lastPos.y -= 1; break;//up
                    case 1: lastPos.x += 1; break;//right
                    case 2: lastPos.y += 1; break;//down
                    case 3: lastPos.x -= 1; break;//left
                }

                Vector2Int moveInput = lastAdjacentBlocks[i].GetComponent<GridObject>().gridPosition - lastPos;
                movable.Move(moveInput);
                break;
            }
        }

        lastAdjacentBlocks = adjacentBlocks;
    }

    private UnityEngine.Object[] GetAdjacentBlocks(Vector2Int mySpace)
    {
        UnityEngine.Object[] objects = new UnityEngine.Object[] { null, null, null, null };
        UnityEngine.Object obj = null;

        //Up
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x, mySpace.y - 1));
        if (obj != null)
        {
            objects[0] = obj;
            //print(obj.name);
        }

        //Right
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x + 1, mySpace.y));
        if (obj != null)
        {
            objects[1] = obj;
            //print(obj.name);
        }

        //Down
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x, mySpace.y + 1));
        if (obj != null)
        {
            objects[2] = obj;
            //print(obj.name);
        }

        //Left
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x - 1, mySpace.y));
        if (obj != null)
        {
            objects[3] = obj;
            //print(obj.name);
        }

        return objects;
    }
}
