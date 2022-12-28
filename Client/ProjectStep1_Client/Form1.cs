using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ProjectStep1_Client
{
    public partial class Form1 : Form
    {
        //initialization
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }



        // Connect button handler

        private void button1_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;  // ip of the server field
            string name = textBox_name.Text; // port
            int portNum;
            if (Int32.TryParse(textBox_port.Text, out portNum))
            {
                if (name != "" && name.Length <= 64)  // if name field is given.
                {
                    try
                    {

                        clientSocket.Connect(IP, portNum);   // Connection
                        string message = textBox_name.Text;
                        Byte[] buffer = Encoding.Default.GetBytes(name);
                        clientSocket.Send(buffer);  // send the sign up request.
                        Byte[] buffer5 = new Byte[64];
                        clientSocket.Receive(buffer5);  // get the response message.

                        string incomingMessage = Encoding.Default.GetString(buffer5);
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                        if (incomingMessage != "ok")   // if forr some reason user is not able to join the game.
                        {
                            logs.AppendText("Server: " + incomingMessage + "\n");
                            clientSocket.Close();
                            connected = false;
                            button_connect.Enabled = true;
                            textBox_answer.Visible = false;
                            button_submit.Visible = false;
                            button_disconnect.Enabled = false;
                            label_answer.Visible = false;
                            button_connect.BackColor = Color.White;
                        }
                        else // succeed.
                        {
                            button_connect.Enabled = false;
                            button_disconnect.Enabled = true;
                            button_connect.BackColor = Color.Green;
                            textBox_answer.Visible = true;
                            button_submit.Visible = true;
                            label_answer.Visible = true;
                            connected = true;
                            logs.AppendText("Connected to the server!\n");
                            Thread receiveThread = new Thread(Receive);
                            receiveThread.Start();
                        }
                    }
                    catch
                    {
                        logs.AppendText("Could not connect to the server!\n");
                    }

                }
                else
                {
                    logs.AppendText("You can't leave the name box blank \n");
                }

            }
            else
            {
                logs.AppendText("Check the port\n");
            }
        }


        // Receive thread.
        private void Receive()
        {
            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[1064];
                    clientSocket.Receive(buffer);  // keep receiving.

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    logs.AppendText("Server: " + incomingMessage + "\n");


                    if (incomingMessage != "Sorry, the game has already started. You will participate in the next game.\n")
                    {
                        button_submit.Enabled = true;
                    }
                    else
                    {
                        button_submit.Enabled = false;
                    }



                }
                catch
                {
                    // if the connection is being closed.
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected\n");
                        logs.AppendText("You can try to re-connect later...\n");
                        button_connect.Enabled = true;
                        textBox_answer.Visible = false;
                        button_submit.Visible = false;
                        label_answer.Visible = false;
                        connected = false;
                        button_disconnect.Enabled = false;
                        button_connect.BackColor = Color.White;
                    }

                    clientSocket.Close();
                    connected = false;
                }

            }
        }
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void button_submit_Click(object sender, EventArgs e)
        {
            button_submit.Enabled = false;
            string message = textBox_answer.Text;

            if (message != "" && message.Length <= 64)
            {
                Byte[] buffer = Encoding.Default.GetBytes(message);
                clientSocket.Send(buffer);  // send the answer of the question.
            }
        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            // disconnecting from the server (game)
            // close the socket

            clientSocket.Close();
            connected = false;
            button_connect.Enabled = true;
            textBox_answer.Visible = false;
            button_submit.Visible = false;
            button_disconnect.Enabled = false;
            label_answer.Visible = false;
            button_connect.BackColor = Color.White;
        }

        private void textBox_port_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_ip_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
