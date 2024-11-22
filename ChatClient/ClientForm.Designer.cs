namespace ChatClient
{
    partial class ClientForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtNickname;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.ComboBox cboUsers;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblNickname;
        private System.Windows.Forms.Label lblUsers;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblNickname = new System.Windows.Forms.Label();
            this.txtNickname = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblUsers = new System.Windows.Forms.Label();
            this.cboUsers = new System.Windows.Forms.ComboBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.listBoxChatList = new System.Windows.Forms.ListBox();
            this.listBoxMessengerShow = new System.Windows.Forms.ListBox();
            this.labelCurrentChatUserName = new System.Windows.Forms.Label();
            this.buttonpickedIcon = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblNickname
            // 
            this.lblNickname.Location = new System.Drawing.Point(10, 15);
            this.lblNickname.Name = "lblNickname";
            this.lblNickname.Size = new System.Drawing.Size(69, 20);
            this.lblNickname.TabIndex = 0;
            this.lblNickname.Text = "Nickname:";
            // 
            // txtNickname
            // 
            this.txtNickname.Location = new System.Drawing.Point(85, 12);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(100, 22);
            this.txtNickname.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(190, 10);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(80, 25);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Kết nối";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblUsers
            // 
            this.lblUsers.Location = new System.Drawing.Point(10, 45);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(57, 20);
            this.lblUsers.TabIndex = 3;
            this.lblUsers.Text = "Online Users:";
            // 
            // cboUsers
            // 
            this.cboUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsers.Location = new System.Drawing.Point(85, 42);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.Size = new System.Drawing.Size(185, 24);
            this.cboUsers.TabIndex = 4;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(137, 304);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(317, 22);
            this.txtMessage.TabIndex = 6;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(460, 303);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(70, 25);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "Gửi";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(276, 15);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(391, 50);
            this.txtLog.TabIndex = 5;
            this.txtLog.Text = "";
            // 
            // listBoxChatList
            // 
            this.listBoxChatList.FormattingEnabled = true;
            this.listBoxChatList.ItemHeight = 16;
            this.listBoxChatList.Location = new System.Drawing.Point(10, 82);
            this.listBoxChatList.Name = "listBoxChatList";
            this.listBoxChatList.Size = new System.Drawing.Size(121, 244);
            this.listBoxChatList.TabIndex = 8;
            // 
            // listBoxMessengerShow
            // 
            this.listBoxMessengerShow.FormattingEnabled = true;
            this.listBoxMessengerShow.ItemHeight = 16;
            this.listBoxMessengerShow.Location = new System.Drawing.Point(137, 114);
            this.listBoxMessengerShow.Name = "listBoxMessengerShow";
            this.listBoxMessengerShow.Size = new System.Drawing.Size(438, 180);
            this.listBoxMessengerShow.TabIndex = 9;
            // 
            // labelCurrentChatUserName
            // 
            this.labelCurrentChatUserName.AutoSize = true;
            this.labelCurrentChatUserName.Location = new System.Drawing.Point(273, 95);
            this.labelCurrentChatUserName.Name = "labelCurrentChatUserName";
            this.labelCurrentChatUserName.Size = new System.Drawing.Size(172, 16);
            this.labelCurrentChatUserName.TabIndex = 10;
            this.labelCurrentChatUserName.Text = "labelCurrentChatUserName";
            // 
            // buttonpickedIcon
            // 
            this.buttonpickedIcon.Location = new System.Drawing.Point(537, 304);
            this.buttonpickedIcon.Name = "buttonpickedIcon";
            this.buttonpickedIcon.Size = new System.Drawing.Size(38, 23);
            this.buttonpickedIcon.TabIndex = 11;
            this.buttonpickedIcon.Text = "😀";
            this.buttonpickedIcon.UseVisualStyleBackColor = true;
            this.buttonpickedIcon.Click += new System.EventHandler(this.buttonpickedIcon_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Bạn đang chít chát với";
            // 
            // ClientForm
            // 
            this.ClientSize = new System.Drawing.Size(603, 375);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonpickedIcon);
            this.Controls.Add(this.labelCurrentChatUserName);
            this.Controls.Add(this.listBoxMessengerShow);
            this.Controls.Add(this.listBoxChatList);
            this.Controls.Add(this.lblNickname);
            this.Controls.Add(this.txtNickname);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.cboUsers);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Name = "ClientForm";
            this.Text = "Chat Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.ListBox listBoxChatList;
        private System.Windows.Forms.ListBox listBoxMessengerShow;
        private System.Windows.Forms.Label labelCurrentChatUserName;
        private System.Windows.Forms.Button buttonpickedIcon;
        private System.Windows.Forms.Label label1;
    }
}