using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tasks_Tracker_App
{
    /// <summary>
    /// Task's data class
    /// </summary>
    public struct Task
    {
        public string Title { get; set; }
        public string Ticket { get; set; }
        public string Type { get; set; }
        public bool Resolved { get; set; }
    }

        /// <summary>
        /// Interaction logic MainWindow.xaml
        /// </summary>
        public partial class MainWindow : Window
    {
        public List<Task> Tasks { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Height = MinHeight;

            invertStartStopBtn(false);
            tasksTableButtons.IsEnabled = false;
        }
        
        private void resolvedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ifIsBilled();
        }
        
        private void startTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            startTask();
        }
        
        private void stopTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            stopTask();
            saveTaskToList();
        }
        
        private void saveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            saveTaskToList();
        }
        
        private void tasksTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tasksTable.SelectedIndex > -1)
            {
                tasksTableButtons.IsEnabled = true;
                var selection = (Task)tasksTable.SelectedItem;
                if (selection.Resolved)
                    markResolvedButton.Content = "Mark Unresolved";
                else
                    markResolvedButton.Content = "Mark Resolved";
            }
            else
                tasksTableButtons.IsEnabled = false;
        }
        
        private void startSelectedTaskButton_Click(object sender, RoutedEventArgs e)
        {
            uploadTaskFromList();
            startTask();
        }
        
        private void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            uploadTaskFromList();
        }
        
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            updateTaskInList();
        }
        
        private void markResolvedButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tasksTable.SelectedItem != null)
                {
                    if (tasksTable.SelectedItem is Task)
                    {
                        var selection = (Task)tasksTable.SelectedItem;

                        if (selection.Resolved == false)
                            selection.Resolved = true;
                        else
                            selection.Resolved = false;

                        tasksTable.Items.Insert(tasksTable.SelectedIndex, selection);

                        tasksTable.SelectedIndex -= 1;
                        ifIsBilled(true);
                        tasksTable.SelectedIndex += 1;

                        tasksTable.Items.RemoveAt(tasksTable.SelectedIndex);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteTaskInList();
        }
        
        private void showLogsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Height == MinHeight)
            {
                Height = MaxHeight;
                showLogsBtn.Content = "Hide Logs";
            }
            else
            {
                Height = MinHeight;
                showLogsBtn.Content = "Show Logs";
            }
        }
        
        private void editLogsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (logsGrid.IsEnabled)
            {
                logsGrid.IsEnabled = false;
                editLogsBtn.Content = "Edit";
            }
            else
            {
                logsGrid.IsEnabled = true;
                editLogsBtn.Content = "Save";
            }
        }

        //Disables and enables Start or Stop Task buttons
        public void invertStartStopBtn(bool taskStarted = true)
        {
            if (startTaskBtn.IsEnabled && taskStarted)
            {
                startTaskBtn.IsEnabled = false;
                startSelectedTaskButton.IsEnabled = false;
                stopTaskBtn.IsEnabled = true;
            }
            else
            {
                stopTaskBtn.IsEnabled = false;
                startTaskBtn.IsEnabled = true;
                startSelectedTaskButton.IsEnabled = true;
            }
        }

        //Adds title to task if it's empty
        public void notEmptyTaskTitle()
        {
            if (String.IsNullOrWhiteSpace(taskField.Text))
                taskField.Text = "New Task";
        }

        //method to start the task
        public void startTask()
        {
            notEmptyTaskTitle();


            ifIsBilled();

            invertStartStopBtn();
        }

        //method to stop the task
        public void stopTask()
        {
            notEmptyTaskTitle();


            ifIsBilled();

            invertStartStopBtn();
        }

        //Saves current fields' data to table
        public void saveTaskToList()
        {
            notEmptyTaskTitle();

            ifIsBilled();

            addNewTaskToList(taskField.Text, ticketField.Text, typeBox.Text, resolvedCheckBox.IsChecked.Value);
        }

        //Adds new task row to the table
        public void addNewTaskToList(string title = "New Task", string ticket = null, string type = "Undefined", bool resolved = false)
        {
            tasksTable.Items.Add(new Task { Title = title, Ticket = ticket, Type = type, Resolved = resolved });
        }

        //Replaces fields' data with selected table's task
        public void uploadTaskFromList()
        {
            try
            {
                if (tasksTable.SelectedItem != null)
                {
                    if (tasksTable.SelectedItem is Task)
                    {
                        var selection = (Task)tasksTable.SelectedItem;

                        taskField.Text = selection.Title;
                        ticketField.Text = selection.Ticket;
                        typeBox.Text = selection.Type;
                        resolvedCheckBox.IsChecked = selection.Resolved;

                        tasksTable.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //Replaces selected task with fields' data
        public void updateTaskInList()
        {
            insertTaskInList();
            deleteTaskInList();
        }

        //Inserts task to current location in the table
        public void insertTaskInList()
        {
            try
            {
                if (tasksTable.SelectedItem != null)
                {
                    if (tasksTable.SelectedItem is Task)
                    {
                        var selection = (Task)tasksTable.SelectedItem;

                        notEmptyTaskTitle();
                        selection.Title = taskField.Text;
                        selection.Ticket = ticketField.Text;
                        selection.Type = typeBox.Text;
                        selection.Resolved = resolvedCheckBox.IsChecked.Value;

                        tasksTable.Items.Insert(tasksTable.SelectedIndex, selection);

                        tasksTable.SelectedIndex -= 1;
                        ifIsBilled(true);
                        tasksTable.SelectedIndex += 1;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //Deletes task row from the table
        public void deleteTaskInList()
        {
            try
            {
                if (tasksTable.SelectedItem != null)
                {
                    if (tasksTable.SelectedItem is Task)
                    {
                        tasksTable.Items.RemoveAt(tasksTable.SelectedIndex);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //Writes line to the log field
        public void writeToLog(string textToLog)
        {
            logsText.Document.Blocks.Add(new Paragraph(new Run(" >>>>> " + textToLog + " <<<<< ")));
        }

        //checks if the task ticket should be added to the list of Billed/Not Billed
        public void ifIsBilled(bool fromTable = false)
        {
            if (!fromTable && resolvedCheckBox.IsChecked.Value 
                && !string.IsNullOrWhiteSpace(ticketField.Text))
            {
                switch (typeBox.Text)
                {
                    case "Billed":
                        addToBilled(!billedTextBox.Text.Contains(ticketField.Text)
                            ? ticketField.Text : null); break;

                    case "Not Billed":
                        addToNotBilled(!notBilledTextBox.Text.Contains(ticketField.Text)
                            ? ticketField.Text : null); break;

                    default: break;
                }
            }

            else if (fromTable && tasksTable.SelectedIndex > -1)
            {
                var selection = (Task)tasksTable.SelectedItem;
                if (selection.Resolved && !string.IsNullOrWhiteSpace(selection.Ticket))
                {
                    switch (selection.Type)
                    {
                        case "Billed":
                            addToBilled(!billedTextBox.Text.Contains(selection.Ticket)
                                ? selection.Ticket : null); break;

                        case "Not Billed":
                            addToNotBilled(!notBilledTextBox.Text.Contains(selection.Ticket) 
                                ? selection.Ticket : null); break;

                        default: break;
                    }
                    
                }
            }

            else
            {
                
            }
        }

        //adds ticket to Billed field
        public void addToBilled(string ticket)
        {
            if (!String.IsNullOrWhiteSpace(ticket))
            {
                billedTextBox.Text += ticket + ", ";
                writeToLog("Ticket #" + ticket + " was added to Billed");
            }
        }

        //adds ticket to Not Billed field
        public void addToNotBilled(string ticket)
        {
            if (!String.IsNullOrWhiteSpace(ticket))
            {
                notBilledTextBox.Text += ticket + ", ";
                writeToLog("Ticket #" + ticket + " was added to Not Billed");
            }
        }

        
    }
}
