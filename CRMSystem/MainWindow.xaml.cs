using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;

namespace CRMSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Employees employees1;
        private DispatcherTimer dispatcher;
        private int counter = 10;
        public MainWindow()
        {
            InitializeComponent();
            DB.tbe = new Entities();
            dispatcher = new DispatcherTimer();
            dispatcher.Interval = new TimeSpan(0, 0, 1);
            dispatcher.Tick += new EventHandler(TimerEnd);
            btnEntry.Visibility = Visibility.Collapsed;
            stackPassword.Visibility = Visibility.Collapsed;


        }
        private void TimerEnd(object sender, EventArgs e)
        {
            try
            {
                if (counter != 0)
                {
                    tbNewCode.Text = "Новый код доступен через \n\t" + string.Format("00:0{0}:{1}", counter / 60, counter % 60) + " секунд ";


                }
                else
                {

                    dispatcher.Stop();
                    
                    tbNewCode.Text = "Код не действителен. Запросите повторную отправку кода";
                  
                }
                counter--;

            }
            catch
            {
                MessageBox.Show("Дваайте еще раз попробуем");
            }



        }
        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void btnEntry_Click(object sender, RoutedEventArgs e)
        {
            // List<Employees> list = new List<Employees>();


            //List<Employees> employees = DB.tbe.Employees.ToList();
            //if (employees. == tbNumber.Text)
            ////{
            //    tbCode.Visibility = Visibility.Visible;

            //}
            //else
            //{
            //    MessageBox.Show("нет");
            //}
        }

        private void tbNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                List<Employees> list1 = DB.tbe.Employees.Where(x => x.Number == tbNumber.Text).ToList();

                if (!string.IsNullOrEmpty(tbNumber.Text))
                {
                    if (list1.Count == 1)
                    {
                        MessageBox.Show("Номер в базе данных существует");
                        btnEntry.Visibility = Visibility.Visible;
                        stackPassword.Visibility = Visibility.Visible;
                        tbPassword.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Номер отсутствует в базе данных");
                    }
                }
                else
                {
                    MessageBox.Show("Поле номера не заполнено!");
                }

            }
        }
        private void checkedPassword()
        {
            if (Regex.IsMatch(tbPassword.Text, @"(?=.[0-9]){1,}"))
            {

            }
            else
            {
                MessageBox.Show("В пароле должна быть хотя бы одна цифра");
            }
        }
        public static int countOutput = 0;
        public static string successfullyCode = "";
        void generateCode()
        {
            List<Employees> list1 = DB.tbe.Employees.Where(x => x.Number == tbNumber.Text && x.Password == tbPassword.Text).ToList();
            if (list1.Count == 1)
            {
                while (true)
                {
                    Random random = new Random();
                    for (int i = 0; i < 8; i++)
                    {
                        int j = random.Next(4); // Выбор 0 - число; 1, 2 - буква; 2 - спецсимвол
                        if (j == 0)
                        {
                            successfullyCode +=  random.Next(9).ToString();
                        }
                        else if (j == 1 || j == 2)
                        {
                            int l = random.Next(2); // Выбор 0 - заглавная; 1 - маленькая буква
                            if (l == 0)
                            {
                                successfullyCode += (char)random.Next('A', 'Z' + 1);
                            }
                            else
                            {
                                successfullyCode += (char)random.Next('a', 'z' + 1);
                            }
                        }
                        else
                        {
                            int l = random.Next(4); // Выбор диапозона
                            if (l == 0)
                            {
                                successfullyCode += (char)random.Next(33, 48);
                            }
                            else if (l == 1)
                            {
                                successfullyCode += (char)random.Next(58, 65);
                            }
                            else if (l == 2)
                            {
                                successfullyCode += (char)random.Next(91, 97);
                            }
                            else if (l == 3)
                            {
                                successfullyCode += (char)random.Next(123, 127);
                            }
                        }
                    }

                    //if (regex.IsMatch(successfullyCode)) ;
                    //{
                    //    break;
                    //}
                }
                MessageBox.Show("Код для доступа " + successfullyCode + "\nУ вас будет дано 10 секунд, чтобы ввести код");

               
              
              
                counter = 10;
                dispatcher.Start();
            }
                

        }
        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                if (!string.IsNullOrEmpty(tbPassword.Text))
                {
                    tbCode.Visibility = Visibility.Visible;
                    tbCode.Focus();
                    tbCode.Text = "";
                    generateCode();
                }
            }
        }
    }
    
}
