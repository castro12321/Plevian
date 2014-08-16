using Plevian.Debugging;
using Plevian.Players;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Plevian.Messages
{
    public partial class MessagesTab : UserControl
    {
        public MessagesTab(Player recipient)
        {
            InitializeComponent();

            addColumn("Sender");
            addColumn("Topic");
            addColumn("Date");

            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Button));
            factory.SetValue(Button.ContentProperty, "Delete");
            factory.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonDelete_Click));
            addColumn("Button", factory);

            //messages.AutoGenerateColumns = false;
            //messages.IsReadOnly = true;
            messages.ItemsSource = recipient.messages;
        }

        private void addColumn(String name)
        {
            addColumn(name, new FrameworkElementFactory(typeof(DataGridElement)));
        }

        private void addColumn(String name, FrameworkElementFactory factory)
        {
            DataGridTemplateColumn dgtColumn = new DataGridTemplateColumn();
            dgtColumn.Header = name;

            DataTemplate dTemplate = new DataTemplate();
            FrameworkElementFactory element = factory;
            element.SetBinding(DataGridElement.DataContextProperty, new Binding(name));

            dTemplate.VisualTree = element;
            dgtColumn.CellTemplate = dTemplate;

            messages.Columns.Add(dgtColumn);
        }

        private void messages_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGridRow selected = DataGridExtensions.GetSelectedRow(messages);
            DataGridCell topicCell = DataGridExtensions.GetCell(messages, selected, 1);
            Message message = selected.DataContext as Message;

            MessageWindow window = new MessageWindow(message);
            window.Show();
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(window);
            MainWindow.getInstance().IsEnabled = false;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = sender as Button;
            Logger.log("clicked " + clicked + "; on " + clicked.DataContext);
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
