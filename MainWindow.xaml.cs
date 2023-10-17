using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace WPF_APP_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentInput = string.Empty;
        private string currentOperator = string.Empty;
        private double previousValue = 0;
        private double result = 0;
               
        public MainWindow()
        {
            InitializeComponent();
            CultureInfo cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            this.KeyDown += Button_KeyDown;
        }
        private void HandleNumericInput(string buttonText)
        {
            if (isNewOperation)
            {
                currentInput = buttonText;
                isNewOperation = false;
            }
            else
            {
                currentInput += buttonText;
            }
            inputTextBox.Text = currentInput;
            isEqualClicked = false;
        }

        private void HandleClear()
        {
            currentInput = string.Empty;
            inputTextBox.Text = currentInput;
        }
        private void HandleOperator(string operatorText)
        {
            if (!string.IsNullOrEmpty(currentInput))
            {
                if (!string.IsNullOrEmpty(currentOperator))
                {
                    Calculate();
                }
                historyTextBox.Text += $"{currentInput} {operatorText} ";
                previousValue = double.Parse(currentInput);
                currentInput = string.Empty;
                currentOperator = operatorText;
                inputTextBox.Text = previousValue.ToString();
            }
            isNewOperation = true;
            isEqualClicked = false;
        }
        private void HandleBackspace()
        {
            if (!string.IsNullOrEmpty(currentInput))
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
                inputTextBox.Text = currentInput;
            }
        }
        private bool isNewOperation = true;
        private bool isEqualClicked = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Content.ToString();

            errorTextBlock.Visibility = Visibility.Collapsed;
            errorTextBlock.Text = string.Empty;

            if (isNewOperation && (buttonText == "+" || buttonText == "-" || buttonText == "*" || buttonText == "/"))
            {
                isNewOperation = false;
                currentOperator = buttonText;
                historyTextBox.Text = previousValue + " " + currentOperator;
            }
            switch (buttonText)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case ",":
                    if (isEqualClicked)
                    {
                        HandleClear();
                        isEqualClicked = false;
                    }
                    if (isNewOperation)
                    {
                        currentInput = string.Empty;
                        isNewOperation = false;
                        inputTextBox.Text = string.Empty;
                    }
                    HandleNumericInput(buttonText);
                    break;
                case "CE":
                    HandleClear();
                    break;
                case "C":
                    HandleClear();
                    currentOperator = string.Empty;
                    historyTextBox.Text = string.Empty;
                    break;
                case "<":
                    if (!string.IsNullOrEmpty(currentInput) && !isEqualClicked)
                    {
                        HandleBackspace();
                    }
                    break;
                case "+":
                case "-":
                case "*":
                case "/":
                    isNewOperation = true;
                    if (!string.IsNullOrEmpty(currentInput))
                    {
                        if (!string.IsNullOrEmpty(currentOperator) && !isEqualClicked)
                        {
                            Calculate();
                        }
                        previousValue = double.Parse(currentInput);
                        currentInput = string.Empty;
                        if (!isEqualClicked)
                        {
                            currentOperator = buttonText;
                            historyTextBox.Text = $"{previousValue} {buttonText} ";
                        }
                        inputTextBox.Text = previousValue.ToString();
                    }
                    isEqualClicked = false;
                    break;
                case "=":
                    if (!string.IsNullOrEmpty(currentInput) && !string.IsNullOrEmpty(currentOperator))
                    {
                        Calculate();
                        currentOperator = string.Empty;
                        inputTextBox.Text = previousValue.ToString();
                        isNewOperation = true;
                        isEqualClicked = true;
                    }
                    break;
                case "x":
                    Close();
                    break;
            }
        }

        private void Calculate()
        {
            double currentValue = double.Parse(currentInput);
            switch (currentOperator)
            {
                case "+":
                    previousValue += currentValue;
                    break;
                case "-":
                    previousValue -= currentValue;

                    break;
                case "*":
                    previousValue *= currentValue;

                    break;
                case "/":
                    if (currentValue != 0)
                    {
                        previousValue /= currentValue;

                    }
                    else
                    {
                        errorTextBlock.Visibility = Visibility.Visible;
                        errorTextBlock.Text = "Ділення на нуль заборонено!";
                        currentInput = string.Empty;
                        historyTextBox.Text = string.Empty;
                        return;
                    }
                    break;
            }
            currentInput = string.Empty;
            currentInput = previousValue.ToString();
        }
        private void Button_KeyDown(object sender, KeyEventArgs e) 
        {
            string key = e.Key.ToString();
            if (key.Length == 2 && key[0] == 'D')
            {
                key = key.Substring(1);
            }
            if (Regex.IsMatch(key, @"^\d$"))
            {
                HandleNumericInput(key);
            }
            else
            {
                switch (key)
                {
                    case "OemComma":
                        HandleNumericInput(",");
                        break;
                    case "Add":
                        HandleOperator("+");
                        break;
                    case "Subtract":
                        HandleOperator("-");
                        break;
                    case "Multiply":
                        HandleOperator("*");
                        break;
                    case "Divide":
                        HandleOperator("/");
                        break;
                    case "Return":
                        HandleOperator("=");
                        break;
                    case "Back":
                        HandleBackspace();
                        break;
                    case "Escape":
                        HandleClear();
                        break;
                    case "Delete":
                        HandleClear();
                        currentOperator = string.Empty;
                        historyTextBox.Text = string.Empty;
                        break;
                    case "NumPad0":
                        HandleNumericInput("0");
                        break;
                    case "NumPad1":
                        HandleNumericInput("1");
                        break;
                    case "NumPad2":
                        HandleNumericInput("2");
                        break;
                    case "NumPad3":
                        HandleNumericInput("3");
                        break;
                    case "NumPad4":
                        HandleNumericInput("4");
                        break;
                    case "NumPad5":
                        HandleNumericInput("5");
                        break;
                    case "NumPad6":
                        HandleNumericInput("6");
                        break;
                    case "NumPad7":
                        HandleNumericInput("7");
                        break;
                    case "NumPad8":
                        HandleNumericInput("8");
                        break;
                   case "NumPad9":
                        HandleNumericInput("9");
                        break;
                   case "Decimal":
                        HandleNumericInput(",");
                        break;

                }
            }
        }
        
    }
}
