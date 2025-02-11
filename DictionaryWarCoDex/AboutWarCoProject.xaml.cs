﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace DictionaryWarCoDex
{
    /// <summary>
    /// Interaction logic for AboutWarCoProject.xaml
    /// </summary>
    public partial class AboutWarCoProject : Window
    {
        public AboutWarCoProject()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public string theDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(theDirectory + @"..\..\About\release_notes.docx");
        }

        private void TextBlock_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            Process.Start(theDirectory + @"..\..\About\License.txt");
        }

        private void TextBlock_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            Process.Start(theDirectory + @"..\..\About\MadeBy.txt");
        }
    }
}
