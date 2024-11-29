using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace mayin
{
    public partial class Skorboard : Form
    {
        private static List<Tuple<string, int>> skorlar = new List<Tuple<string, int>>(); 
        private ListView listViewSkorlar;
        private Button geriDön;

        public Skorboard()
        {
            InitializeComponents();
            LoadSkorlar();
            geriDön.Click += GeriDön_Click;
        }

        private void GeriDön_Click(object? sender, EventArgs e)
        {
            this.Close();   
        }

        private void InitializeComponents()
        {
            
            Text = "Skorboard";
            Size = new Size(400, 400);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(240, 240, 240);

           
            listViewSkorlar = new ListView
            {
                Dock = DockStyle.Top, 
                Height = 300,
                View = View.Details,
                FullRowSelect = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                HeaderStyle = ColumnHeaderStyle.Nonclickable
            };
           
            listViewSkorlar.Columns.Add("Kullanıcı Adı", 250, HorizontalAlignment.Left);
            listViewSkorlar.Columns.Add("Skor", 120, HorizontalAlignment.Center);

            Controls.Add(listViewSkorlar); 

           
            geriDön = new Button
            {
                Size = new Size(100, 30),
                Text = "Geri Dön",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point((ClientSize.Width - 100) / 2, 330),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 20, 60),
                ForeColor = Color.White
            };
            geriDön.FlatAppearance.BorderSize = 0;
            Controls.Add(geriDön); 
        }

        public void SkorEkle(int skor, string kullaniciAdi)
        {
            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                kullaniciAdi = "Misafir";
            }

           
            skorlar.Add(new Tuple<string, int>(kullaniciAdi, skor));
            skorlar = skorlar.OrderByDescending(x => x.Item2).Take(10).ToList(); 

            LoadSkorlar();
        }

        private void LoadSkorlar()
        {
            listViewSkorlar.Items.Clear(); 
            foreach (var skor in skorlar)
            {
                ListViewItem item = new ListViewItem(skor.Item1);
                item.SubItems.Add(skor.Item2.ToString());
                listViewSkorlar.Items.Add(item);
            }
        }
    }
}
