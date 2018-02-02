// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToDoListViewModel.cs" company="Poc LLC">
//   :)
// </copyright>
// <summary>
//   Defines the ToDoListViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WpfApp1.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.SQLite;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using WpfApp1.Annotations;
    using WpfApp1.Components;

    /// <summary>
    /// The to do list view model.
    /// </summary>
    public class ToDoListViewModel : INotifyPropertyChanged
    {
        private string search;

        /// <summary>
        /// The input.
        /// </summary>
        private string input;

        /// <summary>
        /// The date.
        /// </summary>
        private string date;


        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListViewModel"/> class.
        /// </summary>
        public ToDoListViewModel()
        {
            this.Todos = new ObservableCollection<Todos>();
            this.SearchTodo = new ObservableCollection<Todos>();
            try
            {
                using (var connection = new SQLiteConnection("Data Source=todos.db"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS todos(id INTEGER PRIMARY KEY, Checked int DEFAULT 0, Title Varchar(500), Desc Varchar(500), Date Text)";
                    command.ExecuteNonQuery();
                    var command2 = connection.CreateCommand();
                    command2.CommandText = "SELECT * FROM todos";
                    command2.ExecuteNonQuery();
                    SQLiteDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine(reader["Title"].GetType());
                        Debug.WriteLine(reader["Title"]);
                        if (reader["Date"] is DBNull)
                        {
                            Debug.WriteLine("is null");
                        }
                        else
                        {
                            Debug.WriteLine(reader["Date"].GetType());
                        }
                        Debug.WriteLine(reader["Date"]);
                        Debug.WriteLine(reader["Checked"]);
                        Debug.WriteLine(reader["Checked"].GetType());
                        this.Todos.Add(new Todos((string)reader["Title"], (int)reader["Checked"], reader["Date"] is DBNull ? string.Empty : (string)reader["Date"]));
                        Debug.WriteLine("");

                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw new Exception("Could not connect to database");
            }
            this.SearchTodo = new ObservableCollection<Todos>(this.Todos);

            this.AddToDo = new AddCommand(this.OnAddToDo);
            this.SearchIt = new AddCommand(this.OnSearch);
        }

        /// <summary>
        /// Gets the add to do.
        /// </summary>
        public ICommand AddToDo { get; private set; }
        public ICommand SearchIt { get; private set; }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        public string Input
        {
            get => this.input;
            set
            {
                this.input = value;
                this.OnPropertyChanged();
            }
        }

        public string Search
        {
            get => this.search;
            set
            {
                this.search = value;
            }
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string Date
        {
            get => date;
            set
            {
                this.date = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the todos.
        /// </summary>
        public ObservableCollection<Todos> Todos { get; set; }

        public ObservableCollection<Todos> SearchTodo { get; set; }
    

        public void OnSearch(object param)
        {
            this.SearchTodo.Clear();
            if (!string.IsNullOrEmpty(this.Search))
            {
                foreach (var t in this.Todos)
                {
                    if (t.Text.Contains(this.Search))
                    {
                        this.SearchTodo.Add(t);
                    }
                }
            }
            else
            {
                foreach (var todose in Todos)
                {
                    SearchTodo.Add(todose);
                }
            }
            this.OnPropertyChanged("Todos");
            this.OnPropertyChanged("SearchTodos");
        }

        /// <summary>
        /// The on add to do.
        /// </summary>
        /// <param name="param">
        /// The param.
        /// </param>
        public void OnAddToDo(object param)
        {
            Debug.WriteLine("Adding a new todo");
            if (this.Input == null || this.input.Trim().Length == 0)
            {
                return;
            }

            using (var connection = new SQLiteConnection("Data Source=todos.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO todos(Title, Date) values (@Title, @Date)";
                command.Parameters.AddWithValue("Title", this.Input);
                Debug.WriteLine("Adding Date {0}", this.Date);
                command.Parameters.AddWithValue("Date", this.Date);

                command.ExecuteNonQuery();
                connection.Close();
            }
            this.Todos.Add(new Todos(this.Input, 0, Date));
            this.Input = string.Empty;
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The add command.
        /// </summary>
        internal class AddCommand : ICommand
        {
            /// <summary>
            /// The execute.
            /// </summary>
            private readonly Action<object> execute;

            /// <summary>
            /// Initializes a new instance of the <see cref="AddCommand"/> class.
            /// </summary>
            /// <param name="execute">
            /// The execute.
            /// </param>
            public AddCommand(Action<object> execute)
            {
                this.execute = execute;
            }

            /// <summary>
            /// The can execute.
            /// </summary>
            /// <param name="parameter">
            /// The parameter.
            /// </param>
            /// <returns>
            /// The <see cref="bool"/>.
            /// </returns>
            public bool CanExecute(object parameter)
            {
               return true;
            }

            /// <summary>
            /// The execute.
            /// </summary>
            /// <param name="parameter">
            /// The parameter.
            /// </param>
            public void Execute(object parameter)
            {
                this.execute(parameter);
            }

            /// <summary>
            /// The can execute changed.
            /// </summary>
            public event EventHandler CanExecuteChanged;
        }
    
        /// <summary>
        /// The property changed param.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
