using System.Windows.Forms;

namespace SeverChitChat
{
    public partial class ServerForm : Form
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
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.sendMessengerToClient = new System.Windows.Forms.Button();
            this.comboBoxChooseChatUser = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 36);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(579, 195);
            this.txtLog.TabIndex = 0;
            // 
            // btnStartServer
            // 
            this.btnStartServer.BackColor = System.Drawing.Color.Green;
            this.btnStartServer.Location = new System.Drawing.Point(597, 36);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(165, 23);
            this.btnStartServer.TabIndex = 1;
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "ServerLog";
            // 
            // btnStopServer
            // 
            this.btnStopServer.BackColor = System.Drawing.Color.Red;
            this.btnStopServer.Location = new System.Drawing.Point(597, 65);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(165, 23);
            this.btnStopServer.TabIndex = 3;
            this.btnStopServer.Text = "Stop Server";
            this.btnStopServer.UseVisualStyleBackColor = false;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // sendMessengerToClient
            // 
            this.sendMessengerToClient.Location = new System.Drawing.Point(597, 305);
            this.sendMessengerToClient.Name = "sendMessengerToClient";
            this.sendMessengerToClient.Size = new System.Drawing.Size(191, 23);
            this.sendMessengerToClient.TabIndex = 5;
            this.sendMessengerToClient.Text = "sendMessengerToClient";
            this.sendMessengerToClient.UseVisualStyleBackColor = true;
            // 
            // comboBoxChooseChatUser
            // 
            this.comboBoxChooseChatUser.FormattingEnabled = true;
            this.comboBoxChooseChatUser.Location = new System.Drawing.Point(228, 280);
            this.comboBoxChooseChatUser.Name = "comboBoxChooseChatUser";
            this.comboBoxChooseChatUser.Size = new System.Drawing.Size(132, 24);
            this.comboBoxChooseChatUser.TabIndex = 7;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 305);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(576, 91);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(105, 283);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Current online user";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 283);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "chatBox";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.comboBoxChooseChatUser);
            this.Controls.Add(this.sendMessengerToClient);
            this.Controls.Add(this.btnStopServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.txtLog);
            this.Name = "ServerForm";
            this.Text = "Server Chat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnStartServer;
        private Label label1;
        private Button btnStopServer;
        private Button sendMessengerToClient;
        private ComboBox comboBoxChooseChatUser;
        private RichTextBox richTextBox1;
        private Label label2;
        private Label label3;
    }
}