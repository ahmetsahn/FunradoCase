using Runtime.Enums;

namespace Runtime.Signal
{
    public readonly  struct CloseUIPanelSignal
    {
        public readonly UIPanelTypes PanelType;
        
        public CloseUIPanelSignal(UIPanelTypes panelType)
        {
            PanelType = panelType;
        }
    }
}