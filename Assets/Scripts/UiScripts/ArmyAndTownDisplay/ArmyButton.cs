using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyButton : MonoBehaviour
{
    [SerializeField] GameObject armyHighlight;
    [SerializeField] GameObject movementSlider;
    [SerializeField] private GameObject connectedArmy;

    void Start ()
    {
        try{
            ObjectSelector.Instance.onSelectedObjectChange.AddListener(HighlighLogic);
        }catch (NullReferenceException){
            Debug.Log("Object Selector Instance does not exist.");
        }  
    }

    internal void UpdateConnectedArmy(GameObject _army)
    {
        connectedArmy = _army.transform.GetChild(0).gameObject;
    }

    public void SelectArmy ()
    {
        if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected.transform.parent.gameObject == connectedArmy){
            connectedArmy.GetComponentInParent<Army>().ArmyInteraction();
        }else{
            ObjectSelector.Instance.RemoveSelectedObject();
            ObjectSelector.Instance.AddSelectedObject(connectedArmy);
        }
    }

    private void HighlighLogic ()
    {
        if (connectedArmy != null){
            if (ObjectSelector.Instance.objectSelected && ObjectSelector.Instance.lastObjectSelected.name == (connectedArmy.name)) armyHighlight.SetActive(true);
            else armyHighlight.SetActive(false);
        }else armyHighlight.SetActive(false);
    }
}
