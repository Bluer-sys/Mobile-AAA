using System;

namespace CodeBase.Data
{
    [Serializable]
    public class SaveTriggerData
    {
        public string Id;
        public Vector3Data Position;
        public Vector3Data Size;
        public Vector3Data Center;

        public SaveTriggerData(string id, Vector3Data position, Vector3Data size, Vector3Data center)
        {
            Id = id;
            Position = position;
            Size = size;
            Center = center;
        }
    }
}