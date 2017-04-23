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
                    var boxes = CreateControlForNumericProperty(modelNameOfLocalProperty, modelProperty);
                    boxes.Item1.TextChanged += TextBox_TextChanged;
                    boxes.Item2.TextChanged += TextBox_TextChanged;

                    var subGrid = new Grid();
                    subGrid.RowDefinitions.Add(new RowDefinition());
                    subGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    subGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30)});
                    subGrid.ColumnDefinitions.Add(new ColumnDefinition());

                    var lab = new Label() { Content = "do", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };

                    Grid.SetColumn(boxes.Item1, 0);
                    Grid.SetRow(boxes.Item1, 0);

                    Grid.SetColumn(lab, 1);
                    Grid.SetRow(lab, 0);

                    Grid.SetColumn(boxes.Item2, 2);
                    Grid.SetRow(boxes.Item2, 0);

                    subGrid.Children.Add(boxes.Item1);
                    subGrid.Children.Add(boxes.Item2);
                    subGrid.Children.Add(lab);


                    Grid.SetColumn(subGrid, 2);
                    Grid.SetRow(subGrid, i);

                    grid.Children.Add(subGrid);

                    predicates.Add(dete => {
                        if (filterForPropertyEnabledCheckbox.IsChecked == true)
                        {
                            try
                            {
                                if(
                                    double.Parse(boxes.Item1.Text) > ((double)modelProperty.GetValue(dete)) ||
                                    double.Parse(boxes.Item2.Text) < ((double)modelProperty.GetValue(dete))
                                )
                                {
                                    return false;
                                }
                            }
                            catch
                            {
                                return false;
                            }
                        }
                        return true;
                    });
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

        private static Tuple<TextBox,TextBox> CreateControlForNumericProperty(string modelNameOfLocalProperty, System.Reflection.PropertyInfo modelProperty)
        {
            var controlViewToCreate1 = new TextBox() { Text = "0", HorizontalAlignment = HorizontalAlignment.Stretch, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontSize = 20 };
            var controlViewToCreate2 = new TextBox() { Text = "0", HorizontalAlignment = HorizontalAlignment.Stretch, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontSize = 20 };


            controlViewToCreate1.PreviewTextInput += ControlViewToCreate_PreviewTextInput;
            controlViewToCreate2.PreviewTextInput += ControlViewToCreate_PreviewTextInput;


            return new Tuple<TextBox, TextBox>(controlViewToCreate1, controlViewToCreate2);
        }

        private static void ControlViewToCreate_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^-?\d*[.]?\d*$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private static Control CreateControlForPropertyOfUnknowType(string modelNameOfLocalProperty, System.Reflection.PropertyInfo modelProperty)
        {
            Control controlViewToCreate = new Label() { Content = "unimplemented", HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontSize = 20 };

            return controlViewToCreate;
        }
    }
}
