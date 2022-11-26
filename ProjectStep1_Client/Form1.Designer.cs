namespace ProjectStep1_Client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.label_answer = new System.Windows.Forms.Label();
            this.textBox_answer = new System.Windows.Forms.TextBox();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.button_submit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 88);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 156);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "PORT:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 231);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Name:";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(142, 83);
            this.textBox_ip.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(302, 31);
            this.textBox_ip.TabIndex = 3;
            this.textBox_ip.Text = "127.0.0.1";
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(142, 150);
            this.textBox_port.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(302, 31);
            this.textBox_port.TabIndex = 4;
            this.textBox_port.Text = "8080";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(142, 225);
            this.textBox_name.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(302, 31);
            this.textBox_name.TabIndex = 5;
            this.textBox_name.Text = "alp";
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(524, 48);
            this.logs.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.logs.Name = "logs";
            this.logs.ReadOnly = true;
            this.logs.Size = new System.Drawing.Size(580, 948);
            this.logs.TabIndex = 6;
            this.logs.Text = "";
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(276, 302);
            this.button_connect.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(172, 44);
            this.button_connect.TabIndex = 7;
            this.button_connect.Text = "CONNECT";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_answer
            // 
            this.label_answer.AutoSize = true;
            this.label_answer.Location = new System.Drawing.Point(218, 615);
            this.label_answer.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label_answer.Name = "label_answer";
            this.label_answer.Size = new System.Drawing.Size(104, 25);
            this.label_answer.TabIndex = 8;
            this.label_answer.Text = "ANSWER";
            this.label_answer.Visible = false;
            this.label_answer.Click += new System.EventHandler(this.label4_Click);
            // 
            // textBox_answer
            // 
            this.textBox_answer.Location = new System.Drawing.Point(24, 646);
            this.textBox_answer.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox_answer.Name = "textBox_answer";
            this.textBox_answer.Size = new System.Drawing.Size(472, 31);
            this.textBox_answer.TabIndex = 9;
            this.textBox_answer.Visible = false;
            // 
            // button_disconnect
            // 
            this.button_disconnect.Enabled = false;
            this.button_disconnect.Location = new System.Drawing.Point(76, 302);
            this.button_disconnect.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(172, 44);
            this.button_disconnect.TabIndex = 10;
            this.button_disconnect.Text = "DISCONNECT";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.button_disconnect_Click);
            // 
            // button_submit
            // 
            this.button_submit.Location = new System.Drawing.Point(184, 696);
            this.button_submit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button_submit.Name = "button_submit";
            this.button_submit.Size = new System.Drawing.Size(172, 44);
            this.button_submit.TabIndex = 11;
            this.button_submit.Text = "SUBMIT";
            this.button_submit.UseVisualStyleBackColor = true;
            this.button_submit.Visible = false;
            this.button_submit.Click += new System.EventHandler(this.button_submit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 1023);
            this.Controls.Add(this.button_submit);
            this.Controls.Add(this.button_disconnect);
            this.Controls.Add(this.textBox_answer);
            this.Controls.Add(this.label_answer);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form1";
            this.Text = "cs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Label label_answer;
        private System.Windows.Forms.TextBox textBox_answer;
        private System.Windows.Forms.Button button_disconnect;
        private System.Windows.Forms.Button button_submit;
    }
}

