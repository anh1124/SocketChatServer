using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SeverChitChat


{
    public partial class ServerForm : Form
    {
        private bool isServerRunning = false;

        public class ClientInfo
        {
            public Socket Socket { get; set; }
            public string Nickname { get; set; }
            public IPEndPoint EndPoint { get; set; }
        }

        private Socket serverSocket;
        private Thread listenThread;
        private Dictionary<string, ClientInfo> connectedClients = new Dictionary<string, ClientInfo>();
        private Dictionary<string, List<string>> chatHistories = new Dictionary<string, List<string>>(); // Lưu lịch sử tin nhắn
        private object lockObject = new object();

        public ServerForm()
        {
            InitializeComponent();
            this.btnStartServer.Click += new EventHandler(this.btnStartServer_Click);
            this.btnStopServer.Click += new EventHandler(this.btnStopServer_Click);
            this.sendMessengerToClient.Click += new EventHandler(this.sendMessengerToClient_Click);
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            int port = 8888;
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(endPoint);
            serverSocket.Listen(10);
            txtLog.AppendText("\rServer đang chạy và lắng nghe kết nối...\n");

            isServerRunning = true; // Đánh dấu server đang chạy
            listenThread = new Thread(ListenForClient);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ListenForClient()
        {
            while (isServerRunning)
            {
                try
                {
                    Socket newClient = serverSocket.Accept();
                    Thread clientThread = new Thread(() => HandleClient(newClient));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                catch (SocketException)
                {
                    if (!isServerRunning) break; // Dừng khi server tắt
                }
                catch (Exception ex)
                {
                    txtLog.Invoke((Action)(() => txtLog.AppendText($"\rLỗi khi chấp nhận client: {ex.Message}\n")));
                }
            }
        }


        private void UpdateClientComboBox()
        {
            comboBoxChooseChatUser.Invoke((Action)(() =>
            {
                comboBoxChooseChatUser.Items.Clear();
                comboBoxChooseChatUser.Items.Add("ALL");
                lock (lockObject)
                {
                    foreach (var client in connectedClients.Values)
                    {
                        comboBoxChooseChatUser.Items.Add(client.Nickname);
                    }
                }
                comboBoxChooseChatUser.SelectedIndex = 0;
            }));
        }

        private void BroadcastOnlineUsers()
        {
            string userList = "USERLIST|";
            lock (lockObject)
            {
                foreach (var client in connectedClients.Values)
                {
                    userList += $"{client.Nickname},";
                }
            }
            userList = userList.TrimEnd(',');
            BroadcastMessage(userList);

            UpdateClientComboBox();
        }

        private void BroadcastMessage(string message)
        {
            byte[] broadcastData = Encoding.UTF8.GetBytes(message);
            lock (lockObject)
            {
                foreach (var client in connectedClients.Values)
                {
                    try
                    {
                        client.Socket.Send(broadcastData);
                    }
                    catch { }
                }
            }
        }

        private void SaveChatHistory(string user, string message)
        {
            lock (lockObject)
            {
                if (!chatHistories.ContainsKey(user))
                {
                    chatHistories[user] = new List<string>();
                }
                chatHistories[user].Add(message);
            }
        }

        private void HandleClient(Socket client)
        {
            byte[] buffer = new byte[1024];
            ClientInfo clientInfo = null;

            try
            {
                int receivedBytes = client.Receive(buffer);
                string nickname = Encoding.UTF8.GetString(buffer, 0, receivedBytes).Replace("NICK|", "");
                // Kiểm tra trùng lặp tên
                lock (lockObject)
                {
                    string originalNickname = nickname;
                    int count = 1;
                    while (connectedClients.ContainsKey(nickname))
                    {
                        nickname = $"{originalNickname}{count}";
                        count++;
                    }
                }
                // Gửi tên đã được sửa đổi về cho client (nếu thay đổi)
                if (nickname != Encoding.UTF8.GetString(buffer, 0, receivedBytes).Replace("NICK|", ""))
                {
                    client.Send(Encoding.UTF8.GetBytes($"UPDATED_NICK|{nickname}"));
                }

                clientInfo = new ClientInfo
                {
                    Socket = client,
                    Nickname = nickname,
                    EndPoint = (IPEndPoint)client.RemoteEndPoint
                };

                lock (lockObject)
                {
                    connectedClients.Add(nickname, clientInfo);
                    chatHistories[nickname] = new List<string>();
                }

                txtLog.Invoke((Action)(() =>
                {
                    txtLog.AppendText($"\r\n{nickname} đã kết nối.\n");
                    txtLog.AppendText($"\rHiện có {connectedClients.Count} client đang kết nối.\n");
                    UpdateClientComboBox();
                }));

                BroadcastOnlineUsers();

                while (true)
                {
                    receivedBytes = client.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer, 0, receivedBytes);

                    if (message.StartsWith("QUIT"))
                        break;

                    // Xử lý tin nhắn CHAT
                    if (message.StartsWith("CHAT|"))
                    {
                        string[] parts = message.Split('|');
                        if (parts.Length >= 3)
                        {
                            string targetUser = parts[1];
                            string chatMessage = parts[2];

                            // Lưu tin nhắn vào lịch sử
                            SaveChatHistory(nickname, $"To {targetUser}: {chatMessage}");
                            SaveChatHistory(targetUser, $"From {nickname}: {chatMessage}");

                            // Gửi tin nhắn đến người nhận
                            if (connectedClients.ContainsKey(targetUser))
                            {
                                string formattedMessage = $"CHAT|{nickname}|{chatMessage}";
                                byte[] messageData = Encoding.UTF8.GetBytes(formattedMessage);
                                connectedClients[targetUser].Socket.Send(messageData);
                            }
                        }
                    }
                    // Xử lý lệnh DELETE
                    // Trong ServerForm.cs, cập nhật phần xử lý DELETE trong HandleClient:
                    if (message.StartsWith("DELETE|"))
                    {
                        string[] parts = message.Split('|');
                        if (parts.Length >= 4)
                        {
                            string targetUser = parts[1];
                            string senderNickname = parts[2];
                            string messageContent = parts[3];

                            // Xóa tin nhắn trong lịch sử trên server
                            lock (lockObject)
                            {
                                if (chatHistories.ContainsKey(targetUser))
                                {
                                    string messageToDelete = $"From {senderNickname}: {messageContent}";
                                    chatHistories[targetUser].Remove(messageToDelete);
                                }
                                if (chatHistories.ContainsKey(senderNickname))
                                {
                                    string messageToDelete = $"To {targetUser}: {messageContent}";
                                    chatHistories[senderNickname].Remove(messageToDelete);
                                }
                            }

                            // Gửi thông báo xóa cho người nhận
                            if (connectedClients.ContainsKey(targetUser))
                            {
                                string deleteNotification = $"DELETE|{senderNickname}|{messageContent}";
                                byte[] deleteData = Encoding.UTF8.GetBytes(deleteNotification);
                                connectedClients[targetUser].Socket.Send(deleteData);
                            }

                            // Gửi xác nhận xóa cho người gửi
                            if (connectedClients.ContainsKey(senderNickname))
                            {
                                string deleteConfirmation = $"DELETE|{targetUser}|{messageContent}";
                                byte[] confirmData = Encoding.UTF8.GetBytes(deleteConfirmation);
                                connectedClients[senderNickname].Socket.Send(confirmData);
                            }
                        }
                    }
                    txtLog.Invoke((Action)(() => txtLog.AppendText($"\r\n{nickname}: {message}\n")));
                }
            }
            catch (Exception ex)
            {
                txtLog.Invoke((Action)(() =>
                    txtLog.AppendText($"\rLỗi xử lý client: {ex.Message}\n")));
            }
            finally
            {
                if (clientInfo != null)
                {
                    lock (lockObject)
                    {
                        connectedClients.Remove(clientInfo.Nickname);
                    }
                    txtLog.Invoke((Action)(() =>
                    {
                        txtLog.AppendText($"\r\n{clientInfo.Nickname} đã ngắt kết nối.\n");
                        txtLog.AppendText($"\rHiện có {connectedClients.Count} client đang kết nối.\n");
                        UpdateClientComboBox();
                    }));
                    BroadcastOnlineUsers();
                }
                client.Close();
            }
        }

        private void sendMessengerToClient_Click(object sender, EventArgs e)
        {
            string selectedUser = comboBoxChooseChatUser.SelectedItem?.ToString();
            string serverMessage = richTextBox1.Text.Trim();

            if (string.IsNullOrEmpty(serverMessage))
            {
                MessageBox.Show("Nội dung tin nhắn không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUser == "ALL")
            {
                // Gửi tin nhắn cho tất cả
                BroadcastMessage($"SERVER|{serverMessage}");
                txtLog.AppendText($"\r[SERVER TO ALL]: {serverMessage}\n");

                lock (lockObject)
                {
                    foreach (var user in connectedClients.Keys)
                    {
                        SaveChatHistory(user, $"SERVER TO ALL: {serverMessage}");
                    }
                }
            }
            else
            {
                // Gửi tin nhắn riêng cho một user
                lock (lockObject)
                {
                    if (connectedClients.ContainsKey(selectedUser))
                    {
                        var client = connectedClients[selectedUser];
                        // Thay đổi format tin nhắn thành CHAT|Server|message
                        string privateMessage = $"CHAT|Server|{serverMessage}";
                        client.Socket.Send(Encoding.UTF8.GetBytes(privateMessage));

                        SaveChatHistory(selectedUser, $"SERVER TO YOU: {serverMessage}");
                        txtLog.AppendText($"\r[SERVER TO {selectedUser}]: {serverMessage}\n");
                    }
                }
            }

            richTextBox1.Clear();
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            try
            {
                isServerRunning = false; // Đánh dấu server đã dừng

                // Đóng socket server
                if (serverSocket != null)
                {
                    serverSocket.Close();
                }

                // Đóng tất cả kết nối client
                lock (lockObject)
                {
                    foreach (var client in connectedClients.Values)
                    {
                        client.Socket.Close();
                    }
                    connectedClients.Clear();
                }

                txtLog.AppendText("\rServer đã dừng.\r\n");
            }
            catch (Exception ex)
            {
                txtLog.AppendText($"\rLỗi khi dừng server: {ex.Message}\n");
            }
        }

    }
}
