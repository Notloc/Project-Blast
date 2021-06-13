using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Notloc.Utility
{
    public class UpdatableData : ScriptableObject
    {
        /// <summary>
        /// Event that triggers when the scriptable objects data is updated.
        /// Only works in the editor. Will never be called in builds.
        /// </summary>
        public event System.Action<UpdatableData> OnDataUpdated;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            UnityEditor.EditorApplication.update += NotifySubscribers;
        }

        private void NotifySubscribers()
        {
            UnityEditor.EditorApplication.update -= NotifySubscribers;
            OnDataUpdated?.Invoke(this);
        }
#else
    protected virtual void OnValidate() {}
#endif
    }
}