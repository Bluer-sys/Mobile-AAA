using System;
using CodeBase.UI;
using CodeBase.UI.Services.Windows;

namespace CodeBase.StaticData.Windows
{
    [Serializable]
    public class WindowConfig
    {
        public WindowId WindowId;
        public WindowBase Prefab;
    }
}