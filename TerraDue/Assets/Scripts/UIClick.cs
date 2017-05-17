using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClick : MonoBehaviour {

    public Button buildTower;
    public Button castAbility;
    public GameObject player;

    void Start()
    {
        buildTower.onClick.AddListener(BuildTower);
        castAbility.onClick.AddListener(CastAbility);
    }

    void BuildTower()
    {
        Debug.Log(player.name);
    }

    void CastAbility()
    {
        Debug.Log(player.name);
    }
}
