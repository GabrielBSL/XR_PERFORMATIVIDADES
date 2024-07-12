using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Main.AI
{
    public class DataLoader : MonoBehaviour
    {
        public static List<DataPoint> LoadData(string path)
        {
            string json = File.ReadAllText(path);
            DataSet dataSet = JsonUtility.FromJson<DataSet>(json);

            return new List<DataPoint>(dataSet.data);
        }
    }
}
