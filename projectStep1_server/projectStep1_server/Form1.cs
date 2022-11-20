using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projectStep1_server
{
    public partial class Form1 : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>();
        List<String> names = new List<String>();
        String[] questions;
        Dictionary<string, double> scores = new Dictionary<string, double>();
        Dictionary<string, double> answers = new Dictionary<string, double>();
        int playerCount = 0;
        int namesReceived = 0;
        int answersReceived = 0;
        bool gameStarted = false;

        bool terminating = false;
        bool listening = false;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int serverPort;

            if (Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3);

                listening = true;
                button_start.Enabled = false;
                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");
                int qnum;
                Int32.TryParse(textBox_qnum.Text, out qnum);

                getQuestions();
                //sendQuestion(0);
                int i = 2;
                //while (i <= qnum*2)
                //{
                //    if ( answersReceived == playerCount)
                //    {
                        
                //    }
                //}
            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }
        private void getQuestions()
        {
            questions = System.IO.File.ReadAllLines(@"questions.txt");
        }
        private void sendQuestion(int qnum)
        {
            Byte[] buffer_question = Encoding.Default.GetBytes(questions[qnum]);
            foreach (Socket client in clientSockets)
            {
                try
                {
                    client.Send(buffer_question);
                }
                catch
                {
                    logs.AppendText("There is a problem! Check the connection...\n");
                    terminating = true;
                    serverSocket.Close();
                }

            }
        }
        private void Accept()
        {
            while (listening)
            {
                try
                {
                    Socket newClient = serverSocket.Accept();
                    playerCount++;
                    Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                    receiveThread.Start();

                    //Byte[] buffer = new Byte[64];
                    //newClient.Receive(buffer);

                    //string incomingMessage = Encoding.Default.GetString(buffer);
                    //incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    //logs.AppendText("Client: " + incomingMessage + "\n");

                    //logs.AppendText("A client is connected.\n");
                    //if(names.Count == 0 || !names.Contains(incomingMessage))
                    //{
                    //    names.Add(incomingMessage);
                    //    Byte[] buffer_unique = Encoding.Default.GetBytes("ok");

                    //    newClient.Send(buffer_unique);
                    //    Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                    //    receiveThread.Start();
                    //    clientSockets.Add(newClient);
                    //}
                    //else
                    //{

                    //    Byte[] buffer_unique = Encoding.Default.GetBytes("The names should be unique");

                    //    newClient.Send(buffer_unique);
                    //    newClient.Close();

                    //}


                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }
        private void Receive(Socket thisClient) // updated
        {
            bool connected = true;
            string name = "";

            while (connected && !terminating)
            {
                try
                {
                    if (name == "")
                    {
                        Byte[] buffer_name = new Byte[64];
                        thisClient.Receive(buffer_name);

                        name = Encoding.Default.GetString(buffer_name);
                        name = name.Substring(0, name.IndexOf("\0"));
                        if (names.Count == 0 || !names.Contains(name))
                        {
                            names.Add(name);
                            Byte[] buffer_unique = Encoding.Default.GetBytes("ok");
                            thisClient.Send(buffer_unique);
                            clientSockets.Add(thisClient);
                            logs.AppendText(name +" has connected"+"\n");
                        }
                        else
                        {
                            Byte[] buffer_unique = Encoding.Default.GetBytes("The name should be unique");
                            thisClient.Send(buffer_unique);
                            thisClient.Close();
                            logs.AppendText("Player is refused due to repeating name" + "\n");
                        }

                        
                    }
                    else
                    { 
                        Byte[] buffer = new Byte[64];
                        thisClient.Receive(buffer);

                        string incomingMessage = Encoding.Default.GetString(buffer);
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                        int answer;
                        Int32.TryParse(incomingMessage, out answer);
                        answers.Add(name, answer);
                        logs.AppendText(name + " gave " +incomingMessage + " as an answer" + "\n");
                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("A client has disconnected\n");
                    }
                    thisClient.Close();
                    clientSockets.Remove(thisClient);
                    connected = false;
                }
            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
