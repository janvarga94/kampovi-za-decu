using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KampoviZaDecu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotify implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
        private Dete _currentDete = new Dete();
        private Dete _selectedDete;

        public Dete CurrentDete
        {
            get { return _currentDete; }
            set
            {
                if (value == null)
                    _currentDete = new Dete();
                else
                    _currentDete = value;

                NotifyPropertyChanged(nameof(CurrentDete));
            }
        }
        public Dete SelectedDete
        {
            get { return _selectedDete; }
            set
            {
                _selectedDete = value;
                NotifyPropertyChanged(nameof(SelectedDete));
                CurrentDete = value;
                if (value != null)
                {
                    izmenaRadio.IsChecked = true;
                    Stanje = Stanje.Izmena;
                }

            }
        }

        public ObservableCollection<Dete> Deca { get; set; } = new ObservableCollection<Dete>();

        private Stanje _stanje;
        public Stanje Stanje
        {
            get { return _stanje; }
            set
            {
                _stanje = value; NotifyPropertyChanged(nameof(Stanje));
                switch (_stanje)
                {
                    case Stanje.Izmena:
                        break;
                    case Stanje.Dodavanje:
                        tabela.UnselectAll();
                        break;
                }
            }
        }

        private string _currentProjekatPath = "<projekat nije odabran>";
        public string CurrentProjekatPath
        {
            get { return _currentProjekatPath; }
            set
            {
                _currentProjekatPath = value;
                NotifyPropertyChanged(nameof(CurrentProjekatPath));
                if (value != null)
                    ProjekatOdabran = true;
            }
        }

        private bool _projekatOdabran = false;
        public bool ProjekatOdabran
        {
            get
            {
                return _projekatOdabran;
            }

            set
            {
                _projekatOdabran = value;
                NotifyPropertyChanged(nameof(ProjekatOdabran));
            }
        }

        private string _nazivProjekta = "<projekat nije odabran>";
        public string NazivProjekta
        {
            get { return _nazivProjekta; }
            set
            {
                _nazivProjekta = value;
                NotifyPropertyChanged(nameof(NazivProjekta));
            }
        }

        private ICollectionView _collectionView;

        private string _search = "";
        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                NotifyPropertyChanged(nameof(Search));
                _collectionView.Refresh();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            _collectionView = CollectionViewSource.GetDefaultView(Deca);


            for (int i = 0; i < App.DecaHeader.Length; i++)
            {
                var current = App.DecaHeader[i];
                var currentDeteProperty = typeof(Dete).GetProperties().ElementAt(i);
                var nameOfCurrentDeteProperty = typeof(Dete).GetProperties().Select(p => p.Name).ElementAt(i);
                
                deteForm.RowDefinitions.Add(new RowDefinition());

                var label = new Label() { Content = current, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, FontSize = 20 };
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, i);

                deteForm.Children.Add(label);

                if (currentDeteProperty.PropertyType == typeof(string))
                {
                    tabela.Columns.Add(new DataGridTextColumn() { Header = current, Binding = new Binding(nameOfCurrentDeteProperty) });

                    var textBox = new TextBox() { HorizontalAlignment = HorizontalAlignment.Stretch, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, FontSize = 20 };
                    Binding binding = new Binding();
                    binding.Path = new PropertyPath(nameof(CurrentDete) + "." + nameOfCurrentDeteProperty);
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                    BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
                    Grid.SetColumn(textBox, 1);
                    Grid.SetRow(textBox, i);

                    deteForm.Children.Add(textBox);

                    if (currentDeteProperty == typeof(Dete).GetProperty(nameof(Dete.KojiSport)))
                    {
                        Binding binding2 = new Binding();
                        binding2.Path = new PropertyPath(nameof(CurrentDete) + "." + nameof(CurrentDete.Sport));
                        binding2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                        BindingOperations.SetBinding(textBox, TextBox.IsEnabledProperty, binding2);
                    }
                }
                else if (currentDeteProperty.PropertyType == typeof(bool))
                {
                    tabela.Columns.Add(new DataGridCheckBoxColumn() { Header = current, Binding = new Binding(nameOfCurrentDeteProperty) });

                    var textBox = new CheckBox() { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center };
                    Binding binding = new Binding();
                    binding.Path = new PropertyPath(nameof(CurrentDete) + "." + nameOfCurrentDeteProperty);
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                    BindingOperations.SetBinding(textBox, CheckBox.IsCheckedProperty , binding);
                    Grid.SetColumn(textBox, 1);
                    Grid.SetRow(textBox, i);

                    deteForm.Children.Add(textBox);          

                }

            }

            dodavanjeRadio.Checked += DodavanjeRadio_Checked;

            _collectionView.Filter = (object dete) =>
            {
                var ddete = (Dete)dete;
                if(ddete.Ime != null && ddete.Prezime != null)
                    return ddete.Ime.ToLower().Contains(_search.ToLower()) ||
                        ddete.Prezime.ToLower().Contains(_search.ToLower());

                Console.WriteLine("Bro, ovo ispravi");
                return false; //this shouldnt be happening
            };


        }

        private void DodavanjeRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)e.Source).IsChecked == true)
                Stanje = Stanje.Dodavanje;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {      
                Deca.Add(CurrentDete);
                CurrentDete = null;
        }

        private void tabela_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Deca.Remove(SelectedDete);
                dodavanjeRadio.IsChecked = true;
            }
        }

        //novi projekat
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.Title = "Create";

            openFileDialog1.ShowDialog();
            if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                var newFileFullPath = openFileDialog1.FileName;
                if (!newFileFullPath.Contains(".csv"))
                    newFileFullPath += ".csv";
                try
                {
                    File.Create(newFileFullPath).Close();
                    OdaberiProjekatDaRadimSaNjim(newFileFullPath);
                }
                catch
                {
                    MessageBox.Show("Nemogu kreirati fajl, pokusaj promeniti lokaciju");
                }
            }

        }

        //open postojeci
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            openFileDialog1.ShowDialog();
            if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
                OdaberiProjekatDaRadimSaNjim(openFileDialog1.FileName);
        }

        private void OdaberiProjekatDaRadimSaNjim(string fullPath)
        {
            try
            {
                CurrentProjekatPath = fullPath;
                var parsedDeca = FileThings.Parse(fullPath);
                Deca.Clear();
                foreach (var dete in parsedDeca)
                {
                    Deca.Add(dete);
                }
                ProjekatOdabran = true;
                NazivProjekta = fullPath.Split('\\').LastOrDefault()?.Replace(".csv", "");
            }
            catch
            {
                MessageBox.Show("Neka greska prilikom ucitavanja, mozda je fajl zauzet :/, zatvorite excel mozda");
            }
        }

        //save
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (!FileThings.Save(CurrentProjekatPath, Deca))
            {
                MessageBox.Show("Neka greska prilikom cuvanja, mozda je fajl zauzet :/, zatvorite excel mozda");
            }
        }

        //print
        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();

            if ((pd.ShowDialog() == true))
            {
                tabela.Margin = new Thickness(50, 50, 0, 0);

                await Task.Delay(200);

                pd.PrintVisual(DataGridViewBox, "Printing a UserControl");

                tabela.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void tabela_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
