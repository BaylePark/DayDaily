using MaterialDesignThemes.Wpf.Transitions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DayDaily.Controls
{
    public class AnimatedContentControl : TransitioningContentBase
    {
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (!IsLoaded) return;
            RunOpeningEffects();
        }
    }
}
