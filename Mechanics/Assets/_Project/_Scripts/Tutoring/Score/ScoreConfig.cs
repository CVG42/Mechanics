using UnityEngine;

[CreateAssetMenu(fileName = "ScoreConfig", menuName = "Scriptable Objects/Score/ScoreConfig")]
public class ScoreConfig : ScriptableObject
{
    public ScoreRange[] Ranges;
}
