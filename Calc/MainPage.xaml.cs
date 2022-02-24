using System;
using System.Linq;
using System.Data;
using Xamarin.Forms;

namespace Calc
{
    public partial class MainPage : ContentPage
    {
        string result = "";
        char lastChar;

        public MainPage()
        {
            InitializeComponent();
        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button b = sender as Button;

                Console.WriteLine(">" + b.Text + "<");
                if (isNumeric(b.Text))
                {
                    if(lastChar == '\n' || lastChar == 'r')
                    {
                        result = "";
                    }
                    result += b.Text;
                    lastChar = b.Text.Last();
                    printResult(result);
                }
                else if(isExpr(b.Text))
                {
                    if (lastChar == 'r') result = "";

                    if(isExpr(lastChar.ToString()) && result.Length != 0)
                    {
                        result = result.Substring(0, result.Length - 1);
                        result += b.Text;
                    }
                    else
                    {
                        result += b.Text;
                    }
                    lastChar = b.Text.Last();
                    printResult(result);
                }
                else if(b.Text == ".")
                {
                    lastChar = '.';
                    result += ".";
                    printResult(result);

                }
                else if(b.Text == "C")
                {
                    result = "";
                    lastChar = '\n';
                    printResult(result);
                }
                else if(b.Text == "<-")
                {
                    if(result.Length > 0)
                    {
                        result = result.Substring(0, result.Length - 1);
                        if (lastChar != 'r' && result.Length > 0)
                        {
                            lastChar = result.Last();
                        }
                    }
                    
                    printResult(result);
                }
                else if(b.Text == "=")
                {
                    Console.WriteLine("<<<<<<<<<<<<<<< evaluation");
                    evaluate();
                }

                if(tryEvaluate() != "Error" && lastChar != '\n')
                {
                    QuickRsult.Text = tryEvaluate();
                }
                else if(lastChar == '\n')
                {
                    QuickRsult.Text = "";
                }

                //printResult(result);
            }

            Console.WriteLine(sender.GetType());
        }

        private string tryEvaluate()
        {
            DataTable dt = new DataTable();
            try
            {
                var res = dt.Compute(result, "");
                return res.ToString().Replace(",", ".");
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        private void evaluate()
        {
            DataTable dt = new DataTable();
            try
            {
                var res = dt.Compute(result, "");
                printResult(res.ToString().Replace(",","."));
                lastChar = '\n';
                //if (res is decimal)
                //{
                //    printResult(res.ToString());
                //}
                //else
                //{
                //    printResult("Error");
                //}
            }
            catch(Exception)
            {
                printResult("Error");
                lastChar = 'r';
                return;
            }
        }

        private void printResult(string str)
        {
            Console.WriteLine("writing >" + str + "<");
            result = str;
            Result.Text = str;
        }

        private bool isValid(string str)
        {
            if (isNumeric(str)) return true;
            if (isExpr(str)) return true;
            if (str == ",") return true;

            return false;
        }

        private bool isNumeric(string str)
        {
            string numbers = "0123456789";
            for(int i=0; i<str.Length; i++)
            {
                if(!numbers.Contains(str[i].ToString())) {
                    return false;
                }
            }

            Console.WriteLine(">>>>>>>>>>> is numeric");
            return true;
        }

        private bool isExpr(string str)
        {
            if (str.Length != 1) return false;
            string numbers = "+-*/";
            for (int i = 0; i < str.Length; i++)
            {
                if (!numbers.Contains(str[i].ToString()))
                {
                    return false;
                }
            }

            Console.WriteLine(">>>>>>>>>>> is expr");
            return true;
        }
    }
}
