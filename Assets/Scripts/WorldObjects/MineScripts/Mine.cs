using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : WorldObject, IEnteranceInteraction
{
    [SerializeField] private GameObject flag;

    [Header("Mine information")]
    [SerializeField] private List<ResourceType> mineType;
    private ResourceIncome mineIncome;

    [Header("Mine Enterance Information")]
    [SerializeField] private GameObject mineEnterance;
    private List<PathNode> enteranceCells;

    [Header("Mine Garrison references")]
    private UnitsInformation unitsInformation;

    #region Initialization

    private void Awake (){
        enteranceCells = new List<PathNode>();
        mineIncome = new ResourceIncome(new int[]{
            1000, 2, 2, 1, 1, 1, 1
        });
        
    }

    public void Initialize (Vector2Int gridPosition, float rotation, PlayerTag playerTag, List<ResourceType> mineType, int [] garrisonUnits)
    {
        Initialize(gridPosition, rotation, ObjectType.Mine);
        unitsInformation = new UnitsInformation(garrisonUnits);
        this.mineType = mineType;
    }

    public override void FinalizeObject()
    {
        GameGrid.Instance.PlaceBuildingOnGrid(gridPosition, BuildingType.TwoByTwo, GetRotation(), gameObject);
        GameGrid.Instance.GetGridCellInformation(gridPosition).AddOccupyingObject(mineEnterance);
    }

    public void SetEnteranceInformation (List <PathNode> enteranceList){
        enteranceCells = enteranceList;
    }

    #endregion

    #region Player Management

    public override void ChangeOwningPlayer(PlayerTag ownedByPlayer)
    {
        PlayerManager.Instance.GetPlayer(GetPlayerTag()).RemoveObject(this);
        base.ChangeOwningPlayer(ownedByPlayer);
        if (ownedByPlayer != PlayerTag.None){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = PlayerManager.Instance.GetPlayerColour(GetPlayerTag());
        }else{
            flag.SetActive(false);
        }
        PlayerManager.Instance.GetPlayer(GetPlayerTag()).AddObject(this);
        
    }

    #endregion

    #region Interaction Manager

    public void Interact<T> (T other){
        Army army = other as Army;
        if (army.GetPlayerTag() == GetPlayerTag()){
            // Do something with friendly army
        }else{
            if (IsMineEmpty()){
                ChangeOwningPlayer(army.GetPlayerTag());
            }else{
                Debug.Log("Do battle");
            }
        }
    }

    public void Interact (){ } 

    #endregion

    public ResourceIncome GetIncome (){
        return new ResourceIncome(mineIncome, mineType);
    }

    private bool IsMineEmpty (){
        return unitsInformation.IsArmyEmpty();
    }
}
