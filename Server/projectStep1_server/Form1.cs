using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace projectStep1_server
{
    public partial class Form1 : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>();
        List<Socket> playerSockets = new List<Socket>();
        List<String> names = new List<String>();
        String[] questions;
        List<String> realQuestions = new List<String>();
        List<double> realAnswers = new List<double>();
        Dictionary<string, double> scores = new Dictionary<string, double>();
        Dictionary<string, double> scoresOfPlayers = new Dictionary<string, double>();
        Dictionary<string, double> answers = new Dictionary<string, double>();
        int playerCount = 0;
        int playersInGame = 0;
        int answersReceived = 0;
        List<string> winnerNames = new List<string>();
        bool finishGame = false;
        bool gameStarted = false;
        bool terminating = false;
        bool listening = false;
        bool startGame = false;
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
            button2.Visible = true;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSockets = new List<Socket>();
            names = new List<String>();







            scores = new Dictionary<string, double>();
            scoresOfPlayers = new Dictionary<string, double>();
            answers = new Dictionary<string, double>();
            playerCount = 0;
            playersInGame = 0;
            answersReceived = 0;
            winnerNames = new List<string>();
            finishGame = true;
            gameStarted = false;
            terminating = false;
            listening = false;

            int serverPort;
            if (Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3);
                listening = true;


                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");
            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }

        private void playGame()
        {
            while (!terminating && listening)
            {

                if (playersInGame >= 2 && startGame && finishGame)
                {
                    finishGame = false;
                    sendQuestion(0, "\nGAME STARTS....\n");

                    gameStarted = true;
                    int qno = 1;
                    logs.AppendText("\nGAME STARTS....\n");
                    string anc = "";
                    while (qno <= realQuestions.Count && !finishGame)
                    {
                        //logs.AppendText(playersInGame + " " + answersReceived + " \n");
                        if (answersReceived == playersInGame && !finishGame)
                        {

                            calculateRoundScore(qno - 1, ref anc);


                            if (qno != realQuestions.Count && !finishGame)
                            {
                                answersReceived = 0;
                                sendQuestion(qno, anc);
                            }
                            qno++;

                        }





                    }


                    foreach (Socket client in playerSockets)
                    {
                        string endMsg = "";
                        if (!finishGame)
                        {
                            endMsg = anc;
                        }
                        endMsg += "The game is finished!\n";
                        endMsg += "Here is the FINAL score table!\n\n";
                        endMsg += "-------------------------\n";
                        var a = scoresOfPlayers.OrderByDescending(key => key.Value);
                        foreach (var item in a)
                        {
                            //logs.AppendText("Username: " + item.Key + "--> Score: " + item.Value + "\n");
                            endMsg += "Username: " + item.Key + "--> Score: " + item.Value + "\n";
                            endMsg += "-------------------------\n";
                        }
                        endMsg += "\n\n";
                        endMsg += "---------------- THE WINNER(s) ----------------\n";
                        foreach (string name in winnerNames)
                        {
                            endMsg += name + "\n";
                        }


                        Byte[] bufferInformEnd = Encoding.Default.GetBytes(endMsg);
                        client.Send(bufferInformEnd);
                        Thread.Sleep(1000);

                    }

                    //foreach (Socket client in clientSockets)
                    //{
                    //    client.Close();
                    //}
                    Thread.Sleep(1000);
                    button2.Enabled = true;
                    startGame = false;
                    finishGame = true;
                    playerSockets = new List<Socket>(clientSockets);
                    playersInGame = playerCount;

                    scoresOfPlayers = new Dictionary<string, double>(scores);
                    answersReceived = 0;
                    winnerNames.Clear();
                    answers.Clear();
                    //terminating = true;
                    //listening = false;
                    //serverSocket.Close();
                    //button1.Enabled = true;
                    Thread.CurrentThread.Abort();

                    //break;

                }


            }

        }

        private void calculateRoundScore(int anum, ref string announcement) //TODO: multiplayer scoring
        {
            string sendMessage = "";
            double rightAns = realAnswers[anum];

            double mindiff = double.MaxValue;
            List<string> minplayer = new List<string>();
            Thread.Sleep(1000);


            foreach (string keys in answers.Keys)
            {

                if (Math.Abs(answers[keys] - rightAns) == mindiff)
                {
                    minplayer.Add(keys);
                }
                else if (Math.Abs(answers[keys] - rightAns) < mindiff)
                {
                    minplayer = new List<string>();
                    mindiff = Math.Abs(answers[keys] - rightAns);
                    minplayer.Add(keys);
                }
            }
            sendMessage += "\n------- Received all anwers -------\n";
            sendMessage += "Correct answer of the question is -----> " + rightAns;
            sendMessage += "\nGiven answers:\n";

            foreach (var pair in answers)
            {

                sendMessage += "Username: " + pair.Key;
                sendMessage += "    |    ";
                sendMessage += "Answer: " + pair.Value;
                sendMessage += " -------> Difference: " + Math.Abs(pair.Value - rightAns);
                sendMessage += "\n";


            }



            if (minplayer.Count >= 2)
            {
                double pointsEach = Math.Round((double)1 / (double)minplayer.Count, 2);
                foreach (string name in minplayer)
                {
                    scoresOfPlayers[name] += pointsEach;
                }
                logs.AppendText("\n\n---------- There is a tie! ----------\n\n");
                sendMessage += "\n\n---------- There is a tie! ----------\n\n";
                foreach (string name in minplayer)
                {
                    string message = "Username: " + name + " got +" + pointsEach + " pts\n";
                    sendMessage += message;
                    logs.AppendText(message);
                }
                logs.AppendText("\n\n");
                sendMessage += ("\n\n");
            }
            else
            {
                scoresOfPlayers[minplayer[0]] += 1.0;
                string message = "\n\n" + minplayer[0] + " is the winner! He/she got +1 pts!\n\n";
                sendMessage += message;
                logs.AppendText(message);
            }


            if (anum < realAnswers.Count - 1)
            {
                logs.AppendText("-*-*-*-*-*-*-*-*-*- | CUMULATIVE SCORES | -*-*-*-*-*-*-*-*-*-\n");
                sendMessage += "-*-*-*-*-*-*-*-*-*- | CUMULATIVE SCORES | -*-*-*-*-*-*-*-*-*-\n";
                logs.AppendText("_______________________________________________\n");
                sendMessage += "_______________________________________________\n";


                var a = scoresOfPlayers.OrderByDescending(key => key.Value);
                foreach (var item in a)
                {
                    logs.AppendText("Username: " + item.Key + "--> Score: " + item.Value + "\n");
                    sendMessage += "Username: " + item.Key + "--> Score: " + item.Value + "\n";
                }
                logs.AppendText("_______________________________________________\n");
                sendMessage += "_______________________________________________\n";

            }
            else  // if we are in the last question
            {

                var a = scoresOfPlayers.OrderByDescending(key => key.Value);


                double winnerScore = a.ElementAt(0).Value;
                foreach (var item in a)
                {
                    if (item.Value == winnerScore)
                    {
                        winnerNames.Add(item.Key);
                    }
                }
            }
            announcement = sendMessage;
        }



        private void getQuestions(int qnum)

        {

            realAnswers = new List<double>();
            realQuestions = new List<String>();
            questions = System.IO.File.ReadAllLines(@"../../questions.txt");


            int questionsAdded = 0;
            for (int i = 0; i < questions.Length; i++)
            {
                if (questionsAdded < qnum)
                {
                    if (i == questions.Length - 1)
                    {
                        int line3;
                        Int32.TryParse(questions[i], out line3);
                        realAnswers.Add(line3);
                        logs.AppendText("Answer:" + questions[i] + "\n");
                        questionsAdded++;
                        i = 0;
                        if (questionsAdded == qnum)
                        {
                            break;
                        }
                    }
                    if (i % 2 == 0)
                    {
                        logs.AppendText("Question:" + questions[i] + "\n");
                        realQuestions.Add(questions[i]);

                    }
                    else
                    {
                        int line2;
                        Int32.TryParse(questions[i], out line2);
                        realAnswers.Add(line2);
                        logs.AppendText("Answer:" + questions[i] + "\n");
                        questionsAdded++;
                    }
                }
            }




        }
        private void sendQuestion(int qnum, string anc)
        {
            Byte[] buffer_question = Encoding.Default.GetBytes(realQuestions[qnum]);
            Byte[] buffer_anc = Encoding.Default.GetBytes(anc);
            foreach (Socket client in playerSockets)
            {
                try
                {
                    //Thread.Sleep(50);
                    client.Send(buffer_anc);
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
                    //clientSockets.Add(newClient);
                    Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                    receiveThread.Start();


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
            bool unique = true;
            bool connected = true;
            string name = "";

            while (connected && !terminating)
            {
                try
                {
                    if (name == "")
                    {


                        //break;
                        // thisClient.Close();


                        Byte[] buffer_name = new Byte[64];
                        thisClient.Receive(buffer_name);

                        name = Encoding.Default.GetString(buffer_name);
                        name = name.Substring(0, name.IndexOf("\0"));
                        if (names.Count == 0 || !names.Contains(name))
                        {
                            names.Add(name);
                            Byte[] buffer_unique = Encoding.Default.GetBytes("ok");
                            thisClient.Send(buffer_unique);

                            if (startGame)
                            {
                                Byte[] bufferDeny = Encoding.Default.GetBytes("Sorry, the game has already started. You will participate in the next game.\n");
                                thisClient.Send(bufferDeny);
                            }
                            else
                            {
                                playerSockets.Add(thisClient);
                                playersInGame++;
                                scoresOfPlayers[name] = 0;
                            }
                            clientSockets.Add(thisClient);
                            playerCount++;
                            logs.AppendText(name + " has connected" + "\n");
                            scores[name] = 0;

                        }
                        else
                        {
                            unique = false;
                            Byte[] buffer_unique = Encoding.Default.GetBytes("The name should be unique");
                            thisClient.Send(buffer_unique);
                            thisClient.Close();
                            logs.AppendText("Player is refused due to repeating name" + "\n");
                        }


                    }
                    else
                    {
                        Byte[] buffer2 = new Byte[64];
                        thisClient.Receive(buffer2);
                        if (!finishGame)
                        {
                            string incomingMessage = Encoding.Default.GetString(buffer2);
                            incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                            int answer;
                            Int32.TryParse(incomingMessage, out answer);
                            answers[name] = answer;
                            logs.AppendText(name + " gave " + incomingMessage + " as an answer" + "\n");
                            answersReceived++;
                        }
                    }
                }
                catch
                {
                    if (!terminating && unique)
                    {
                        logs.AppendText("Player with username: " + name + " has disconnected\n");
                        scores[name] = -1;
                        names.Remove(name);
                        playerCount--;



                        thisClient.Close();
                        clientSockets.Remove(thisClient);
                        playerSockets.Remove(thisClient);

                        if (scoresOfPlayers.ContainsKey(name))
                        {
                            if (answers.ContainsKey(name))
                            {
                                answersReceived--;
                            }
                            scoresOfPlayers.Remove(name);
                            playersInGame--;
                            scores.Remove(name);
                            answers.Remove(name);
                        }



                        foreach (Socket socket in clientSockets)
                        {
                            if (!finishGame)
                            {
                                Byte[] informAboutDisconnectionBuffer = Encoding.Default.GetBytes("Player with username: " + name + " disconnected!\n\n");
                                socket.Send(informAboutDisconnectionBuffer);
                            }

                        }

                        connected = false;
                        if (gameStarted && playersInGame == 1)
                        {
                            var a = scores.OrderByDescending(key => key.Value);
                            double winnerScore = a.ElementAt(0).Value;
                            foreach (var item in a)
                            {
                                if (item.Value == winnerScore)
                                {
                                    winnerNames.Add(item.Key);
                                }
                            }
                            finishGame = true;
                        }
                    }

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

        private void textBox_port_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int qnum;
            Int32.TryParse(textBox_qnum.Text, out qnum);
            if (qnum == 0)
            {
                logs.AppendText("You cannot set the number of questions to 0!\n");
            }
            else
            {
                getQuestions(qnum);
                button_start.Visible = true;
                textBox_port.Visible = true;
                label1.Visible = true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (playerCount > 1)
            {
                startGame = true;
                Thread gameThread = new Thread(playGame);
                gameThread.Start();
            }
            else
            {
                logs.AppendText("Cannot start the game with less than 2 players!\n");
            }

        }
    }
}
