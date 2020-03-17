﻿namespace BookOrganizer.UI.WPFCore.DialogServiceManager
{
    public interface IDialogWindow
    {
        bool? DialogResult { get; set; }
        object DataContext { get; set; }
        
        bool? ShowDialog();
    }
}
