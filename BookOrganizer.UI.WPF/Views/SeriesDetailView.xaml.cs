using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.Views
{
    public partial class SeriesDetailView : UserControl
    {
        public SeriesDetailView()
        {
            InitializeComponent();

            //Style itemContainerStyle = new Style(typeof(ListBoxItem));
            //itemContainerStyle.Setters.Add(new Setter(AllowDropProperty, true));
            //itemContainerStyle.Setters.Add(new EventSetter(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(s_PreviewMouseLeftButtonDown)));
            //itemContainerStyle.Setters.Add(new EventSetter(DropEvent, new DragEventHandler(listbox1_Drop)));
            //booksInSeriesListBox.ItemContainerStyle = itemContainerStyle;
        }

        //void s_PreviewMouseMoveEvent(object sender, MouseEventArgs e)
        //{
        //    if (sender is ListBoxItem && e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        ListBoxItem draggedItem = sender as ListBoxItem;
        //        DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
        //        draggedItem.IsSelected = true;
        //    }
        //}
    }
}
