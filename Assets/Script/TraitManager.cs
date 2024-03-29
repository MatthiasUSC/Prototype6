using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TraitManager : MonoBehaviour
{
    [System.Serializable]
    public struct Trait {
        public string id;
        public int weight;
        public string displayName;
    }


    List<Trait> masterTraitList = new List<Trait>{
        new Trait{id="highhp", weight=2, displayName="High HP"},
        new Trait{id="bigbullets", weight=1, displayName="Big Bullets"},
        new Trait{id="lowhp", weight=-2, displayName="Low HP"},
        new Trait{id="multibullet", weight=1, displayName="Multiple Bullets"},
        new Trait{id="curveleft", weight=-1, displayName="Hook"},
        new Trait{id="curveright", weight=-1, displayName="Slice"},
        new Trait{id="xraybullet", weight=2, displayName="X-ray Bullets"},
        new Trait{id="bouncebullets", weight=0, displayName="Bouncing bullets but friendly fire"},
        new Trait{id="noaim", weight=-4, displayName="No aiming"},
        new Trait{id="invertedcontrols", weight=-4, displayName="Inverted controls"},
        new Trait{id="smallsize", weight=1, displayName="Small Size"},
        new Trait{id="bigsize", weight=-1, displayName="Big Size"},
        new Trait{id="fastspeed", weight=1, displayName="Fast Speed"},
        new Trait{id="slowspeed", weight=-1, displayName="Slow Speed"},
        new Trait{id="slowbullets", weight=-1, displayName="Slow Bullets"},
        new Trait{id="jammedgun", weight=-1, displayName="Jammed Gun"},
        new Trait{id="shakyaim", weight=-1, displayName="Shaky Aim"},
        new Trait{id="homingbullet", weight=1, displayName="Homing Bullets"},
        new Trait{id="teleporthit", weight=1, displayName="Teleportation"}
    };

    public List<GameObject> players;

    static public int pointLimit;
    static public int numberOfTraits;

    public TextMeshProUGUI pointLimitUI;
    public TextMeshProUGUI numberOfTraitsUI;

    public List<TextMeshProUGUI> traitsUI;
    void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            pointLimit -= 1;
        }
        if(Input.GetKeyDown(KeyCode.W)){
            pointLimit += 1;
        }
        if(Input.GetKeyDown(KeyCode.A)){
            numberOfTraits -= 1;
        }   
        if(Input.GetKeyDown(KeyCode.S)){
            numberOfTraits += 1;
        }
        if(Input.GetKeyDown(KeyCode.E)){
            pointLimitUI.enabled = !pointLimitUI.enabled;
            numberOfTraitsUI.enabled = !numberOfTraitsUI.enabled;
            for(int i = 0; i < traitsUI.Count; i++){
                traitsUI[i].enabled = !traitsUI[i].enabled;
            }
        }

        pointLimitUI.text = "Point Baseline: " + pointLimit.ToString();
        numberOfTraitsUI.text = "Num of Traits: " + numberOfTraits.ToString();

    }
    void Start()
    {
        if(numberOfTraits > 0){
            for(int i = 0; i < players.Count; i++){
                shuffleMasterList();

                List<Trait> tempTraitList = new List<Trait>(masterTraitList);
                List<Trait> playerTraitList = new List<Trait>();

                int points = 0;
                string text = "Traits: \n";
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
                    text += pickedTrait.displayName;
                    text += ", ";
                    text += pickedTrait.weight.ToString();
                    text += "\n";
                }

                players[i].GetComponent<TraitList>().traitList = playerTraitList;

                traitsUI[i].text = text;
            }
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
