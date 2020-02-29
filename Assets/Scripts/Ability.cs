using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BraidTimeWalkClone.Ability
{
    public abstract class Ability : MonoBehaviour
    {
        public abstract IEnumerator Cast();
    }
}


