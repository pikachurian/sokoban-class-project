using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        //If this block would change position after being clamped, move
        if (newPosition != gridObject.gridPosition)
        {
            //Check the tag of the object occupying the new position
            string spaceTag = GridManager.reference.GetSpaceTag(newPosition);
            switch (spaceTag)
            {
                case "Clingy":
                case "Wall":
                    newPosition = nullPosition;
                    break;

                case "Sticky":
                case "Smooth":
                    //Move the pushed object in the same direction of the player's input.
                    Object smoothBlock = GridManager.reference.GetSpaceObject(newPosition);
                    if (smoothBlock.GetComponent<Movable>().Move(input) == false)
                        newPosition = nullPosition;
                    break;
            }

            if (newPosition != nullPosition)
            {
                if(transform.tag == "Player")
                    MoveAdjacentStickyBlocks(gridObject.gridPosition, input);
                gridObject.gridPosition = newPosition;
                return true;
            }
        }
        return false;
    }

    public void MoveAdjacentStickyBlocks(Vector2Int mySpace, Vector2Int input)
    {
        List<Object> stickies = new List<Object>();
        Object obj = null;
        
        //Up
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x, mySpace.y - 1));
        if (obj != null && obj.GetComponent<Transform>().tag == "Sticky")
            stickies.Add(obj);
        //Left
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x - 1, mySpace.y));
        if (obj != null && obj.GetComponent<Transform>().tag == "Sticky")
            stickies.Add(obj);
        //Right
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x + 1, mySpace.y));
        if (obj != null && obj.GetComponent<Transform>().tag == "Sticky")
            stickies.Add(obj);
        //Down
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x, mySpace.y + 1));
        if (obj != null && obj.GetComponent<Transform>().tag == "Sticky")
            stickies.Add(obj);

        foreach(Object o in stickies)
        {
            o.GetComponent<Movable>().Move(input);
        }

        print(stickies);
    }
}
