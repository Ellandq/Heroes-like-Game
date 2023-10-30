using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityEnterance : ObjectEnterance
{
    [SerializeField] private City city;

    private void Start (){
        SetEnteranceCells();
        GameGrid.Instance.GetGridCellInformation(GameGrid.Instance.GetGridPosFromWorld(transform.position)).AddOccupyingObject(gameObject);
    }

    private void SetEnteranceCells()
    {
        List <PathNode> enteranceList = new List<PathNode>();
        Vector2Int gridPosition = GameGrid.Instance.GetGridPosFromWorld(transform.TransformPoint(Vector3.zero));
        float cityRotation = city.GetRotation();

        if (cityRotation == 0){
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (cityRotation == 90){
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (cityRotation == 180){
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else if (cityRotation == 270){
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
            try{
                enteranceList.Add(GameGrid.Instance.GetPathNodeInformation(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1)));
            }catch (NullReferenceException){
                Debug.Log("Enterance tile does not exist. ");
            }
        }else {
            Debug.Log("Enterance list of object: " + this.transform.parent.gameObject.name + " is empty.");
            return;
        }

        SetEnteranceList(enteranceList);
    }
}
