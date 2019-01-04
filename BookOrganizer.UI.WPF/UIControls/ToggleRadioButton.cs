using System.Windows.Controls;

namespace BookOrganizer.UI.WPF
{
    public class ToggleRadioButton : RadioButton
    {
        protected override void OnToggle()
        {
            if (IsChecked == true)
                IsChecked = IsThreeState
                    ? (bool?)null
                    : (bool?)false;

            else
                IsChecked = IsChecked.HasValue;
        }
    }
}
