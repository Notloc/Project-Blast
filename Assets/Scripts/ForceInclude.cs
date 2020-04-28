using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object allows us to create a list of scriptable objects.
/// The intended use is to use these to serialize assets such as databases, which have no active memory references in any scenes.
/// By doing this and placing the list in the Resource folder, we force Unity to include our databases in builds.
/// </summary>
[CreateAssetMenu(menuName = "Resources/Force Inclusion List")]
public class ForceInclude : ScriptableObject
{
    [SerializeField] List<ScriptableObject> includes;
}
