using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;

namespace CompareDirectories
{
    /// <summary>
    /// Main View code-behind class.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Window Settings

        [DllImport("user32.dll")]
        private extern static Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);
        [DllImport("user32.dll")]
        private extern static Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

        private const Int32 GWL_STYLE = -16;
        private const Int32 WS_MAXIMIZEBOX = 0x10000;
        private const Int32 WS_MINIMIZEBOX = 0x20000;

        #endregion

        #region Properties

        private string GetFilter
        {
            get
            {
                return fileFilterDropDown.SelectedValue.ToString();
            }
        }

        public MainViewModel MainViewModel { get; set; }

        private string FilterChosen
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            this.MainViewModel = new MainViewModel();
            this.DataContext = this.MainViewModel;
        }

        #endregion

        #region Event Handlers

        private void browseButton1_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                bool emptyDirectoryPath = String.IsNullOrEmpty(MainViewModel.PathFirstDir);
                bool newDirectoryPath = false;
                if (!emptyDirectoryPath)
                {
                    newDirectoryPath = (!MainViewModel.PathFirstDir.Equals(dialog.SelectedPath));
                }
                if ((emptyDirectoryPath) || (newDirectoryPath))
                {
                    MainViewModel.PathFirstDir = dialog.SelectedPath;
                    MainViewModel.GetFilesAndDirectories(MainViewModel.ViewModelFirstDatagrid);
                }
            }
        }

        private void browseButton2_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                bool emptyDirectoryPath = String.IsNullOrEmpty(MainViewModel.PathSecondDir);
                bool newDirectoryPath = false;
                if (!emptyDirectoryPath)
                {
                    newDirectoryPath = (!MainViewModel.PathSecondDir.Equals(dialog.SelectedPath));
                }
                if ((emptyDirectoryPath) || (newDirectoryPath))
                {
                    MainViewModel.PathSecondDir = dialog.SelectedPath;
                    MainViewModel.GetFilesAndDirectories(MainViewModel.ViewModelSecondDatagrid);
                }
            }
        }

        private void fileFilterDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter itemSelected = (Filter)fileFilterDropDown.SelectedItem;
            FilterChosen = itemSelected.Name;
            if (folder1TextBox.Text.Length != 0)
            {
                firstDataGrid.Items.Refresh();
            }
            if (folder2TextBox.Text.Length != 0)
            {
                //worker2.RunWorkerAsync(folder1TextBox.Text);
                secondDataGrid.Items.Refresh();
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hWnd = new WindowInteropHelper(this).Handle;
            Int32 windowLong = GetWindowLong(hWnd, GWL_STYLE);
            windowLong = windowLong & ~WS_MAXIMIZEBOX;
            SetWindowLong(hWnd, GWL_STYLE, windowLong);
        }


        private void firstDataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0.0f)
            {
                ScrollViewer sv = null;
                Type t = firstDataGrid.GetType();
                try
                {
                    sv = t.InvokeMember("InternalScrollHost", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, secondDataGrid, null) as ScrollViewer;
                    sv.ScrollToHorizontalOffset(e.HorizontalOffset);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }

            if (e.VerticalChange != 0.0f)
            {
                ScrollViewer sv = null;
                Type t = firstDataGrid.GetType();
                try
                {
                    sv = t.InvokeMember("InternalScrollHost", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, secondDataGrid, null) as ScrollViewer;
                    sv.ScrollToVerticalOffset(e.VerticalOffset);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }

            firstDataGrid.Items.Refresh();
        }

        private void secondDataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0.0f)
            {
                ScrollViewer sv = null;
                Type t = secondDataGrid.GetType();
                try
                {
                    sv = t.InvokeMember("InternalScrollHost", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, firstDataGrid, null) as ScrollViewer;
                    sv.ScrollToHorizontalOffset(e.HorizontalOffset);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }

            if (e.VerticalChange != 0.0f)
            {
                ScrollViewer sv = null;
                Type t = secondDataGrid.GetType();
                try
                {
                    sv = t.InvokeMember("InternalScrollHost", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, firstDataGrid, null) as ScrollViewer;
                    sv.ScrollToVerticalOffset(e.VerticalOffset);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            secondDataGrid.Items.Refresh();
        }

        private void secondDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            if (folder1TextBox.Text.Length != 0)
            {
                DataItem rowDataContext = e.Row.DataContext as DataItem;

                if (rowDataContext != null)
                {

                    string rowItemName = rowDataContext.ItemName;

                    //if (listDiff != null)
                    //{
                    //    foreach (var element in listDiff)
                    //    {
                    //        if (rowItemName.Equals(element.ItemName))
                    //        {
                    //            e.Row.Foreground = new SolidColorBrush(Colors.White);
                    //            e.Row.Background = new SolidColorBrush(Colors.DarkRed);

                    //        }

                    //    }
                    //}

                }
            }
        }



        private void firstDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            if (folder2TextBox.Text.Length != 0)
            {
                DataGridRow item = e.Row;
                var c = item.DataContext as DataItem;

                if (c != null)
                {
                    string row = c.ItemName;


                    //if (listDiff != null)
                    //{
                    //    foreach (var element in listDiff)
                    //    {
                    //        if (row == element.ItemName)
                    //        {
                    //            e.Row.Foreground = new SolidColorBrush(Colors.White);
                    //            e.Row.Background = new SolidColorBrush(Colors.DarkRed);
                    //        }
                    //    }
                    //}
                }
            }
        }

        #endregion
    }
}
