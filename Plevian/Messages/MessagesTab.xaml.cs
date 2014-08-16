using Plevian.Debugging;
using Plevian.Players;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Plevian.Messages
{
    public partial class MessagesTab : UserControl
    {
        public MessagesTab(Player recipient)
        {
            InitializeComponent();
            messages.ItemsSource = recipient.messages;
            messages.IsReadOnly = true;
        }

        private void messages_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGridRow selected = DataGridExtensions.GetSelectedRow(messages);
            DataGridCell topicCell = DataGridExtensions.GetCell(messages, selected, 1);
            Message message = selected.DataContext as Message;
            ToolTip tooltip = new ToolTip { Content = message.message };
            topicCell.ToolTip = tooltip;
            tooltip.IsOpen = true;
        }
    }

    public static class DataGridExtensions
    {
        /// <summary>
        /// Gets the selected row of the DataGrid
        /// </summary>
        /// <param name="grid">The DataGrid instance</param>
        /// <returns></returns>
        public static DataGridRow GetSelectedRow(this DataGrid grid)
        {
            return (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        }

        /// <summary>
        /// Gets the visual child of an element
        /// </summary>
        /// <typeparam name="T">Expected type</typeparam>
        /// <param name="parent">The parent of the expected element</param>
        /// <returns>A visual child</returns>
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        /// <summary>
        /// Gets the specified cell of the DataGrid
        /// </summary>
        /// <param name="grid">The DataGrid instance</param>
        /// <param name="row">The row of the cell</param>
        /// <param name="column">The column index of the cell</param>
        /// <returns>A cell of the DataGrid</returns>
        public static DataGridCell GetCell(DataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);

                return cell;
            }
            return null;
        }
    }
}
