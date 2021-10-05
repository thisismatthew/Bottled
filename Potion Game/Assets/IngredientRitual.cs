using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientRitual : MonoBehaviour
{
    public int RitualSpotsCompleted = 0;
    public List<RitualSpot> RitualSpots;
    private List<RitualSpot> _ritualSpotsUpdate;
    public int _ritualSpotsNeeded;
    public GameObject IngredientPrefab;
    public Animator RitualCircleAnimator;


    private void Start()
    {
        _ritualSpotsUpdate = new List<RitualSpot>();
        _ritualSpotsNeeded = RitualSpots.Count;
        IngredientPrefab.active = false;
    }
    // Update is called once per frame
    void Update()
    {
        
        foreach (RitualSpot spot in RitualSpots)
        {
            if (spot.RitualSpotComplete)
            {
                RitualSpotsCompleted++;
                _ritualSpotsUpdate.Add(spot);
            }
        }
        foreach (RitualSpot spot in _ritualSpotsUpdate)
        {
            RitualSpots.Remove(spot);
        }

        if (RitualSpotsCompleted == _ritualSpotsNeeded)
        {
            RitualCircleAnimator.SetTrigger("All Candle");
            IngredientPrefab.active = true;

            RitualSpotsCompleted = 0;
        }
    }
}
