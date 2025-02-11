﻿using System;
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
using System.Windows.Shapes;

namespace DictionaryWarCoDex
{
    /// <summary>
    /// Interaction logic for DictionaryAD.xaml
    /// </summary>
    public partial class DictionaryAD : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();
        public DictionaryAD()
        {
            InitializeComponent();
            FillUpListView();
            ToolTipShow();
        }

        private void FillUpListView()
        {
            var pom = from s in DC.Dictionaries
                      where s.UserID == LoginUserData.UseridLOG
                      select new { s.DictionaryID, s.DictionaryName };
            listView1.ItemsSource = pom;
        }
        private void ToolTipShow()
        {//Navodi da postoji conteksni meni...
            ToolTip tool = new ToolTip { Content = "Right click on item to rename or remove" };
            listView1.ToolTip = tool;
            tool.PlacementTarget = listView1;
            tool.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            tool.IsOpen = true;
        }

        private void BtnAddNewDictionary_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbNameOfNewDictionary.Text))
            {
                Dictionary pom = new Dictionary
                {
                    DictionaryName = tbNameOfNewDictionary.Text.Trim(),
                    DateOfCreation = DateTime.Today,
                    UserID = LoginUserData.UseridLOG
                };
                try
                {
                    DC.Dictionaries.InsertOnSubmit(pom);
                    DC.SubmitChanges();
                    tbNameOfNewDictionary.Clear();
                }
                catch
                {
                    MessageBox.Show("Error!");
                }
                finally
                {
                    FillUpListView();
                }
            }
            else
            {
                MessageBox.Show("You need name for your new dictionary!", "Insert name", MessageBoxButton.OK, MessageBoxImage.Information);
                tbNameOfNewDictionary.Focus();
            }
        }

        private void BtnDeleteCM(object sender, RoutedEventArgs e)
        {//Ako u recniku ima reci pitace dal da brise sve reci iz tog recnika a ako nema obrisace odma recnik
            //Puca ako se ne izabere nista !
            var kkk = from p in DC.Words.AsEnumerable()
                      where p.DictionaryID == (int)listView1.SelectedValue
                      select p;
            if (kkk.Any())
            {
                if (MessageBox.Show("You will delete all words in this dictionary, are you sure ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var result = from s in DC.Results
                                 join w in DC.Words
                                 on s.WordID equals w.WordID
                                 where w.DictionaryID == (int)listView1.SelectedValue
                                 select s;
                    DC.Results.DeleteAllOnSubmit(result);

                    var ppp = from k in DC.Words
                              join c in DC.Dictionaries
                              on k.DictionaryID equals c.DictionaryID
                              where k.DictionaryID == (int)listView1.SelectedValue 
                              select new { worddd = k, diccc = c};
                    foreach (var item in ppp)
                    {
                        DC.Words.DeleteOnSubmit(item.worddd);
                        DC.Dictionaries.DeleteOnSubmit(item.diccc);
                    }
                    try
                    {
                        DC.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong here" +ex, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        FillUpListView();
                    }
                }
            }
            else
            {
                try
                {
                    Dictionary pom = (from s in DC.Dictionaries
                                      where s.DictionaryID == (int)listView1.SelectedValue
                                      select s).First();
                    DC.Dictionaries.DeleteOnSubmit(pom);
                    DC.SubmitChanges();
                    MessageBox.Show("Selected item is deleted");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something is not okay here ! " + ex, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                finally
                {
                    FillUpListView();
                }
            }
        }

        private void BtnRenameCM(object sender, RoutedEventArgs e)
        {
            btnAddNewDictionary.Visibility = Visibility.Hidden;
            btnRename.Visibility = Visibility.Visible;
            tbNameOfNewDictionary.Focus();
            btnRename.IsDefault = true;
        }

        private void BtnRename_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbNameOfNewDictionary.Text))
                MessageBox.Show("Textbox can't be empty");
            else
            {
                btnRename.Visibility = Visibility.Hidden;
                btnAddNewDictionary.Visibility = Visibility.Visible;

                var pom = (from s in DC.Dictionaries
                           where s.DictionaryID == (int)listView1.SelectedValue
                           select s).First();
                pom.DictionaryName = tbNameOfNewDictionary.Text;
                try
                {
                    DC.SubmitChanges();
                    MessageBox.Show("You renamed your Dictionary !");
                    tbNameOfNewDictionary.Clear();
                }
                catch (Exception)
                {
                    MessageBox.Show("Error");
                }
                finally
                {
                    FillUpListView();
                }
            }
        }
    }
}
