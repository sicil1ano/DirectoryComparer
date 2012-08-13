using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Windows.Threading;

namespace CompareDirectories
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private extern static Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);
	    [DllImport("user32.dll")]
	    private extern static Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

        private const Int32 GWL_STYLE = -16;
	    private const Int32 WS_MAXIMIZEBOX = 0x10000;
	    private const Int32 WS_MINIMIZEBOX = 0x20000;

        private BackgroundWorker worker1 = new BackgroundWorker();
        private BackgroundWorker worker2 = new BackgroundWorker();
        private BackgroundWorker workerRecursive = new BackgroundWorker();

        private string GetFilter {
            get {
                return fileFilterDropDown.SelectedValue.ToString();
            }
        }
        static List<Info> filesList1;
        static List<Info> filesList2;

        static List<Info> listDiff;

        
        //private string defaultFilter = "*.*";
        private string path1 ="";
        private string path2 = "";
        private bool RecursiveScan1 { get; set; }
        private bool RecursiveScan2 { get; set; }
        private bool RecursiveScanCheck { get; set; }
        //private string filterChosen = "";
        private string FilterChosen
        {
            get;
            set;
        }

        private int FilesFound1
        {
            get;
            set;
        }

        private int FilesFound2
        {
            get;
            set;
        }

        private int SubDirectoriesFound1
        {
            get;
            set;
        }

        private int SubDirectoriesFound2
        {
            get;
            set;
        }

        /* inserire checkbox se si vuole ricerca ricorsiva (con top directory o no*/
        public MainWindow()
        {
            InitializeComponent();
            comboBoxFilters();
            
            FilterChosen = "*.*";
            filesList1 = new List<Info>();
            filesList2 = new List<Info>();
            worker1.DoWork += new DoWorkEventHandler(worker1_DoWork);
            worker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker1_RunWorkerCompleted);
            worker2.DoWork += new DoWorkEventHandler(worker2_DoWork);
            worker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker2_RunWorkerCompleted);
            workerRecursive.DoWork += new DoWorkEventHandler(workerRecursive_DoWork);
            workerRecursive.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerRecursive_RunWorkerCompleted);
        }

        void workerRecursive_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
            {
                updateDatagrids();
                firstDataGrid.Items.Refresh();
                secondDataGrid.Items.Refresh();
                System.Threading.Thread.Sleep(100);
            }));

            //worker2.RunWorkerAsync(path2);
            //updateDatagrids();
        }

        void workerRecursive_DoWork(object sender, DoWorkEventArgs e)
        {
            worker1.RunWorkerAsync(path1);
            //System.Threading.Thread.Sleep(1000);
            worker2.RunWorkerAsync(path2);
            System.Threading.Thread.Sleep(250);
        }

        void worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate()
                {
                    updateSecondDatagrid();
                    updateFileDirCounters2();
                }));
            System.Threading.Thread.Sleep(100);
            //updateSecondDatagrid();
           // updateFileDirCounters2();
            //updateSecondDatagrid();
            //updateFileDirCounters2();
        }

        void worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            scanForFiles2(path2);
        }

        void worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //updateFirstDatagrid();
            //updateFileDirCounters1();
           /* if (path2.Length != 0)
                 worker2.RunWorkerAsync(path2);*/
           

           // updateFirstDatagrid();
            //updateFileDirCounters1();
            //System.Windows.Forms.MessageBox.Show("Completed");
            this.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate()
            {
                updateFirstDatagrid();
                updateFileDirCounters1();
            }));
            System.Threading.Thread.Sleep(100);
        }

        void worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            scanForFiles1(path1);
        }

        private void browseButton1_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path1 = dialog.SelectedPath;
                //initializingWorker1();
                worker1.RunWorkerAsync(path1);
                //scanForFiles1(dialog.SelectedPath);
                folder1TextBox.Text = dialog.SelectedPath;
                //path1 = dialog.SelectedPath;
                
            }
        }

       private void browseButton2_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
               path2 = dialog.SelectedPath;
               //scanForFiles2(dialog.SelectedPath);
               worker2.RunWorkerAsync(path2);
               folder2TextBox.Text = dialog.SelectedPath;
               
               
            }
        }

        private void scanForFiles1(string path) {
            
            //firstDataGrid.ItemsSource = null;
            filesList1.Clear();
            
           // BindingOperations.ClearAllBindings(firstDataGrid);
            
            int filesFound = 0;
            int subDirectoriesFound = 0;
            var dirInfo = new System.IO.DirectoryInfo(path);
          
                if (RecursiveScanCheck)
                {
                   int fileRecFound = 0;
                    int subDirRecFound = 0;
                    foreach (var file in dirInfo.GetFiles(FilterChosen, System.IO.SearchOption.TopDirectoryOnly))
                    {
                        if (file != null)
                        {
                            filesList1.Add(new Info()
                            {
                                SubFolder = false,
                                Name = file.Name,
                                Path = file.FullName,
                                CreatedDate = file.CreationTimeUtc,
                                ModifiedDate = file.LastWriteTimeUtc
                            });
                            filesFound++;
                        }
                        //firstDataGrid.Items.Add(file.Name);
                        //System.Windows.Forms.MessageBox.Show(file.Directory.ToString());
                    }

                    recursiveSearch1(path, out fileRecFound, out subDirRecFound);
                    filesFound = filesFound + fileRecFound;
                    subDirectoriesFound = subDirectoriesFound + subDirRecFound;
                                        
                   // firstDataGrid.ItemsSource = filesList1;
                   // firstDataGrid.DataContext = filesList1;
                }
                else
                { 
                    foreach (var file in dirInfo.GetFiles(FilterChosen, System.IO.SearchOption.TopDirectoryOnly))
                    {
                        filesList1.Add(new Info()
                        {
                            SubFolder = false,
                            Name = file.Name,
                            Path = file.FullName,
                            CreatedDate = file.CreationTimeUtc,
                            ModifiedDate = file.LastWriteTimeUtc
                        });
                        filesFound++;
                      //  firstDataGrid.Items.Add(file.Name);
                    }
                    //firstDataGrid.ItemsSource = filesList1;
                    //firstDataGrid.DataContext = filesList1;
                }
           // }
            FilesFound1 = filesFound;
            SubDirectoriesFound1 = subDirectoriesFound;
            //this.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate()
            //{
            //    updateFirstDatagrid();
            //    updateFileDirCounters1();
            //}));
           // updateFirstDatagrid();
            //updateFileDirCounters1();
            //firstDataGrid.ItemsSource = filesList;
        }

        private void updateFileDirCounters1()
        {
            filesFound1TextBox.Text = FilesFound1.ToString();
            subdirectories1FoundTextBox.Text = SubDirectoriesFound1.ToString();
        }

        /* in ricerca non ricorsiva il refresh non funziona bene, pare che il refresh funzioni prima che si mostrino i dati */ /* ok, abbassando la priorità del dispatcher del refresh pare funzioni */

        private void scanForFiles2(string path)
        {
            
            //secondDataGrid.ItemsSource = null;
            filesList2.Clear();

           // BindingOperations.ClearAllBindings(secondDataGrid);
            
            int filesFound = 0;
            int subDirectoriesFound = 0;
            var dirInfo = new System.IO.DirectoryInfo(path);
           
                if (RecursiveScanCheck)
                {
                    int fileRecFound = 0;
                    int subDirRecFound = 0;
                    foreach (var file in dirInfo.GetFiles(FilterChosen, System.IO.SearchOption.TopDirectoryOnly))
                    {
                        filesList2.Add(new Info()
                        {
                            SubFolder = false,
                            Name = file.Name,
                            Path = file.FullName,
                            CreatedDate = file.CreationTimeUtc,
                            ModifiedDate = file.LastWriteTimeUtc
                        });
                        filesFound++;
                        //firstDataGrid.Items.Add(file.Name);
                        //System.Windows.Forms.MessageBox.Show(file.Directory.ToString());
                    }

                    recursiveSearch2(path, out fileRecFound, out subDirRecFound);
                    filesFound = filesFound + fileRecFound;
                    subDirectoriesFound = subDirectoriesFound + subDirRecFound;

                    
                   // secondDataGrid.ItemsSource = filesList2;
                   // secondDataGrid.DataContext = filesList2;
                }
                else
                {
                    foreach (var file in dirInfo.GetFiles(FilterChosen, System.IO.SearchOption.TopDirectoryOnly))
                    {
                        filesList2.Add(new Info()
                        {
                            SubFolder = false,
                            Name = file.Name,
                            Path = file.FullName,
                            CreatedDate = file.CreationTimeUtc,
                            ModifiedDate = file.LastWriteTimeUtc
                        });
                        filesFound++;
                        //secondDataGrid.Items.Add(file.Name);
                    }
                    //secondDataGrid.ItemsSource = filesList2;
                    //secondDataGrid.DataContext = filesList2;
                }
            
            FilesFound2 = filesFound;
            SubDirectoriesFound2 = subDirectoriesFound;
           
            
            
        }

        private void updateFileDirCounters2()
        {
            filesFound2TextBox.Text = FilesFound2.ToString();
            subdirectoriesFound2TextBox.Text = SubDirectoriesFound2.ToString();
        }

        private void comboBoxFilters() {
           /* fileFilterDropDown.Items.Add("*.*");
            fileFilterDropDown.Items.Add("*.config");
            fileFilterDropDown.Items.Add("*.dll");
            fileFilterDropDown.Items.Add("*.exe");*/
            List<Filter> filtersList = new List<Filter>();
            string[] array = new string[] { "*.*", "*.pdf", "*.txt", "*.config", "*.dll", "*.zip","*.rar","*.exe" };
            foreach(string filter in array){
                filtersList.Add(new Filter()
                {
                    Name = filter  
                });
            }
            fileFilterDropDown.DataContext = filtersList;

        }

        public class Filter {
            public string Name
            {
                get;
                set;
            }
        }

        private void fileFilterDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter itemSelected = (Filter) fileFilterDropDown.SelectedItem;
            FilterChosen = itemSelected.Name;
            if (folder1TextBox.Text.Length != 0)
            {
                //scanForFiles1(folder1TextBox.Text);
                worker1.RunWorkerAsync(folder1TextBox.Text);
                firstDataGrid.Items.Refresh();
            }
            if (folder2TextBox.Text.Length != 0)
            {
                //scanForFiles2(folder2TextBox.Text);
                worker2.RunWorkerAsync(folder1TextBox.Text);
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

        private void recursiveSearch1(string path, out int filesFound, out int subDirFound)
        {
            filesFound = 0;
            subDirFound = 0;
            var dirInfo = new DirectoryInfo(path);

            foreach(var dir in dirInfo.GetDirectories())
            {
                if (dir.FullName == path)
                    continue;
                else
                {
                    foreach (var file in dir.GetFiles(FilterChosen, SearchOption.TopDirectoryOnly))
                    {
                        filesList1.Add(new Info()
                        {
                            SubFolder = true,
                            Name = "."+@"\"+dir.Name + @"\" + file.Name,
                            //Name = file.FullName.Replace(path,""),
                            Path = file.FullName,
                            CreatedDate = file.CreationTimeUtc,
                            ModifiedDate = file.LastWriteTimeUtc
                        });
                        filesFound++;
                    }
                    subDirFound++;

                    foreach (var subDir in dir.GetDirectories())
                    {
                        foreach (var file in subDir.GetFiles(FilterChosen, SearchOption.TopDirectoryOnly))
                        {
                            filesList1.Add(new Info()
                            {
                                SubFolder = true,
                                //Name = dir.Name + @"\" + file.Name,
                                Name = "."+file.FullName.Replace(path, ""),
                                Path = file.FullName,
                                CreatedDate = file.CreationTimeUtc,
                                ModifiedDate = file.LastWriteTimeUtc
                            });
                            filesFound++;
                        }
                        subDirFound++;
                    }
                    //subDirFound++;
                }
            }
            
        }

        private void recursiveSearch2(string path, out int filesFound, out int subDirFound)
        {
            filesFound = 0;
            subDirFound = 0;
            var dirInfo = new DirectoryInfo(path);

            foreach (var dir in dirInfo.GetDirectories())
            {
                if (dir.FullName == path)
                    continue;
                else
                {
                    foreach (var file in dir.GetFiles(FilterChosen, SearchOption.TopDirectoryOnly))
                    {
                        filesList2.Add(new Info()
                        {
                            SubFolder = true,
                            Name = "." + @"\" + dir.Name + @"\" + file.Name,
                            //Name = file.FullName.Replace(path,""),
                            Path = file.FullName,
                            CreatedDate = file.CreationTimeUtc,
                            ModifiedDate = file.LastWriteTimeUtc
                        });
                        filesFound++;
                    }
                    subDirFound++;

                    foreach (var subDir in dir.GetDirectories())
                    {
                        foreach (var file in subDir.GetFiles(FilterChosen, SearchOption.TopDirectoryOnly))
                        {
                            filesList2.Add(new Info()
                            {
                                SubFolder = true,
                                //Name = dir.Name + @"\" + file.Name,
                                Name = "." + file.FullName.Replace(path, ""),
                                Path = file.FullName,
                                CreatedDate = file.CreationTimeUtc,
                                ModifiedDate = file.LastWriteTimeUtc
                            });
                            filesFound++;
                        }
                        subDirFound++;
                    }
                    //subDirFound++;
                }
                
            }
            
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

        private void checkIfFoldersAreEqual()
        {
            if (FilesFound1 == FilesFound2 && SubDirectoriesFound1 == SubDirectoriesFound2)
            {
                //bool checkLists = filesList1.SequenceEqual(filesList2);
                bool checkLists = listDiff.Count == 0;
                if (checkLists)
                {
                    resultOfComparingTextBlock.Text = "Equal Folders!";
                    resultOfComparingTextBlock.Foreground = new SolidColorBrush(Colors.DarkGreen);
                    resultOfComparingTextBlock.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    resultOfComparingTextBlock.Text = "Different Folders!";
                    resultOfComparingTextBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
                    resultOfComparingTextBlock.Visibility = System.Windows.Visibility.Visible;

                }

            }
            else
            {
                resultOfComparingTextBlock.Text = "Different Folders!";
                resultOfComparingTextBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
                resultOfComparingTextBlock.Visibility = System.Windows.Visibility.Visible;
            }
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

        
        private void folder1TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (folder2TextBox.Text.Length != 0)
            {
                //checkIfFoldersAreEqual();
                showDiffTwoCollections();
                checkIfFoldersAreEqual();
            }
           
        }

        private void folder2TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (folder1TextBox.Text.Length != 0)
            {
                //checkIfFoldersAreEqual();
                showDiffTwoCollections();
                checkIfFoldersAreEqual();
            }
            
        }

        private void showDiffTwoCollections()
        {
           listDiff = filesList1.Where(x => !filesList2.Any(x1 => x1.Name == x.Name))
                .Union(filesList2.Where(x => !filesList1.Any(x1 => x1.Name == x.Name))).ToList<CompareDirectories.Info>();

          
        }

        private void secondDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            
            if (folder1TextBox.Text.Length != 0)
            {
                //showDiffTwoCollections();
                //Info itemSelected = (Info)secondDataGrid.SelectedItem;
                
                //DataGridRow item = e.Row;
                //DataRowView item = e.Row.Item as DataRowView;
                //var c = item.DataContext as Info;

                Info rowDataContext = e.Row.DataContext as Info;

                if (rowDataContext != null)
                {
                    //DataRow row = item.Row;
                    //string row =
                    //string itemName = (string) row["Name"];
                    string rowItemName = rowDataContext.Name;
                    //var itemNameValue = row["Name"];
                    if (listDiff != null)
                    {
                        foreach (var element in listDiff)
                        {
                            if (rowItemName.Equals(element.Name))
                            {
                                e.Row.Foreground = new SolidColorBrush(Colors.White);
                                e.Row.Background = new SolidColorBrush(Colors.DarkRed);
                                
                            }
                            //else continue;
                        }
                    }
                    //secondDataGrid.Items.Refresh();
                }
            }
        }
        
        /* inserire thread per scansione cartelle e ricerca ricorsiva */

        private void firstDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            
            if (folder2TextBox.Text.Length != 0)
            {
                //showDiffTwoCollections();
                DataGridRow item = e.Row;
                var c = item.DataContext as Info;

                if (c != null)
                {
                    string row = c.Name;

                    //var itemNameValue = row["Name"];
                    if (listDiff != null)
                    {
                        foreach (var element in listDiff)
                        {
                            if (row == element.Name)
                            {
                                e.Row.Foreground = new SolidColorBrush(Colors.White);
                                e.Row.Background = new SolidColorBrush(Colors.DarkRed);
                            }
                        }
                    }
                }
            }
        }

        private void updateDatagrids()
        {
            showDiffTwoCollections();
            firstDataGrid.ItemsSource = filesList1;
            firstDataGrid.DataContext = filesList1;
            secondDataGrid.ItemsSource = filesList2;
            secondDataGrid.DataContext = filesList2;
        }

        private void updateFirstDatagrid()
        {
            //showDiffTwoCollections();
            firstDataGrid.ItemsSource = filesList1;
            firstDataGrid.DataContext = filesList1;
            firstDataGrid.Items.Refresh();
        }

        private void updateSecondDatagrid()
        {
            //showDiffTwoCollections();
            secondDataGrid.ItemsSource = filesList2;
            secondDataGrid.DataContext = filesList2;
            secondDataGrid.Items.Refresh();
        }

        private void recursiveScanCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (recursiveScanCheck.IsChecked == true)
            {
                RecursiveScanCheck = true;
                if (path1.Length != 0 && path2.Length != 0)
                {
                    workerRecursive.RunWorkerAsync();
                    //scanForFiles1(path1);
                 // scanForFiles2(path2);
                   // updateDatagrids();
                    //worker1.RunWorkerAsync(path1);
                    //worker2.RunWorkerAsync(path2);
                    //updateDatagrids();
                    //firstDataGrid.Items.Refresh();
                   // secondDataGrid.Items.Refresh();
                    if (!workerRecursive.IsBusy)
                    {
                        checkIfFoldersAreEqual();
                    }
                    //showDiffTwoCollections();
                }
            }
            //else if(recursiveScanCheck.IsChecked == false)
            //{
            ////    RecursiveScanCheck = false;
            ////    if (path1.Length != 0 && path2.Length != 0)
            ////    {
            ////       workerRecursive.RunWorkerAsync();
            ////       if (!workerRecursive.IsBusy)
            ////       {
            ////           System.Threading.Thread.Sleep(500);
            ////           //worker1.RunWorkerAsync(path1);
            ////           // worker2.RunWorkerAsync(path2);
            ////           //updateDatagrids();
            ////           //scanForFiles1(path1);
            ////           //  scanForFiles2(path2);

            ////           //updateDatagrids();
            ////           // firstDataGrid.Items.Refresh();
            ////           //secondDataGrid.Items.Refresh();
            ////           checkIfFoldersAreEqual();
            ////       }
            ////        //showDiffTwoCollections();
            ////    }
            ////}

        }

        private void recursiveScanCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            RecursiveScanCheck = false;
                if (path1.Length != 0 && path2.Length != 0)
                {
                   workerRecursive.RunWorkerAsync();
                   if (!workerRecursive.IsBusy)
                   {
                       System.Threading.Thread.Sleep(500);
                       //worker1.RunWorkerAsync(path1);
                       // worker2.RunWorkerAsync(path2);
                       //updateDatagrids();
                       //scanForFiles1(path1);
                       //  scanForFiles2(path2);

                       //updateDatagrids();
                       // firstDataGrid.Items.Refresh();
                       //secondDataGrid.Items.Refresh();
                       checkIfFoldersAreEqual();
                   }
                    //showDiffTwoCollections();
                }
            }

        private void firstDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IInputElement element = e.MouseDevice.DirectlyOver;
            if (element != null && element is FrameworkElement)
            {
                if (((FrameworkElement)element).Parent is System.Windows.Controls.DataGridCell)
                {
                    var grid = sender as System.Windows.Controls.DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        var rowview = grid.SelectedItem as DataRowView;
                        if (rowview != null)
                        {
                            DataRow row = rowview.Row;
                            System.Windows.Forms.MessageBox.Show("selected: " + row["Name"].ToString());
                        }
                    }
                }
            }
        }

        }

}
