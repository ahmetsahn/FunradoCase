using Runtime.Enums;

namespace Runtime.Signal
{
    public readonly struct OpenUIPanelSignal
    {
        public readonly UIPanelType PanelType;
        
        public OpenUIPanelSignal(UIPanelType panelType)
        {
            PanelType = panelType;
        }
    }
}