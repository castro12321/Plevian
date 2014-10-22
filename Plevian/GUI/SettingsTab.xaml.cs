using Plevian.Maps;
using Plevian.Players;
using Plevian.Save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SWF = System.Windows.Forms;

namespace Plevian.GUI
{
    /// <summary>
    /// Interaction logic for SettingsTab.xaml
    /// </summary>
    public partial class SettingsTab : UserControl
    {
        public SettingsTab()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates and shows directory pick dialog to the user
        /// </summary>
        /// <param name="description">Description to show to the user</param>
        /// <param name="initialDirectory">Root directory where to start the dialog</param>
        /// <returns>Directory picked by the user</returns>
        public static String SelectDirectory(String description, String initialDirectory)
        {
            SWF.FolderBrowserDialog folderDialog = new SWF.FolderBrowserDialog();
            folderDialog.Description = description;
            folderDialog.SelectedPath = initialDirectory;

            if (folderDialog.ShowDialog() == SWF.DialogResult.OK)
                return folderDialog.SelectedPath;
            return null;
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            Game game = Game.game;

            String savePath = SelectDirectory("Select save folder", "");

            SaveWriter writeSave = new SaveWriter(savePath);
            writeSave.writeSave(game.map, game.players);
        }

        private void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            String loadPath = SelectDirectory("Select load folder", "");
            SaveReader readSave = new SaveReader(loadPath);

            Entry.players = readSave.getPlayers();
            Entry.map = readSave.getMap(Entry.players);
            MainWindow.getInstance().Close();
        }
    }
}
