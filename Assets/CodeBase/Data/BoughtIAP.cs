using System;

namespace CodeBase.Data
{
    [Serializable]
    public class BoughtIAP
    {
        public string IAPid;
        public int Count;

        public BoughtIAP(string iaPid, int count)
        {
            IAPid = iaPid;
            Count = count;
        }
    }
}