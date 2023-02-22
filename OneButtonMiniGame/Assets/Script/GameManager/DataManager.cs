using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] public float speed = 0.5f;
    [SerializeField] public float second_to_wake_up = 2.0f;
    [SerializeField] public TextAsset wake_up_probability_csv;
    [SerializeField] public TextAsset kyoro_probability_csv;
    public WakeUpProbability[] wake_up_probability;
    public KyoroProbability[] kyoro_probability;


[ContextMenu("CSVをデータ化")]
private void PrivateMethod()
{
    wake_up_probability = CSVSerializer.Deserialize<WakeUpProbability>(wake_up_probability_csv.text);
    kyoro_probability = CSVSerializer.Deserialize<KyoroProbability>(kyoro_probability_csv.text);
}
}

[System.Serializable]
public class WakeUpProbability
{
    public float second;
    public int wake_up_probability;
}

[System.Serializable]
public class KyoroProbability
{
    public int number;
    public int kyoro_probability;
}