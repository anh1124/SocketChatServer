using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniWord_NgoNgocTrungAnh
{
    public partial class IconPickerForm : Form
    {
        private FlowLayoutPanel flowLayoutPanel;
        public string SelectedEmoji { get; private set; }
    
        public IconPickerForm()
        {
            InitializeComponent();
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Chọn Icon";

            flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Thêm một số emoji phổ biến
            string[] emojis = new string[]
            {
            "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆",
            "😉", "😊", "😋", "😎", "😍", "😘", "😗", "😙",
            "👍", "👎", "❤️", "💕", "💖", "💗", "💓", "💔",
            "🌟", "⭐", "✨", "💫", "🌙", "☀️", "⚡", "❄️"
            };

            foreach (string emoji in emojis)
            {
                Button btn = new Button
                {
                    Text = emoji,
                    Font = new Font("Segoe UI Emoji", 20),
                    Width = 50,
                    Height = 50
                };

                btn.Click += (s, e) =>
                {
                    SelectedEmoji = emoji;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };

                flowLayoutPanel.Controls.Add(btn);
            }

            this.Controls.Add(flowLayoutPanel);
        }
    }
}
