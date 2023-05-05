using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Character Info Data", menuName = "ScriptableObjects/Character Info Data", order = 1)]
    public class CharacterInfoSO : ScriptableObject
    {
        [SerializeField] private List<CharacterInfo> characterInfos = null;

        public Sprite getIconById(int id)
        {
            Sprite icon = null;
            foreach (var c in characterInfos)
            {
                if (c.Id == id)
                {
                    icon = c.Icon;
                    break;
                }
            }
            return icon;
        }
        
        [Serializable]
        private class CharacterInfo
        {
            public int Id;
            public String Name;
            public Sprite Icon;
        }
    }
}