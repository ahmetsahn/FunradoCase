using Runtime.Enums;

namespace Runtime.Signal
{
    public readonly struct OpenUIPanelSignal
    {
        public readonly UIPanelTypes PanelType;
        
        public OpenUIPanelSignal(UIPanelTypes panelType)
        {
            PanelType = panelType;
        }
    }
}