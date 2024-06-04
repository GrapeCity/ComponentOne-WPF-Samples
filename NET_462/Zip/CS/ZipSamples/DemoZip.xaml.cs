using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.C1Zip;
using System.IO;
using Microsoft.Win32;

namespace ZipSamples
{

    public partial class DemoZip : Window
    {
        C1ZipFile _zip;
        CollectionViewSource _cvs = new CollectionViewSource();
        MemoryStream zipMemoryStream = null;

        public DemoZip()
        {
            this.InitializeComponent();

            _flex.SelectedItemChanged += _flex_SelectedItemChanged;

            // bind grid to entries collection
        }

        void _flex_SelectedItemChanged(object sender, EventArgs e)
        {
            if (_flex.SelectedItem != null)
            {
                _btnView.IsEnabled = true;
                _btnRemove.IsEnabled = true;
            }
            else
            {
                _btnView.IsEnabled = false;
                _btnRemove.IsEnabled = false;
            }
        }

        // refresh view when collection is modified
        void RefreshView()
        {
            //var sel = _flex.SelectedItem;
            _flex.ItemsSource = null;
            if (_zip == null)
            {
                return;
            }
            _flex.ItemsSource = _zip.Entries;
            if (_zip.Entries.Count == 0)
            {
                _btnCompress.IsEnabled = false;
                _btnRemove.IsEnabled = false;
                _btnView.IsEnabled = false;
                _btnExtract.IsEnabled = false;
                _zip = null;
            }
            //_flex.SelectedItem = sel;
        }

        // open an existing zip file
        void _btnOpen_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();

                op.RestoreDirectory = true;
                op.Filter = "Zip(*.zip)|*.zip";

                var open = op.ShowDialog();

                if (open.HasValue && open.Value)
                {

                    Clear();
                    progressBar.Visibility = Visibility.Visible;

                    if (_zip == null)
                    {
                        _zip = new C1ZipFile();
                    }

                    _zip.Open(op.FileName);
                    _btnExtract.IsEnabled = true;
                    RefreshView();

                }
            }
            catch (Exception x)
            {
                progressBar.Visibility = Visibility.Collapsed;
                // snapped?
                System.Diagnostics.Debug.WriteLine(x.Message);
            }
            progressBar.Visibility = Visibility.Collapsed;
        }

        // remove selected entries from zip
        void _btnRemove_Click(object sender, RoutedEventArgs e)
        {
            foreach (C1ZipEntry entry in _flex.SelectedItems)
            {
                _zip.Entries.Remove(entry.FileName);
            }
            RefreshView();
        }

        // show a preview of the selected entry
        void _btnView_Click(object sender, RoutedEventArgs e)
        {
            var entry = _flex.SelectedItem as C1ZipEntry;
            if (entry != null)
            {
                using (var stream = entry.OpenReader())
                {
                    var sr = new System.IO.StreamReader(stream);
                    _tbContent.Text = sr.ReadToEnd();
                }
                _preview.Visibility = Visibility.Visible;
                _mainpage.Visibility = Visibility.Collapsed;
            }
        }

        // close the preview pane by hiding it
        void _btnClosePreview_Click_1(object sender, RoutedEventArgs e)
        {
            _preview.Visibility = Visibility.Collapsed;
            _mainpage.Visibility = Visibility.Visible;
        }

        // add files by folder
        private void _btnPickFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
                dlg.ShowDialog();
                if (dlg.SelectedPath != null)
                {

                    if (_btnExtract.IsEnabled)
                    {
                        Clear();
                    }
                    progressBar.Visibility = Visibility.Visible;

                    if (zipMemoryStream==null)
                    {
                        zipMemoryStream = new MemoryStream();
                    }
                    if (_zip==null)
                    {
                        _zip = new C1ZipFile(zipMemoryStream, true); 
                    }
                    _zip.Entries.AddFolder(dlg.SelectedPath);

                    _btnCompress.IsEnabled = true;
                }

            }
            catch
            {
                progressBar.Visibility = Visibility.Collapsed;
                // snapped?
            }
            RefreshView();
            progressBar.Visibility = Visibility.Collapsed;
        }

        // add files
        private void _btnPickFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
                op.Multiselect = true;
                op.RestoreDirectory = true;
                op.Filter = "All files(*.*)|*.*";

                var open = op.ShowDialog();

                if (open.HasValue && open.Value)
                {
                    var files = op.FileNames;
                    if (files.Length == 0)
                    {
                        return;
                    }
                    if (_btnExtract.IsEnabled)
                    {
                        Clear();
                    }
                    progressBar.Visibility = Visibility.Visible;
                    if (zipMemoryStream == null)
                    {
                        zipMemoryStream = new MemoryStream();
                    }
                    if (_zip == null)
                    {
                        _zip = new C1ZipFile(zipMemoryStream, true);
                    }
                    foreach (var f in files)
                    {
                        _zip.Entries.Add(f);
                    }

                    _btnCompress.IsEnabled = true;
                }
            }
            catch
            {
                // snapped?
            }
            RefreshView();
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void _btnCompress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (zipMemoryStream != null)
                {
                   
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Zip(*.zip)|*.zip";
                    var save= saveFileDialog.ShowDialog();
                    if (save.HasValue && save.Value)
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, zipMemoryStream.ToArray());
                        MessageBox.Show("Compress Successfully, the path is :" + saveFileDialog.FileName);

                    }
                }
            }
            catch
            {
                progressBar.Visibility = Visibility.Collapsed;
                // snapped?
            }
            RefreshView();

        }

        private void _btnExtract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
                dlg.ShowDialog();
                if (dlg.SelectedPath != null)
                {
                    progressBar.Visibility = Visibility.Visible;

                    foreach (var entry in _zip.Entries)
                    {

                        var dstFileName = dlg.SelectedPath + @"\" + entry.FileName;
                        var dstFolder = System.IO.Path.GetDirectoryName(dstFileName);

                        if (dstFolder.Length > 0 && !Directory.Exists(dstFolder))
                        {
                            Directory.CreateDirectory(dstFolder);
                            var dstName = System.IO.Path.GetFileName(dstFileName);
                            if (dstName.Length > 0)
                            {
                                entry.Extract(dstFileName);
                            }
                        }
                        else
                        {
                            entry.Extract(dstFileName);
                        }
                    }
                    progressBar.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Extract Successfully, the path is :" + dlg.SelectedPath);

                }
            }
            catch (Exception)
            {
                progressBar.Visibility = Visibility.Collapsed;
               
            }
           
            RefreshView();
            
        }

        private void _btnClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            _flex.ItemsSource = null;

            if (zipMemoryStream != null)
            {
                zipMemoryStream.Flush();
                zipMemoryStream.Dispose();
            }
            zipMemoryStream = null;
            _btnCompress.IsEnabled = false;
            _btnExtract.IsEnabled = false;
            if (_zip != null)
            {
                _zip.Close();
                _zip = null;
            }
        }
    }
}
