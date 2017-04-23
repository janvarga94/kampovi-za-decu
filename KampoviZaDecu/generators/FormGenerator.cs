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
    class FormGenerator
    {
        public void Generate(object model, string modelNameOfLocalProperty, string[] modelNameMapping, Grid grid)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < modelNameMapping.Length; i++)
            {
                var current = modelNameMapping[i];
                var modelProperty = model.GetType().GetProperties().ElementAt(i);

                grid.RowDefinitions.Add(new RowDefinition());

                var label = new Label() { Content = current, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, FontSize = 20 };
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, i);

                grid.Children.Add(label);

                Control controlViewToCreate = null;

                if (modelProperty.PropertyType == typeof(string))
                {
                    controlViewToCreate = CreateControlForStringProperty(modelNameOfLocalProperty, modelProperty);
                }
                else if (modelProperty.PropertyType == typeof(bool))
                {
                    controlViewToCreate = CreateControlForBoolProperty(modelNameOfLocalProperty, modelProperty);
                }
                else if (modelProperty.PropertyType.IsNumber())
                {
                    controlViewToCreate = CreateControlForNumericProperty(modelNameOfLocalProperty, modelProperty);
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


                EnabledForAttribute attribute;              
                if (ReflectionHelper.TryGetAttribute(modelProperty, out attribute))
                {
                    Binding binding2 = new Binding();
                    binding2.Path = new PropertyPath(modelNameOfLocalProperty + "." + attribute.BindingProperty);
                    binding2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    BindingOperations.SetBinding(controlViewToCreate, TextBox.IsEnabledProperty, binding2);
                }
            }
        }
        private static Control CreateControlForBoolProperty(string modelNameOfLocalProperty, System.Reflection.PropertyInfo modelProperty)
        {
            Control controlViewToCreate = new CheckBox() { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center };
            Binding binding = new Binding();
            binding.Path = new PropertyPath(modelNameOfLocalProperty + "." + modelProperty.Name);
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            BindingOperations.SetBinding(controlViewToCreate, CheckBox.IsCheckedProperty, binding);

            return controlViewToCreate;
        }

        private static Control CreateControlForStringProperty(string modelNameOfLocalProperty, System.Reflection.PropertyInfo modelProperty)
        {
            Control controlViewToCreate = new TextBox() { HorizontalAlignment = HorizontalAlignment.Stretch, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontSize = 20 };
            Binding binding = new Binding();
            binding.Path = new PropertyPath(modelNameOfLocalProperty + "." + modelProperty.Name);
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            BindingOperations.SetBinding(controlViewToCreate, TextBox.TextProperty, binding);

            return controlViewToCreate;
        }

        private static Control CreateControlForNumericProperty(string modelNameOfLocalProperty, System.Reflection.PropertyInfo modelProperty)
        {
            Control controlViewToCreate = new TextBox() { HorizontalAlignment = HorizontalAlignment.Stretch, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontSize = 20 };
            Binding binding = new Binding();
            binding.Path = new PropertyPath(modelNameOfLocalProperty + "." + modelProperty.Name);
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            BindingOperations.SetBinding(controlViewToCreate, TextBox.TextProperty, binding);


            controlViewToCreate.PreviewTextInput += ControlViewToCreate_PreviewTextInput;


            return controlViewToCreate;
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
