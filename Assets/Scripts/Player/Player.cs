using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header ("Events and coroutines")]
    private Coroutine waitForObjectToBeDestroyed;
    public UnityEvent onArmyAdded;
    public UnityEvent onCityAdded;

    [Header("Player basic information")]
    private WorldObject objectToDestroy;
    private WorldObject lastObjectSelectedByPlayer;
    private PlayerState playerState;
    private PlayerTag playerTag;
    private Color playerColor;
    private bool isPlayerAi;
    private bool playerLost;
    private short daysToLoose = 4;

    [Header("Player structures and armies: ")]
    private List<Army> ownedArmies;
    private List<City> ownedCities;
    private List<Mine> ownedMines;

    [Header("Player resources")]
    private ResourceIncome playerResources;

    [Header("Player daily production")]
    private ResourceIncome resourceIncome;

    private void Start (){
        playerLost = false;
        PlayerManager.Instance.OnNewDayPlayerUpdate.AddListener(DailyResourceGain);
        PlayerManager.Instance.OnNewDayPlayerUpdate.AddListener(PlayerDailyUpdate);
        playerResources = PlayerManager.Instance.GetStartingResources();
        PlayerManager.Instance.PlayerManagerReady();
    }

    public void AddObject (WorldObject obj){
        if (obj is Army){
            Army army = obj as Army;
            ownedArmies.Add(army);
            army.ChangeOwningPlayer(playerTag);
        }else if (obj is City){
            City city = obj as City;
            ownedCities.Add(city);
            city.ChangeOwningPlayer(playerTag);
        }else if (obj is Mine){
            Mine mine = obj as Mine;
            ownedMines.Add(mine);
            mine.ChangeOwningPlayer(playerTag);
        }
    }

    public void RemoveObject (WorldObject obj){
        if (GameManager.Instance.gameHandler.activeSelf){
            if (obj is Army){
                Army army = obj as Army;
                int index = ownedArmies.FindIndex(a => a == army);
                if (index >= 0) ownedArmies.RemoveAt(index); 

            }else if (obj is City){
                City city = obj as City;
                int index = ownedCities.FindIndex(c => c == city);
                if (index >= 0) ownedCities.RemoveAt(index); 

            }else if (obj is Mine){
                Mine mine = obj as Mine;
                int index = ownedMines.FindIndex(m => m == mine);
                if (index >= 0) ownedMines.RemoveAt(index); 
            }
            if (waitForObjectToBeDestroyed == null){
                waitForObjectToBeDestroyed = StartCoroutine(WaitForObjectToBeDestroyed(obj));
            }
        }else{
            objectToDestroy = obj;
        }
    }
    
    #region Resource Manipulation

    public void UpdateResourceGain(){
        ResourceIncome income = new ResourceIncome();

        foreach (City city in ownedCities){
            income += city.GetIncome();
        }

        foreach (Mine mine in ownedMines){
            income += mine.GetIncome();
        }
    }

    private void DailyResourceGain()
    {
        UpdateResourceGain();
        playerResources += resourceIncome;
    }

    public void AddResources (ResourceIncome resource){ playerResources += resource; }

    public void RemoveResources (ResourceIncome cost){ playerResources -= cost; }
    
    #endregion

    #region Update

    // A daily player update
    private void PlayerDailyUpdate ()
    {
        CheckPlayerLooseCondition();
    }

    // A player update on a new turn
    public void NewTurnUpdate ()
    {
        if (lastObjectSelectedByPlayer != null){
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(lastObjectSelectedByPlayer);
            ObjectSelector.Instance.AddSelectedObject(lastObjectSelectedByPlayer);
        }else{
            AddFirstObjectToFollow();
        }
    }

    #endregion

    public void GetSelectedObject ()
    {
        lastObjectSelectedByPlayer =  ObjectSelector.Instance.lastObjectSelected;
    }

    private void AddFirstObjectToFollow ()
    {
        if (ownedArmies.Count > 0){
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(ownedArmies[0].transform.GetChild(0).gameObject);
            ObjectSelector.Instance.AddSelectedObject(ownedArmies[0].transform.GetChild(0).gameObject);
        }else if (ownedCities.Count > 0){
            CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(ownedCities[0]);
            ObjectSelector.Instance.AddSelectedObject(ownedCities[0]);
        }
    }


    // Checks if any loose conditions are met
    private void CheckPlayerLooseCondition ()
    {
        if (GetPlayerTag() !=  PlayerTag.None){
            if (ownedCities.Count == 0){
                daysToLoose--;
                if (ownedArmies.Count == 0){
                    playerLost = true;
                }else{
                    if (daysToLoose == 0){
                        PlayerLost();
                    }
                }
                if (!playerLost){
                    Debug.Log(this.gameObject.name + " has " + daysToLoose + " turns to regain a city.");
                }
            }
        }
    }

    private void PlayerLost ()
    {
        Debug.Log(gameObject.name + " has lost. ");
        
        foreach (Army army in ownedArmies){
            Destroy(army.gameObject);
        }
        foreach (City city in ownedCities){
            city.ChangeOwningPlayer(PlayerTag.None);
        }
        foreach (Mine mine in ownedMines){
            mine.ChangeOwningPlayer(PlayerTag.None);
        }
        playerLost = true;
    }

    private IEnumerator WaitForObjectToBeDestroyed (WorldObject objectToDestroy){
        while (objectToDestroy != null){
            yield return null;
        }
        ObjectSelector.Instance.RemoveSelectedObject();
        if (PlayerManager.Instance.currentPlayer == this) UIManager.Instance.UpdatePlayerDisplay(this); 

        waitForObjectToBeDestroyed = null;
    }

    private void OnEnable(){
        if (objectToDestroy != null){
            RemoveObject(objectToDestroy);
            objectToDestroy = null;
        }
    }

    private void OnDestroy(){
        PlayerManager.Instance.OnNewDayPlayerUpdate.RemoveListener(DailyResourceGain);
        PlayerManager.Instance.OnNewDayPlayerUpdate.RemoveListener(PlayerDailyUpdate);
    }

    public PlayerTag GetPlayerTag () { return playerTag; }

    public ResourceIncome GetAvailableResources () { return playerResources; }
}