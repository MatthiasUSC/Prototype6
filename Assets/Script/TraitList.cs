using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitList : MonoBehaviour
{
    public List<TraitManager.Trait> traitList = new List<TraitManager.Trait>();

    public bool hasTrait(string traitId){
        for(int i = 0; i < traitList.Count; i++){
            if(traitList[i].id.Equals(traitId)){
                return true;
            }
        }
        return false;
    }
}
