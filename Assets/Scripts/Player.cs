using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GridObject gridObject;

    private void Update()
    {
        Vector2 input = GetInput();

        //Get the new position
        Vector2Int newPosition = Vector2Int.zero;
        newPosition.x = (int)Mathf.Clamp((float)gridObject.gridPosition.x + input.x, 1f, (float)GridMaker.reference.dimensions.x);
        newPosition.y = (int)Mathf.Clamp((float)gridObject.gridPosition.y + input.y, 1f, (float)GridMaker.reference.dimensions.y);

        //Check if the space is free, if so move there
        if (GridManager.reference.IsSpaceFree(newPosition))
            gridObject.gridPosition = newPosition;
    }

    private Vector2 GetInput()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) //left
            input = Vector2.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) //right
            input = Vector2.right;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //up
            input = Vector2.down;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) //down
            input = Vector2.up;
        return input;
    }
}
