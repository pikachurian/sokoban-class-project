using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Movable : MonoBehaviour
{
    [HideInInspector]
    public bool hasMoved = false;

    private GridObject gridObject;
    private Vector2Int nullPosition = new Vector2Int(-1, -1);

    private void Start()
    {
        gridObject = GetComponent<GridObject>();
    }

    private void Update()
    {
        hasMoved = false;
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
                    if (transform.tag != "Clingy")
                    {
                        //Move the pushed object in the same direction of the player's input.
                        Object smoothBlock = GridManager.reference.GetSpaceObject(newPosition);
                        if (smoothBlock.GetComponent<Movable>().Move(input) == false)
                            newPosition = nullPosition;
                    }
                    break;
            }

            if (newPosition != nullPosition && hasMoved == false)
            {
                hasMoved = true;
                Vector2Int previousGridPosition = gridObject.gridPosition;
                gridObject.gridPosition = newPosition;

                if (transform.tag != "Sticky")
                {
                    MoveAdjacentStickyBlocks(previousGridPosition, input);
                }

                MoveAdjacentClingyBlock(previousGridPosition, input);

                return true;
            }
        }
        //print(name + " couldn't move");
        return false;
    }

    public void MoveAdjacentClingyBlock(Vector2Int mySpace, Vector2Int input)
    {
        //Check if there's a clingy block in the opposite direction of the input.
        Object obj = GridManager.reference.GetSpaceObject(mySpace - input);
        //print("Checking for clingy got " + obj.name);
        if (obj != null && obj.GetComponent<Transform>().tag == "Clingy")
        {
            obj.GetComponent<Movable>().Move(input);
        }
    }
    public void MoveAdjacentStickyBlocks(Vector2Int mySpace, Vector2Int input)
    {
        List<Object> stickies = new List<Object>();
        Object obj = null;
        //print(transform.name + " Move Sticky");
        //Up
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x, mySpace.y - 1));
        if (obj != null && obj.GetComponent<Transform>().tag == "Sticky")
        {
            stickies.Add(obj);
            //print(obj.name);
        }
        //Left
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x - 1, mySpace.y));
        if (obj != null && obj.GetComponent<Transform>().tag == "Sticky")
        {
            stickies.Add(obj);
            //print(obj.name);
        }
        //Right
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x + 1, mySpace.y));
        if (obj != null && obj.GetComponent<Transform>().tag == "Sticky")
        {
            stickies.Add(obj);
            //print(obj.name);
        }
        //Down
        obj = GridManager.reference.GetSpaceObject(new Vector2Int(mySpace.x, mySpace.y + 1));
        if (obj != null && obj.GetComponent<Transform>().tag == "Sticky")
        {
            stickies.Add(obj);
            //print(obj.name);
        }

        //print(stickies.Count.ToString() + " stickies");
        foreach (Object o in stickies)
        {
            //if (o.GetComponent<Movable>().hasMoved == false)
            //{
            o.GetComponent<Movable>().Move(input);
             //   hasMoved = true;
            //}
        }

        //print(stickies);
    }
}
