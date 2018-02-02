using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    using MaterialDesignThemes.Wpf;

    using WpfApp1.Components;

    /// <summary>
    /// Interaction logic for toDoList.xaml
    /// </summary>
    public partial class toDoList : UserControl
    {

        private Models.ToDoListViewModel model;
        public toDoList()
        {
            InitializeComponent();

            model = new Models.ToDoListViewModel();
            DataContext = model;
        }

        private void DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventargs)
        {
            this.model.OnAddToDo(sender);
        }
    }
}
