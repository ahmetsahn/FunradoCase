using Runtime.Enums;
using Runtime.Signal;

namespace Runtime.UI
{
    public class RetryLevelButton : LoadLevelButton
    {
        protected override void CloseUIPanel()
        {
            SignalBus.Fire(new CloseUIPanelSignal(UIPanelType.GameOverPanel));
        }
    }
}