using System;
using UnityEngine;
using UnityEngine.UI;

public class UIClick : MonoBehaviour {

    public Button buildTower;
    public Button castAbility;
    public GameObject player;
    public GameObject[] towerSlots;

    private enum SelectionMode
    {
        None, TowerSlot, Target
    };
    private SelectionMode selection;

    void Start()
    {
        buildTower.onClick.AddListener(BuildTower);
        castAbility.onClick.AddListener(CastAbility);
        selection = SelectionMode.None;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (selection != SelectionMode.None)
            {
                RaycastHit hit;
                // Find the clicked object
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.transform != null)
                    {
                        var obj = hit.transform.gameObject;
                        switch (selection)
                        {
                            // Build a defensive tower
                            case SelectionMode.TowerSlot:
                                var slot = Array.FindAll(towerSlots, s => s.name == obj.name);
                                if (slot.Length > 0)
                                {
                                    Debug.Log("Build tower at " + slot[0].name);
                                }
                                break;
                            // Cast through the enemy
                            case SelectionMode.Target:
                                if(player.tag == "Human" && obj.tag == "Alien")
                                {
                                    Debug.Log("Attack alien enemy");
                                }
                                else if(obj.tag == "Alien" && obj.tag == "Human")
                                {
                                    Debug.Log("Attack human enemy");
                                }
                                break;
                        }
                    }
                }
                selection = SelectionMode.None;
            }
        }
    }

    void BuildTower()
    {
        player.GetComponent<MoveToClickPoint>().InhibitMovement();
        selection = SelectionMode.TowerSlot;
    }

    void CastAbility()
    {
        player.GetComponent<MoveToClickPoint>().InhibitMovement();
        selection = SelectionMode.Target;
    }
}
