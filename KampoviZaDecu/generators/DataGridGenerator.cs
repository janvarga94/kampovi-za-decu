using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace KampoviZaDecu.generators
{
    class DataGridGenerator
    {
        public void Generate(Type model, string[] modelNameMapping, DataGrid datagrid)
        {
            for (int i = 0; i < modelNameMapping.Length; i++)
            {
                var current = modelNameMapping[i];
                var currentDeteProperty = model.GetProperties().ElementAt(i);

                if (currentDeteProperty.PropertyType == typeof(string))
                {
                    datagrid.Columns.Add(new DataGridTextColumn() { Header = current, Binding = new Binding(currentDeteProperty.Name) });
                }
                else if (currentDeteProperty.PropertyType == typeof(bool))
                {
                    datagrid.Columns.Add(new DataGridCheckBoxColumn() { Header = current, Binding = new Binding(currentDeteProperty.Name) });
                }
            }
        }
    }
}
