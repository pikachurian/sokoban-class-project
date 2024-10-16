using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Movable : MonoBehaviour
{
    private GridObject gridObject;
    private Vector2Int nullPosition = new Vector2Int(-1, -1);

    private void Start()
    {
        gridObject = GetComponent<GridObject>();
    }

    //Returns true if this object moved
    public bool Move(Vector2Int input)
    {
        //Get the new position
        Vector2Int newPosition = Vector2Int.zero;
        newPosition.x = (int)Mathf.Clamp((float)gridObject.gridPosition.x + input.x, 1f, (float)GridMaker.reference.dimensions.x);
        newPosition.y = (int)Mathf.Clamp((float)gridObject.gridPosition.y + input.y, 1f, (float)GridMaker.reference.dimensions.y);

        //Check the tag of the object occupying the new position
        string spaceTag = GridManager.reference.GetSpaceTag(newPosition);
        switch (spaceTag)
        {
            case "Wall":
                newPosition = nullPosition;
                break;

            case "Smooth":
                //Move the pushed object in the same direction of the player's input.
                Object smoothBlock = GridManager.reference.GetSpaceObject(newPosition);
                if (smoothBlock.GetComponent<Movable>().Move(input) == false)
                    newPosition = nullPosition;
                break;
        }

        if (newPosition != nullPosition)
        {
            gridObject.gridPosition = newPosition;
            return true;
        }
        return false;
    }
}
