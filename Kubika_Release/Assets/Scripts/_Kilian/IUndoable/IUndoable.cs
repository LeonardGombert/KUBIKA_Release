using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public interface IUndoable
    {
        void UndoDecrement(int actionType);
        void UndoIncrement(int actionType);
    }
}
