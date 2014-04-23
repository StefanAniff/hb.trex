using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Trex.Invoices.InvoiceManagementScreen.CustomerTreeView;

namespace Trex.Invoices.Implemented.InvoiceManagementScreen.CustomerTreeView
{
    public class CustomListViewBox
    {
        public static DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(CustomListViewBox),
                new PropertyMetadata(null, OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject d)
        {
            return (IList)d.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject d, IList value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (ListBox)d;

            listBox.SelectionChanged += delegate
            {
               ReSetSelectedItems(listBox);
            };
        }

        private static void ReSetSelectedItems(ListBox listBox)
        {
            var selectedItems = GetSelectedItems(listBox);
            
            selectedItems.Clear();


            if (listBox.SelectedItems != null)
            {
                var temp = selectedItems;
                foreach (var item in listBox.SelectedItems)
                {
                    temp.Add(item);
                }
                selectedItems = temp;
            }
        }
    }
}