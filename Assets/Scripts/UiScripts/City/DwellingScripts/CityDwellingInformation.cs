using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CityDwellingInformation : MonoBehaviour
{
    private Player player;
    private List<DwellingObject> cityDwellings;
    private List<float> cityDwellingUnitCount;

    public void Initialize (){
        cityDwellings = new List<DwellingObject>();
        cityDwellingUnitCount = new List<float>();
    }

    public void ChangeOwningPlayer (Player player) { this.player = player; }

    public void AddDailyUnits ()
    {
        if (player.GetPlayerTag() != PlayerTag.None){
            if (cityDwellings.Count > 0){
                for (int i = 0; i < cityDwellings.Count; i++){
                    if (cityDwellings[i] != null){
                        cityDwellingUnitCount[i] += cityDwellings[i].unitWeeklyGain / 7;
                    }
                }
            }
        }
    }

    public void AddDwelling (CityFraction fraction, int index)
    {
        cityDwellings.Add(DwellingManager.Instance.GetDwellingObject(fraction, index));
        cityDwellingUnitCount.Add(cityDwellings[cityDwellings.Count - 1].unitWeeklyGain / 2);
    }

    public void AddDwelling (UnitName unitName)
    {
        cityDwellings.Add(DwellingManager.Instance.GetDwellingObject(unitName));
        cityDwellingUnitCount.Add(cityDwellings[cityDwellings.Count - 1].unitWeeklyGain / 2);
    }

    public void BuyUnits (int index, int unitCount)
    {
        player.RemoveResources(cityDwellings[index].unitCost);
        cityDwellingUnitCount[index] -= unitCount;
    } 

    public void BuyUnits (int[] index, int[] unitCount)
    {
        for (int i = 0; i < index.Length; i++){
            BuyUnits(index[i], unitCount[i]);
        }
    } 

    public short CalculateUnitsAvailableToBuy (short index)
    {
        if (cityDwellings[index].unitCost > player.GetAvailableResources()) return 0;
        return Convert.ToInt16(Math.Min(player.GetAvailableResources() / cityDwellings[index].unitCost, Mathf.FloorToInt(cityDwellingUnitCount[index])));
    }

    public List<DwellingObject> GetDwellings () { return cityDwellings; }

    public float GetUnitCount (int index) { return cityDwellingUnitCount[index]; }
}
