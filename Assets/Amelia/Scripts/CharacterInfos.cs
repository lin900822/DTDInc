using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterSelection
{
    
    [CreateAssetMenu(fileName ="Character Info", menuName ="ScriptableObjects/Character Selection")]
    public class CharacterInfos : ScriptableObject
    {
        public int ChracterID;
        //public CharacterType Type; 這個地方的類別??
        public string CharacterName;

        [TextArea(1, 5)]
        public string Description;

    }
}

