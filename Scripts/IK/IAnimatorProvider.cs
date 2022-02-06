using System;
using UnityEngine;

namespace DoubTech.MCC.IK
{
    public interface IAnimatorProvider
    {
        Animator Animator { get; }
        public event Action<Animator> OnAnimatorChanged;
    }
}
