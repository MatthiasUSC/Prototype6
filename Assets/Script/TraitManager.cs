using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    [System.Serializable]
    public struct Trait {
        public string id;
        public int weight;
        public string displayName;
    }

    List<Trait> masterTraitList = new List<Trait>{
        new Trait{id="highhp", weight=1, displayName="High HP"},
        new Trait{id="bigbullets", weight=1, displayName="Big Bullets"},
        new Trait{id="lowhp", weight=-1, displayName="Low HP"},
        new Trait{id="bouncebullets", weight=0, displayName="Bouncing bullets but friendly fire"},
        new Trait{id="noaim", weight=-1, displayName="No aiming"},
        new Trait{id="invertedcontrols", weight=-3, displayName="Inverted controls"}
    };

    public List<GameObject> players;

    public int pointLimit;
    public int numberOfTraits;

    void Start()
    {
        for(int i = 0; i < players.Count; i++){
            shuffleMasterList();

            List<Trait> tempTraitList = new List<Trait>(masterTraitList);
            List<Trait> playerTraitList = new List<Trait>();

            int points = 0;
            for(int j = 0; j < numberOfTraits; j++){
                if(tempTraitList.Count == 0){
                    break;
                }

                Trait pickedTrait;
                if(points >= pointLimit){
                    pickedTrait = getNegativeTrait(tempTraitList);
                } else {
                    pickedTrait = getPositiveTrait(tempTraitList);
                }

                points += pickedTrait.weight;
                tempTraitList.Remove(pickedTrait);
                playerTraitList.Add(pickedTrait);
            }

            players[i].GetComponent<TraitList>().traitList = playerTraitList;
        }
    }

    Trait getNegativeTrait(List<Trait> list){
        for(int i = 0; i < list.Count; i++){
            if(list[i].weight <= 0){
                return list[i];
            }
        }
        return list[0];
    }

    Trait getPositiveTrait(List<Trait> list){
        for(int i = 0; i < list.Count; i++){
            if(list[i].weight > 0){
                return list[i];
            }
        }
        return list[0];
    }

    void shuffleMasterList(){
         List<Trait> tempList1 = new List<Trait>(masterTraitList);
         List<Trait> tempList2 = new List<Trait>();
         while(tempList1.Count > 0){
            int randomIndex = Random.Range(0, tempList1.Count);
            tempList2.Add(tempList1[randomIndex]);
            tempList1.RemoveAt(randomIndex);
         }
        masterTraitList = tempList2;
    }
}
