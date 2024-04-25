using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Core.Save
{
    [System.Serializable]
    public abstract class ASavableModel : ScriptableObject
    {
        public abstract ISaveable Save();
        public abstract void Load(object data);
        public abstract void SetDefaultValue();
    }
    public interface ISaveable
    {
        public void CopyFrom(ISaveable obj);
    }
}
