using KampoviZaDecu.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KampoviZaDecu.generators
{
    public delegate void OnFilterChange();

    class FilterFormGenerator
    {
        public event OnFilterChange FilterChange;

        public Predicate<object> Generate(object model, string modelNameOfLocalProperty, string[] modelNameMapping, Grid grid)
        {
            var predicates = new List<Predicate<object>>();

            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < modelNameMapping.Length; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());

                var current = modelNameMapping[i];
                var modelProperty = model.GetType().GetProperties().ElementAt(i);

                var label = new Label() { Content = current, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, FontSize = 20 };
                Grid.SetColumn(label, 1);
                Grid.SetRow(label, i);

                var filterForPropertyEnabledCheckbox = new CheckBox() { VerticalAlignment = VerticalAlignment.Center, IsChecked = false };
                Grid.SetColumn(filterForPropertyEnabledCheckbox, 0);
                Grid.SetRow(filterForPropertyEnabledCheckbox, i);

                filterForPropertyEnabledCheckbox.Checked += Checkbox_CheckedChanged;
                filterForPropertyEnabledCheckbox.Unchecked += Checkbox_CheckedChanged;

                grid.Children.Add(label);
                grid.Children.Add(filterForPropertyEnabledCheckbox);

                Control controlViewToCreate = null;

                if (modelProperty.PropertyType == typeof(string))
                {
                    TextBox textBox;
                    controlViewToCreate = textBox = CreateControlForStringProperty(modelNameOfLocalProperty, modelProperty);
                    predicates.Add(dete => {
                        if (filterForPropertyEnabledCheckbox.IsChecked == true)
                        {
                            if(modelProperty.GetValue(dete) == null)
                                return false;
                            if(!((string)modelProperty.GetValue(dete)).ToLower().Contains(textBox.Text.ToLower()))
                                return false;
                        }              
                        return true;
                    });
                    textBox.TextChanged += TextBox_TextChanged;
                }
                else if (modelProperty.PropertyType == typeof(bool))
                {
                    CheckBox checkbox;
                    controlViewToCreate = checkbox = CreateControlForBoolProperty(modelNameOfLocalProperty, modelProperty);
                    predicates.Add(dete => {
                        if (
                         filterForPropertyEnabledCheckbox.IsChecked == true && modelProperty.GetValue(dete) != null && ((bool)modelProperty.GetValue(dete)) != checkbox.IsChecked
                        )
                        {
                            return false;
                        }
                        return true;
                    });
                    checkbox.Checked += Checkbox_CheckedChanged;
                    checkbox.Unchecked += Checkbox_CheckedChanged;
                }
                else if (modelProperty.PropertyType.IsNumber())
                {
                    TextBox textbox;
                    controlViewToCreate = textbox = CreateControlForNumericProperty(modelNameOfLocalProperty, modelProperty);
                    textbox.TextChanged += TextBox_TextChanged;
                    //TODO filter
                }
                else
                {
                    controlViewToCreate = CreateControlForPropertyOfUnknowType(modelNameOfLocalProperty, modelProperty);
                }

                if (controlViewToCreate != null)
                {
                    Grid.SetColumn(controlViewToCreate, 2);
                    Grid.SetRow(controlViewToCreate, i);

                    grid.Children.Add(controlViewToCreate);
                }

            }

            return (object obj) => {
                bool result = true;
                foreach (var predicate in predicates)
                {
                    result = result && predicate.Invoke(obj);
                }
                return result;
            };
        }

        private void Checkbox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            FilterChange?.Invoke();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterChange?.Invoke();
        }

        private static CheckBox CreateControlForBoolProperty(string modelNameOfLocalProperty, System.Reflection.PropertyInfo modelProperty)
        {
            var controlViewToCreate = new CheckBox() { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center };
            
            return controlViewToCreate;
        }

        private static TextBox CreateControlForStringProperty(string modelNameOfLocalProperty, System.Reflection.PropertyInfo modelProperty)
        {
            var controlViewToCreate = new TextBox() { HorizontalAlignment = HorizontalAlignment.Stretch, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontSize = 20 };
            
         
            return controlViewToCreate;
        }

        private static TextBox CreateControlForNumericProperty(string modelNameOfLocalProperty, System.Reflection.PropertyInfo modelProperty)
        {
            var controlViewToCreate = new TextBox() { HorizontalAlignment = HorizontalAlignment.Stretch, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontSize = 20 };
            

            controlViewToCreate.PreviewTextInput += ControlViewToCreate_PreviewTextInput;


            return controlViewToCreate;
        }

        private static void ControlViewToCreate_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^-?[0-9]*\\.[0-9]*");
            e.Handled = regex.IsMatch(e.Text);
        }

        private static Control CreateControlForPropertyOfUnknowType(string modelNameOfLocalProperty, System.Reflection.PropertyInfo modelProperty)
        {
            Control controlViewToCreate = new Label() { Content = "unimplemented", HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontSize = 20 };

            return controlViewToCreate;
        }
    }
}
