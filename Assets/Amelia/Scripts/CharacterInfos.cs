using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterSelection
{
    
    [CreateAssetMenu(fileName ="Character Info", menuName ="ScriptableObjects/Character Selection")]
    public class CharacterInfos : ScriptableObject
    {
        public int ChracterID;
        //public CharacterType Type; �o�Ӧa�誺���O??
        public string CharacterName;

        [TextArea(1, 5)]
        public string Description;

    }
}

