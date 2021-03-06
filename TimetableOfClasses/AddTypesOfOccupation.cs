﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibOfTimetableOfClasses;

namespace TimetableOfClasses
{
    public partial class AddTypesOfOccupation : Form
    {
        private MTypesOfOccupations type;

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool isEmpty(string[] strArgs)
        {
            foreach (var cur in strArgs)
            {
                if (cur.Length == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void message()
        {
            MessageBox.Show("Заполните все пустые строки", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public AddTypesOfOccupation()
        {
            InitializeComponent();
            tbFullName.Text = "Лекция";
            tbShortName.Text = "ЛК";
        }

        public AddTypesOfOccupation(MTypesOfOccupations mTypesOfOccupations)
        {
            InitializeComponent();

            tbFullName.Text = mTypesOfOccupations.FullName;
            tbFullName.Enabled = false;

            tbShortName.Text = mTypesOfOccupations.ShortName;


            this.Text = "Изменение группы";
            type = mTypesOfOccupations;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            string[] args = new string[] { tbFullName.Text, tbShortName.Text };
           
            if (!isEmpty(args))
            {
                if (Add())
                {
                    Close();
                }
            }
            else message();
        }

        private bool Add()
        {
            string errors = "";
            ushort fullname, shortname;
            if (ushort.TryParse(tbFullName.Text, out fullname) && fullname > 0)
                if (ushort.TryParse(tbShortName.Text, out shortname) && shortname <= fullname && shortname > 0)

                {
                    if (type == null)
                    {
                        MTypesOfOccupations Type = new MTypesOfOccupations(tbFullName.Text, tbShortName.Text);
                        if (Controllers.CTypesOfOccupations.Insert(Type))
                            return true;
                        else errors = "Невозможно добавить этот вид занятия";
                    }
                    else
                    {
                        type.FullName = tbFullName.Text;
                        type.ShortName = tbShortName.Text;
                        if (Controllers.CTypesOfOccupations.Update(type))
                            return true;
                        else errors = "Невозможно так изменить этот вид занятия";
                    }
                }
                else errors = "Введите правильный полный вид занятия!";
            else errors = "Введите правильный краткий вид занятия!";
            if (errors != "") MessageBox.Show(errors, "Попробуйте еще раз");
            return false;
        }



        private void KeyPress1(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != '\b' && l != '-' && (l < '0' || l > '9'))
            {
                e.Handled = true;
            }
        }

        private void KeyPress2(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != '\b' && l != ' ' && l != '-')
            {
                e.Handled = true;
            }
        }

        private void KeyPress3(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != '\b' && l != ' ' && l != ',')
            {
                e.Handled = true;
            }
        }

        private void KeyPress4(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < '0' || l > '9') && l != '\b')
            {
                e.Handled = true;
            }
        }

        private void SelectionOfLetters1(object sender, EventArgs e)
        {
            string pattern = @"^[0-9]{2}-[А-Я]{1,2}[а-я]{2}-[0-9]{1}[а-я]{0,1}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

            TextBox R = sender as TextBox;


            if (!regex.IsMatch(R.Text))
            { R.Text = "00-ААаа-0а"; }
            else
            { }


        }

        private void SelectionOfLetters2(object sender, EventArgs e)
        {
            TextBox R = sender as TextBox;
            R.Text = Regex.Replace(R.Text, "[^а-яА-Я-, ]", "");

            while (R.Text.IndexOf("-") == 0)
                R.Text = R.Text.Substring(1);
            while (R.Text.IndexOf(" ") == 0)
                R.Text = R.Text.Substring(1);
            while (R.Text.IndexOf(",") == 0)
                R.Text = R.Text.Substring(1);
            if (R.Text.Length != 0)
            {
                while (R.Text.LastIndexOf(" ") == R.Text.Length - 1 && R.Text.Length != 0)
                    R.Text = R.Text.Remove(R.Text.Length - 1);
                while (R.Text.LastIndexOf("-") == R.Text.Length - 1 && R.Text.Length != 0)
                    R.Text = R.Text.Remove(R.Text.Length - 1);
                while (R.Text.LastIndexOf(",") == R.Text.Length - 1 && R.Text.Length != 0)
                    R.Text = R.Text.Remove(R.Text.Length - 1);
            }
            R.Text = R.Text.ToLower();
            R.Text = FirstLetterToUpper(R.Text);

        }

        private static string FirstLetterToUpper(string str)
        {
            if (str.Length > 0)
            {
                int index = 0;
                string text = Char.ToUpper(str[0]) + str.Substring(1);
                while (str.IndexOf(",", index + 1) > index)
                {
                    index = str.IndexOf(",", index + 1);
                    for (int i = index + 1; i < text.Length; i++)
                    {
                        if (text[i] >= 'А' && text[i] <= 'я')
                        {
                            text = text.Substring(0, i) + Char.ToUpper(text[i]) + text.Substring(i + 1);
                            break;
                        }
                    }
                }
                return text;
            }
            return "";
        }

        private void fieldChanged(object sender, EventArgs e)
        {
            TextBox R = sender as TextBox;
            if (R.TextLength == 0)
                R.BackColor = Color.Red;
            else
                R.BackColor = Color.White;
        }
    }
}
