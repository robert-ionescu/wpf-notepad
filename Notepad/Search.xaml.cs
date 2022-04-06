using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFDynamicTab
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        
        private Regex regex;
        private Match match;

       
        private bool isFirstFind = true;
        public TabItem tab { get; set; }
        public Search()
        {
            
            InitializeComponent();
        }
        private void findButton_Click(object sender, EventArgs e)
        {
            FindText();
            //tab.Focus();
        }
        private void FindText()
        {
            
            var data = (tab.Content as TextBox).Text;
           
            if (isFirstFind)
            {
                regex = this.GetRegExpression();
                match = regex.Match(data);
                isFirstFind = false;
            }
            else
            {
           
                match = regex.Match(data, match.Index + 1);
            }

           
            if (match.Success)
            {
                
                (tab.Content as TextBox).SelectionStart = match.Index;
                (tab.Content as TextBox).SelectionLength = match.Length;
            }
            else 
            {
               
                
                MessageBox.Show(String.Format("Cannot find '{0}'.", searchTextBox.Text.ToString()));

                isFirstFind = true;
            }
        }
        public Regex GetRegExpression()
        {
            Regex result;
            String regExString;

            // Get what the user entered
            regExString = searchTextBox.Text;

            if ((bool)useRegulatExpressionCheckBox.IsChecked)
            {
               
            }
            
            else if ((bool)useWildcardsCheckBox.IsChecked)
            {
                regExString = regExString.Replace("*", @"\w*");     
                regExString = regExString.Replace("?", @"\w");     

                
                regExString = String.Format("{0}{1}{0}", @"\b", regExString);
            }
            else
            {
                
                regExString = Regex.Escape(regExString);
            }

            
            if ((bool)matchWholeWordCheckBox.IsChecked)
            {
                regExString = String.Format("{0}{1}{0}", @"\b", regExString);
            }

           
            if ((bool)matchCaseCheckBox.IsChecked)
            {
                result = new Regex(regExString);
            }
            else
            {
                result = new Regex(regExString, RegexOptions.IgnoreCase);
            }

            return result;
        }

        private void replaceAllButton_Click(object sender, RoutedEventArgs e)
        {
            
            var data = (tab.Content as TextBox).Text;

            Regex replaceRegex = this.GetRegExpression();
            String replacedString;

           
            int selectedPos = (tab.Content as TextBox).SelectionStart;

           
            replacedString = replaceRegex.Replace
                ((tab.Content as TextBox).Text, this.replaceTextBox.Text);

            
            if ((tab.Content as TextBox).Text != replacedString)
            {
                
                (tab.Content as TextBox).Text = replacedString;
                MessageBox.Show("Replacements are made.");

                
                (tab.Content as TextBox).SelectionStart = selectedPos;
            }
            else 
            {
                MessageBox.Show("Cannot find '{0}'.   ", this.searchTextBox.Text);
            }

            (tab.Content as TextBox).Focus();
        }

        private void replaceButton_Click(object sender, EventArgs e)
        {
            var data = (tab.Content as TextBox).Text;
           
            Regex regexTemp = GetRegExpression();
            Match matchTemp = regexTemp.Match((tab.Content as TextBox).SelectedText);

            if (matchTemp.Success)
            {
              
                if (matchTemp.Value == (tab.Content as TextBox).SelectedText)
                {
                    (tab.Content as TextBox).SelectedText = replaceTextBox.Text;
                }
            }

            FindText();
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            isFirstFind = true;
        }


        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void matchWholeWordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            isFirstFind = true;
        }


        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void matchCaseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            isFirstFind = true;
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void useWildcardsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            isFirstFind = true;
        }
    }
}
