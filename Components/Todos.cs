using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Components
{
    using System.ComponentModel;
    using System.Data.SQLite;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Security.RightsManagement;
    using System.Windows.Input;

    using WpfApp1.Annotations;
    using WpfApp1.Models;

    public class Todos : INotifyPropertyChanged
    {
        public ICommand AddCompleted { get; private set; }
        public ICommand AddRemoved{ get; private set; }

        public string Text { get; private set; }

        private bool isCompleted;
        public bool IsCompleted
        {
            get => this.isCompleted;
            set
            {
                this.isCompleted = value;
                this.OnPropertyChanged();
            } 
        }

        private string date;
        public string Date
        {
            get => date; 
            set
            {
                this.date = value;
                this.OnPropertyChanged();
            } 
        }
        
        public Todos(string str, int isCompleted = 0, string s = null)
        {
            Text = str;
            Date = s is null ? "" : s;
            AddCompleted = new ToDoListViewModel.AddCommand(this.Complete);
            AddRemoved = new ToDoListViewModel.AddCommand(this.OnAddRemoved);
            IsCompleted = isCompleted != 0;
        }

        private void OnAddRemoved(object s)
        {
            Debug.WriteLine("Hello");
        }

        public void Complete(object param)
        {
            using (var connection = new SQLiteConnection("Data Source=todos.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE todos SET Checked=@b WHERE Title=@a";
                command.Parameters.AddWithValue("a", this.Text);
                command.Parameters.AddWithValue("b", this.isCompleted);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
