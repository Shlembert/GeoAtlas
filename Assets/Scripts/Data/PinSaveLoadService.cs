using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PinSaveLoadService : MonoBehaviour
{
    private static readonly string SavePath = $"{Application.dataPath}/SaveData/pins.json";

    [System.Serializable]
    private class PinDataListWrapper
    {
        public List<PinData> pins;
    }

    public void SavePinsToJson(List<PinData> pins)
    {
        PinDataListWrapper wrapper = new PinDataListWrapper { pins = pins };

        foreach (var pin in pins) Debug.Log($"��� ������������ ����:{pin.pinName} ");

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("���������� ����� ���������!");
    }

    public List<PinData> LoadPinsFromJson()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("���� ���������� �� ������.");
            return new List<PinData>();
        }

        string json = File.ReadAllText(SavePath);
        PinDataListWrapper wrapper = JsonUtility.FromJson<PinDataListWrapper>(json);
        Debug.Log("�������� ����� ���������!");
        return wrapper.pins;
    }
}
