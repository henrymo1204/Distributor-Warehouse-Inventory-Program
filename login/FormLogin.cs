﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using login.classes;

namespace login
{
    public partial class FormLogin : Form
    {

        SqlConnection sqlcon = null;//sql connection variable

        public FormLogin()
        {
            InitializeComponent();
            Connection open = new Connection();//create a connection object
            this.sqlcon = open.connect();//set sqlcon to the sql connection object returned from the connect function
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();//close login form
        }


        private void Login_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) && string.IsNullOrEmpty(txtPassword.Text))//if user did not input both username and password
            {
                MessageBox.Show("You did not enter username and password!");//show message box
            }
            else if (!string.IsNullOrEmpty(txtUsername.Text) && string.IsNullOrEmpty(txtPassword.Text))//if user did not input password
            {
                MessageBox.Show("You did not enter password!");//show message box
            }
            else if (string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text))//if user did not input username
            {
                MessageBox.Show("You did not enter username!");//show message box
            }
            else//if user did input both username and password
            {
                sqlcon.Open();//open database
                SqlCommand query = new SqlCommand("SELECT loginid, password, usergroup, phonenumber, email, address FROM Login WHERE username = @username;", sqlcon);//query command to look for the correct password based on user input username
                query.Parameters.AddWithValue("username", txtUsername.Text);//set username to look for to user input username
                
                SqlDataReader read = query.ExecuteReader();//execute query and store values to data reader
                
                try
                {
                    string id = "";
                    string pw = "";
                    string group = "";
                    string phone = "";
                    string email = "";
                    string address = "";
                    while (read.Read())
                    {
                        id = read.GetString(0);
                        pw = read.GetString(1); //set output to value output from executing the query
                        group = read.GetString(2); //get user group
                        if(group == "Client")
                        {
                            phone = read.GetString(3);
                            email = read.GetString(4);
                            address = read.GetString(5);
                        }
                    }

                    if (txtPassword.Text == pw)//compare user input password to the password in database
                    {//if they are the same
                        User user = new User();
                        user.UserID = id;
                        user.Password = pw;
                        user.Group = group; //keep track of user group
                        if(group == "Client")
                        {
                            user.PhoneNumber = phone;
                            user.EMail = email;
                            user.Address = address;
                        }

                        if(user.Group == "Admin" || user.Group == "Clerk" || user.Group == "Stocker")
                        {
                            FormMain form = new FormMain(user);//create main form object
                            this.Hide();//hide current form
                            form.Show();//show main form
                        }
                        else if(user.Group == "Client")
                        {
                            FormOrder form = new FormOrder(user);//create main form object
                            this.Hide();//hide current form
                            form.Show();//show main form
                        }
                    }
                    else
                    {//if they are not the same
                        MessageBox.Show("Invalid password!");//shows a message box that tells the user "Invalid Username and Password!"
                    }
                    sqlcon.Close();//close database
                }
                catch
                {
                    MessageBox.Show("Username not found.");
                    sqlcon.Close();//close database
                }
            }
        }
    }
}
