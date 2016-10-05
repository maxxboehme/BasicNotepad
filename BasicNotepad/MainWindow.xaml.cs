using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace BasicNotepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentFileName = null;
        private string currentFilePath = null;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void New()
        {
            documentText.Text = "";
            currentFileName = "Untitled";
            currentFilePath = null;
            UpdateTitle();
        }

        /// <summary>
        /// Displays the open file dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open()
        {
            // Configure dialog box
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = currentFileName; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                currentFilePath = dlg.FileName;
                currentFileName = new FileInfo(currentFilePath).Name;
                using (TextReader tr = new StreamReader(currentFilePath))
                {
                    documentText.Text = tr.ReadToEnd();
                }
                UpdateTitle();
            }
        }

        /// <summary>
        /// Saves the active file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save()
        {
            if (String.IsNullOrEmpty(currentFilePath))
            {
                // File has never been saved, must prompt for name
                SaveAs();
            }
            else
            {
                // Save document
                using (TextWriter tr = new StreamWriter(currentFilePath))
                {
                    tr.Write(documentText.Text);
                }
            }
        }

        /// <summary>
        /// Displays the save file dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAs()
        {
            // Configure dialog box
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = currentFileName; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Save document
                currentFilePath = dlg.FileName;
                using (TextWriter tr = new StreamWriter(currentFilePath))
                {
                    tr.Write(documentText.Text);
                }
                UpdateTitle();
            }
        }

        /// <summary>
        /// Menu button handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuHandler_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            switch (item.Name)
            {
                case "NewMenu":
                    New();
                    break;

                case "OpenMenu":
                    Open();
                    break;

                case "SaveMenu":
                    Save();
                    break;

                case "SaveAsMenu":
                    SaveAs();
                    break;

                case "LineColStatusMenu":
                    LineColumnStatusText.Visibility = LineColumnStatusText.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                    break;

                case "ExitMenu":
                    Application.Current.MainWindow.Close();
                    break;
            }
        }

        private void documentText_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateStatus();
        }

        private void documentText_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateTitle()
        {
            mainWindow.Title = currentFileName + " - " + "Notepad";
        }
        /// <summary>
        /// Updates the status bar text.
        /// </summary>
        private void UpdateStatus()
        {
            int caret = documentText.CaretIndex;
            int row = documentText.GetLineIndexFromCharacterIndex(caret);
            int col = caret - documentText.GetFirstVisibleLineIndex();
            LineColumnStatusText.Text = String.Format("Ln {0}, Col {1}", row, col);
        }
    }
}
