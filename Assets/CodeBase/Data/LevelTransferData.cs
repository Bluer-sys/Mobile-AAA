using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LevelTransferData
    {
        public string Id;
        public string TransferTo;
        public bool IsActive;
        public Vector3Data Position;
        public Vector3Data Size;
        public Vector3Data Center;

        public LevelTransferData(string id, string transferTo, bool isActive, Vector3Data position, Vector3Data size, Vector3Data center)
        {
            Id = id;
            TransferTo = transferTo;
            IsActive = isActive;
            Center = center;
            Size = size;
            Position = position;
        }
    }
}