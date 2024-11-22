using MiniWord_NgoNgocTrungAnh;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ClientForm : Form
    {
        private Socket clientSocket;
        private Thread receiveThread;
        private string nickname;
        private Dictionary<string, List<string>> chatHistories = new Dictionary<string, List<string>>();

        public ClientForm()
        {
            InitializeComponent();

            cboUsers.SelectedIndexChanged += cboUsers_SelectedIndexChanged;
            listBoxChatList.SelectedIndexChanged += listBoxChatList_SelectedIndexChanged;
            listBoxMessengerShow.MouseUp += listBoxMessengerShow_MouseUp;

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNickname.Text))
            {
                MessageBox.Show("Hãy nhập tên người dùng!");
                return;
            }

            nickname = txtNickname.Text;

            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect("127.0.0.1", 8888);

                byte[] nickData = Encoding.UTF8.GetBytes($"NICK|{nickname}");
                clientSocket.Send(nickData);

                MessageBox.Show("Kết nối thành công!");
                txtNickname.Enabled = false;
                btnConnect.Enabled = false;

                StartReceiving();

                // Hiển thị kênh server trong listBoxChatList
                chatHistories["Server"] = new List<string>();
                listBoxChatList.Items.Add("Server");
                listBoxChatList.SelectedIndex = 0; // Mặc định chọn kênh server
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối đến server: {ex.Message}");
            }
        }


        private void StartReceiving()
        {
            receiveThread = new Thread(() =>
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    try
                    {
                        int receivedBytes = clientSocket.Receive(buffer);
                        string message = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                        if (message.StartsWith("UPDATED_NICK|"))
                        {
                            string updatedNickname = message.Replace("UPDATED_NICK|", "");
                            this.Invoke((Action)(() =>
                            {
                                MessageBox.Show($"Tên bạn đã được đổi thành {updatedNickname} do bị trùng.", "Thông báo");
                                nickname = updatedNickname;
                                txtNickname.Text = updatedNickname; // Cập nhật giao diện
                            }));
                        }
                        else if (message.StartsWith("USERLIST|"))
                        {
                            string[] users = message.Replace("USERLIST|", "").Split(',');
                            this.Invoke((Action)(() =>
                            {
                                cboUsers.Items.Clear();
                                foreach (string user in users)
                                {
                                    if (user != nickname && !string.IsNullOrEmpty(user))
                                        cboUsers.Items.Add(user);
                                }
                            }));
                        }
                        else if (message.StartsWith("CHAT|"))
                        {
                            string[] parts = message.Split('|');
                            string sender = parts[1];
                            string chatMessage = parts[2];

                            this.Invoke((Action)(() =>
                            {
                                // Nếu sender là Server, đổi key thành "Server"
                                string chatKey = sender == "Server" ? "Server" : sender;

                                if (!chatHistories.ContainsKey(chatKey))
                                {
                                    chatHistories[chatKey] = new List<string>();
                                    if (!listBoxChatList.Items.Contains(chatKey))
                                    {
                                        listBoxChatList.Items.Add(chatKey);
                                    }
                                }

                                chatHistories[chatKey].Add($"{sender}: {chatMessage}");

                                // Cập nhật hiển thị nếu đang ở đúng tab chat
                                if (listBoxChatList.SelectedItem?.ToString() == chatKey)
                                {
                                    UpdateChatDisplay(chatKey);
                                }
                            }));
                        }
                        else if (message.StartsWith("SERVER|"))
                        {
                            string serverMessage = message.Replace("SERVER|", "");
                            this.Invoke((Action)(() =>
                            {
                                if (!chatHistories.ContainsKey("Server"))
                                {
                                    chatHistories["Server"] = new List<string>();
                                    if (!listBoxChatList.Items.Contains("Server"))
                                    {
                                        listBoxChatList.Items.Add("Server");
                                    }
                                }

                                chatHistories["Server"].Add($"Server: {serverMessage}");
                                if (listBoxChatList.SelectedItem?.ToString() == "Server")
                                {
                                    UpdateChatDisplay("Server");
                                }
                            }));
                        }
                        else if (message.StartsWith("DELETE|"))
                        {
                            string[] parts = message.Split('|');
                            if (parts.Length >= 3)
                            {
                                string sender = parts[1];
                                string messageToDelete = parts[2];

                                this.Invoke((Action)(() =>
                                {
                                    // Xác định key cho lịch sử chat
                                    string chatKey = sender;

                                    if (chatHistories.ContainsKey(chatKey))
                                    {
                                        // Tìm và xóa tin nhắn tương ứng
                                        List<string> messagesToRemove = new List<string>();
                                        foreach (string msg in chatHistories[chatKey])
                                        {
                                            if (msg.EndsWith(messageToDelete) ||
                                                msg == $"{sender}: {messageToDelete}" ||
                                                msg == $"You: {messageToDelete}")
                                            {
                                                messagesToRemove.Add(msg);
                                            }
                                        }

                                        foreach (string msg in messagesToRemove)
                                        {
                                            chatHistories[chatKey].Remove(msg);
                                        }

                                        // Cập nhật hiển thị nếu đang ở đúng tab chat
                                        if (listBoxChatList.SelectedItem?.ToString() == chatKey)
                                        {
                                            UpdateChatDisplay(chatKey);
                                        }
                                    }
                                }));
                            }
                        }

                    }
                    catch
                    {
                        break;
                    }
                }
            });
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }



        private void cboUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboUsers.SelectedItem != null)
            {
                string selectedUser = cboUsers.SelectedItem.ToString();

                // Log thông tin để kiểm tra
                Console.WriteLine($"Selected user from cboUsers: {selectedUser}");
                txtLog.AppendText($"Selected user from cboUsers: {selectedUser}\n");

                // Thêm vào listBoxChatList nếu chưa tồn tại
                if (!listBoxChatList.Items.Contains(selectedUser))
                {
                    listBoxChatList.Items.Add(selectedUser);
                    txtLog.AppendText($"Added new user to listBoxChatList: {selectedUser}\n");
                }

                // Chọn user và cập nhật lịch sử
                listBoxChatList.SelectedItem = selectedUser;

                if (!chatHistories.ContainsKey(selectedUser))
                {
                    chatHistories[selectedUser] = new List<string>();
                    txtLog.AppendText($"Created new chat history for: {selectedUser}\n");
                }

                UpdateChatDisplay(selectedUser);
            }
        }


        private void listBoxChatList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxChatList.SelectedItem != null)
            {
                string selectedUser = listBoxChatList.SelectedItem.ToString();
                txtLog.AppendText($"Selected user from listBoxChatList: {selectedUser}\n");

                // Cập nhật label hiển thị tên người dùng đang chat
                labelCurrentChatUserName.Text = selectedUser;

                // Cập nhật hiển thị chat
                UpdateChatDisplay(selectedUser);
            }
        }

        private void UpdateChatDisplay(string user)
        {
            listBoxMessengerShow.Items.Clear();
            if (chatHistories.ContainsKey(user))
            {
                foreach (string message in chatHistories[user])
                {
                    listBoxMessengerShow.Items.Add(message);
                }
            }
            else
            {
                Console.WriteLine($"Lịch sử chat cho {user} không tồn tại.");
            }
        }


        // In ClientForm.cs, modify the listBoxMessengerShow_MouseUp method:
        private void listBoxMessengerShow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && listBoxMessengerShow.SelectedItem != null)
            {
                string selectedMessage = listBoxMessengerShow.SelectedItem.ToString();

                // Only show delete option if the message contains "You" (sent by the current user)
                if (selectedMessage.StartsWith("You: "))
                {
                    ContextMenu cm = new ContextMenu();
                    MenuItem deleteItem = new MenuItem("Xóa");

                    deleteItem.Click += (s, ev) =>
                    {
                        string currentUser = listBoxChatList.SelectedItem?.ToString();
                        if (currentUser != null && chatHistories.ContainsKey(currentUser))
                        {
                            string messageContent = selectedMessage.Substring("You: ".Length);

                            // Gửi lệnh xóa về server
                            byte[] deleteCommand = Encoding.UTF8.GetBytes($"DELETE|{currentUser}|{nickname}|{messageContent}");
                            clientSocket.Send(deleteCommand);
                        }
                    };

                    cm.MenuItems.Add(deleteItem);
                    cm.Show(listBoxMessengerShow, e.Location);
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (clientSocket != null && clientSocket.Connected && !string.IsNullOrEmpty(txtMessage.Text))
            {
                // Get the current time and format it
                string timeStamp = DateTime.Now.ToString("HH:mm:ss");
                string message = txtMessage.Text;
                string messageWithTime = $"[{timeStamp}] - {message}";
                

                string targetUser = labelCurrentChatUserName.Text; // Người dùng được hiển thị trên label

                if (!string.IsNullOrEmpty(targetUser))
                {
                   
                    

                    // Gửi tin nhắn tới người dùng hoặc server
                    byte[] messageData = Encoding.UTF8.GetBytes($"CHAT|{targetUser}|{messageWithTime}");
                    clientSocket.Send(messageData);

                    // Lưu vào lịch sử chat (không hiển thị thời gian trong lịch sử chat)
                    if (!chatHistories.ContainsKey(targetUser))
                        chatHistories[targetUser] = new List<string>();

                    chatHistories[targetUser].Add($"You: {messageWithTime}");  // Only save the message part, not the timestamp
                    UpdateChatDisplay(targetUser);

                    txtMessage.Clear();
                }
                else
                {
                    MessageBox.Show("Không có người nhận tin nhắn!");
                }
            }
            else
            {
                MessageBox.Show("Kết nối bị gián đoạn hoặc tin nhắn trống!");
            }
        }




        private void buttonpickedIcon_Click(object sender, EventArgs e)
        {
            // Mở form IconPickerForm
            IconPickerForm pickerForm = new IconPickerForm();
            if (pickerForm.ShowDialog() == DialogResult.OK)
            {
                // Nhận emoji đã chọn và thêm vào txtMessage
                txtMessage.Text += pickerForm.SelectedEmoji;
                txtMessage.Focus(); // Đặt con trỏ vào txtMessage
                txtMessage.SelectionStart = txtMessage.Text.Length; // Đặt vị trí con trỏ cuối chuỗi
            }
        }

    }
}
