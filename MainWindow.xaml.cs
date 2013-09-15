using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace CompareDirectories
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /* settings for the window */

        [DllImport("user32.dll")]
        private extern static Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);
        [DllImport("user32.dll")]
        private extern static Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

        private const Int32 GWL_STYLE = -16;
        private const Int32 WS_MAXIMIZEBOX = 0x10000;
        private const Int32 WS_MINIMIZEBOX = 0x20000;

        /* end settings */

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

        public IOUtilities IOUtilities { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel = new CompareDirectories.MainViewModel();
            comboBoxFilters();
            this.DataContext = MainViewModel;
            IOUtilities = new IOUtilities(MainViewModel);
            FilterChosen = "*.*";
        }

        //void workerRecursive_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
        //    {
        //        updateDatagrids();
        //        firstDataGrid.Items.Refresh();
        //        secondDataGrid.Items.Refresh();
        //        System.Threading.Thread.Sleep(100);
        //    }));


        //}

        //void workerRecursive_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    worker1.RunWorkerAsync(path1);

        //    worker2.RunWorkerAsync(path2);
        //    System.Threading.Thread.Sleep(250);
        //}

        //void worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    this.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate()
        //        {
        //            updateSecondDatagrid();
        //            updateFileDirCounters2();
        //        }));
        //    System.Threading.Thread.Sleep(100);

        //}

        //void worker2_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    scanForFiles2(path2);
        //}

        //void worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{

        //    this.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate()
        //    {
        //        updateFirstDatagrid();
        //        updateFileDirCounters1();
        //    }));
        //    System.Threading.Thread.Sleep(100);
        //}

        //void worker1_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    scanForFiles1(path1);
        //}

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
                    //firstDataGrid.Items.Refresh();
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
                    newDirectoryPath = (!MainViewModel.PathFirstDir.Equals(dialog.SelectedPath));
                }
                if ((emptyDirectoryPath) || (newDirectoryPath))
                {
                    MainViewModel.PathSecondDir = dialog.SelectedPath;
                    MainViewModel.GetFilesAndDirectories(MainViewModel.ViewModelSecondDatagrid);
                    //secondDataGrid.Items.Refresh();
                }
            }
        }
                   

        /* method to set the filters available in the filter dropdown */

        private void comboBoxFilters()
        {
            ObservableCollection<Filter> filtersList = new ObservableCollection<Filter>();
            string[] array = new string[] { "*.*", "*.pdf", "*.txt", "*.config", "*.dll", "*.zip", "*.rar", "*.exe" };
            foreach (string filter in array)
            {
                filtersList.Add(new Filter()
                {
                    Name = filter
                });
            }
            fileFilterDropDown.DataContext = filtersList;

        }

        public class Filter
        {
            public string Name
            {
                get;
                set;
            }
        }

        private void fileFilterDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter itemSelected = (Filter)fileFilterDropDown.SelectedItem;
            FilterChosen = itemSelected.Name;
            if (folder1TextBox.Text.Length != 0)
            {

                //worker1.RunWorkerAsync(folder1TextBox.Text);
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

        //private void checkIfFoldersAreEqual()
        //{
        //    if (FilesFound1 == FilesFound2 && SubDirectoriesFound1 == SubDirectoriesFound2)
        //    {

        //        bool checkLists = listDiff.Count == 0;
        //        //bool checkLists = filesList1.SequenceEqual(filesList2);
        //        if (checkLists)
        //        {
        //            resultOfComparingTextBlock.Text = "Equal Folders!";
        //            resultOfComparingTextBlock.Foreground = new SolidColorBrush(Colors.DarkGreen);
        //            resultOfComparingTextBlock.Visibility = System.Windows.Visibility.Visible;
        //        }
        //        else
        //        {
        //            resultOfComparingTextBlock.Text = "Different Folders!";
        //            resultOfComparingTextBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
        //            resultOfComparingTextBlock.Visibility = System.Windows.Visibility.Visible;

        //        }

        //    }
        //    else
        //    {
        //        resultOfComparingTextBlock.Text = "Different Folders!";
        //        resultOfComparingTextBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
        //        resultOfComparingTextBlock.Visibility = System.Windows.Visibility.Visible;
        //    }
        //}

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


        //private void showDiffTwoCollections()
        //{
        //    if(listDiff != null) 
        //        listDiff.Clear();
        //   listDiff = filesList1.Where(x => !filesList2.Any(x1 => x1.ItemName == x.ItemName))
        //        .Union(filesList2.Where(x => !filesList1.Any(x1 => x1.ItemName == x.ItemName))).ToList<CompareDirectories.DataItem>();


        //}

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
    }
}
