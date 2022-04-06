using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows.Shapes;

namespace WPFDynamicTab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TabItem> _tabItems;
        private TabItem _tabAdd;
        private Dictionary<String, String> FilePaths;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                
                _tabItems = new List<TabItem>();

                 
                _tabAdd = new TabItem();
                _tabAdd.Header = "+";
              

                _tabItems.Add(_tabAdd);
                
                FilePaths = new Dictionary<String, String>();
                
                this.LoadDirectories();
                this.AddTabItem();

               
                tabDynamic.DataContext = _tabItems;

                tabDynamic.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private TabItem AddTabItem()
        {
            int count = _tabItems.Count;

           
            TabItem tab = new TabItem();
            
            tab.Header = string.Format("Tab {0}", count);
            tab.Name = string.Format("tab{0}", count);
            tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;
            tab.MouseDoubleClick += new MouseButtonEventHandler(tab_MouseDoubleClick);

           
            TextBox txt = new TextBox();
            txt.Name = "txt";

            tab.Content = txt;

          
            _tabItems.Insert(count - 1, tab);

            return tab;
        }

        private void tabAdd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
          
            tabDynamic.DataContext = null;

            TabItem tab = this.AddTabItem();

            
            tabDynamic.DataContext = _tabItems;

           
            tabDynamic.SelectedItem = tab;
        }

        private void tab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TabItem tab = sender as TabItem;

            TabProperty dlg = new TabProperty();
            
            dlg.txtTitle.Text = tab.Header.ToString();

            if (dlg.ShowDialog() == true)
            {
                
                tab.Header = dlg.txtTitle.Text.Trim();
            }
        }

        private void tabDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tab = tabDynamic.SelectedItem as TabItem;
            if (tab == null) return;

            if (tab.Equals(_tabAdd))
            {
                // clear tab control binding
                tabDynamic.DataContext = null;

                TabItem newTab = this.AddTabItem();

                // bind tab control
                tabDynamic.DataContext = _tabItems;

                // select newly added tab item
                tabDynamic.SelectedItem = newTab;
            }
            else
            {
                
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string tabName = (sender as Button).CommandParameter.ToString();

            var item = tabDynamic.Items.Cast<TabItem>().Where(i => i.Name.Equals(tabName)).SingleOrDefault();

            TabItem tab = item as TabItem;

            if (tab != null)
            {
                if (_tabItems.Count < 3)
                {
                    MessageBox.Show("Cannot remove last tab.");
                }
                else if (MessageBox.Show(string.Format("Are you sure you want to remove the tab '{0}'?", tab.Header.ToString()),
                    "Remove Tab", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // get selected tab
                    TabItem selectedTab = tabDynamic.SelectedItem as TabItem;
                    var data = (tab.Content as TextBox).Text;

                    if (!data.Equals(String.Empty))
                    {


                        MessageBoxResult exitSaveDialog = MessageBox.Show("Do you want to save this tab?",
                            "Save Tab", MessageBoxButton.YesNo);
                        if (exitSaveDialog == MessageBoxResult.Yes)
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
                            if (saveFileDialog.ShowDialog() == true)
                            {
                                File.WriteAllText(saveFileDialog.FileName, data);
                                var filename = saveFileDialog.FileName.ToLower();
                                RemoveSpecialChars(ref filename);
                                tab.Name = filename;
                                if (!FilePaths.ContainsKey(filename))
                                {
                                    FilePaths.Add(filename, saveFileDialog.FileName);
                                }
                                tab.Header = saveFileDialog.FileName;
                            }
                            
                        }

                        
                    }
                   
                    
                    tabDynamic.DataContext = null;

                    _tabItems.Remove(tab);

                    
                    tabDynamic.DataContext = _tabItems;

                    
                    if (selectedTab == null || selectedTab.Equals(tab))
                    {
                        selectedTab = _tabItems[0];
                    }
                    tabDynamic.SelectedItem = selectedTab;
                }
            }
        }
 
        private void btnSaveFileAs_Click(object sender, RoutedEventArgs e)
        {
            
            TabItem tab = tabDynamic.SelectedItem as TabItem;
            var data = (tab.Content as TextBox).Text;
            

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, data);
                var filename = saveFileDialog.FileName.ToLower();
                RemoveSpecialChars(ref filename);
                tab.Name = filename;
                if (!FilePaths.ContainsKey(filename))
                {
                    FilePaths.Add(filename, saveFileDialog.FileName);
                }
                tab.Header = saveFileDialog.FileName;
            }
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {

           
            tabDynamic.DataContext = null;

            TabItem tab = this.AddTabItem();

            
            tabDynamic.DataContext = _tabItems;

           
            tabDynamic.SelectedItem = tab;
        }
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.btnExit_Click(sender, e);
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

            TabItem tab = tabDynamic.SelectedItem as TabItem;
            var data = (tab.Content as TextBox).Text;
            
            if (!data.Equals(String.Empty))
            {


                MessageBoxResult exitSaveDialog = MessageBox.Show("Do you want to save this tab?",
                    "Exit Application", MessageBoxButton.YesNoCancel);
                if (exitSaveDialog == MessageBoxResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        File.WriteAllText(saveFileDialog.FileName, data);
                        var filename = saveFileDialog.FileName.ToLower();
                        RemoveSpecialChars(ref filename);
                        tab.Name = filename;
                        if (!FilePaths.ContainsKey(filename))
                        {
                            FilePaths.Add(filename, saveFileDialog.FileName);
                        }
                        tab.Header = saveFileDialog.FileName;
                    }
                    System.Windows.Application.Current.Shutdown();
                }
                else if (exitSaveDialog == MessageBoxResult.No)
                {
                    System.Windows.Application.Current.Shutdown();
                }

                else
                {

                }
            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
    

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            TabItem tab = tabDynamic.SelectedItem as TabItem;
            var data = (tab.Content as TextBox).Text;
            Search search = new Search();
            search.tab = tab;
            if ((bool)search.ShowDialog())
            {
                
            }

        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {

            TabItem tab = tabDynamic.SelectedItem as TabItem;
            var data = (tab.Content as TextBox).Text;

            if (FilePaths.ContainsKey(tab.Name))
            {
                File.WriteAllText(FilePaths[tab.Name], data);
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, data);
                    var filename = saveFileDialog.FileName.ToLower();
                    RemoveSpecialChars(ref filename);
                    tab.Name = filename;
                    if (!FilePaths.ContainsKey(filename))
                    {
                        FilePaths.Add(filename, saveFileDialog.FileName);
                    }
                    tab.Header =saveFileDialog.FileName;
                    
                }
            }

        }

        public static string RemoveSpecialChars(ref string str)
        {
            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\\", ";", "_", "(", ")", ":", "|", "[", "]", "-", " " };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "");
                }
            }
            return str;
        }
        
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {

           

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                TabItem tab = AddTabItem();
                var filename = openFileDialog.FileName.ToLower();
                RemoveSpecialChars(ref filename);
                tab.Name = filename;
                tab.Header = openFileDialog.FileName;
                var fileText = File.ReadAllText(openFileDialog.FileName);
                TextBox txt = new TextBox();
                txt.Text = fileText;
                tab.Content = txt;
                if (!FilePaths.ContainsKey(filename))
                {
                    FilePaths.Add(filename, openFileDialog.FileName);
                    
                tabDynamic.DataContext = null;

                
                tabDynamic.DataContext = _tabItems;

                
                tabDynamic.SelectedItem = tab;
                }
                else
                {
                    MessageBoxButton cantOpenTwice = MessageBoxButton.OK;
                    MessageBox.Show("You have already opened the selected file!");
                }
                
                
            }
                
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutPage aboutPage = new AboutPage();
            if ((bool)aboutPage.ShowDialog() == true)
            {

            }
        }
        public void LoadDirectories()
        {
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                this.treeView.Items.Add(this.GetItem(drive));
            }
        }
        private TreeViewItem GetItem(DriveInfo drive)
        {
            var item = new TreeViewItem
            {
                Header = drive.Name,
                DataContext = drive,
                Tag = drive
            };
            this.AddDummy(item);
            item.Expanded += new RoutedEventHandler(item_Expanded);
            return item;
        }
        private TreeViewItem GetItem(DirectoryInfo directory)
        {
            var item = new TreeViewItem
            {
                Header = directory.Name,
                DataContext = directory,
                Tag = directory
            };
            this.AddDummy(item);
            item.Expanded += new RoutedEventHandler(item_Expanded);
            return item;
        }

        private TreeViewItem GetItem(FileInfo file)
        {
            var item = new TreeViewItem
            {
                Header = file.Name,
                DataContext = file,
                Tag = file
            };
            return item;
        }
        private void AddDummy(TreeViewItem item)
        {
            item.Items.Add(new DummyTreeViewItem());
        }

        private bool HasDummy(TreeViewItem item)
        {
            return item.HasItems && (item.Items.OfType<TreeViewItem>().ToList().FindAll(tvi => tvi is DummyTreeViewItem).Count > 0);
        }

        private void RemoveDummy(TreeViewItem item)
        {
            var dummies = item.Items.OfType<TreeViewItem>().ToList().FindAll(tvi => tvi is DummyTreeViewItem);
            foreach (var dummy in dummies)
            {
                item.Items.Remove(dummy);
            }
        }

        private void ExploreDirectories(TreeViewItem item)
        {
            var directoryInfo = (DirectoryInfo)null;
            if (item.Tag is DriveInfo)
            {
                directoryInfo = ((DriveInfo)item.Tag).RootDirectory;
            }
            else if (item.Tag is DirectoryInfo)
            {
                directoryInfo = (DirectoryInfo)item.Tag;
            }
            else if (item.Tag is FileInfo)
            {
                directoryInfo = ((FileInfo)item.Tag).Directory;
            }
            if (object.ReferenceEquals(directoryInfo, null)) return;
            foreach (var directory in directoryInfo.GetDirectories())
            {
                var isHidden = (directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                var isSystem = (directory.Attributes & FileAttributes.System) == FileAttributes.System;
                if (!isHidden && !isSystem)
                {
                    item.Items.Add(this.GetItem(directory));
                }
            }
        }

        private void ExploreFiles(TreeViewItem item)
        {
            var directoryInfo = (DirectoryInfo)null;
            if (item.Tag is DriveInfo)
            {
                directoryInfo = ((DriveInfo)item.Tag).RootDirectory;
            }
            else if (item.Tag is DirectoryInfo)
            {
                directoryInfo = (DirectoryInfo)item.Tag;
            }
            else if (item.Tag is FileInfo)
            {
                directoryInfo = ((FileInfo)item.Tag).Directory;
            }
            if (object.ReferenceEquals(directoryInfo, null)) return;
            foreach (var file in directoryInfo.GetFiles())
            {
                var isHidden = (file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                var isSystem = (file.Attributes & FileAttributes.System) == FileAttributes.System;
                if (!isHidden && !isSystem)
                {
                    item.Items.Add(this.GetItem(file));
                }
            }
        }
        void item_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (this.HasDummy(item))
            {
                this.Cursor = Cursors.Wait;
                this.RemoveDummy(item);
                this.ExploreDirectories(item);
                this.ExploreFiles(item);
                this.Cursor = Cursors.Arrow;
            }
        }
        

    }

}
