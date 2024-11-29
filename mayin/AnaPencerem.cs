using Microsoft.VisualBasic;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace mayin
{
    internal class AnaPencerem : Form
    {
        private Button mOyun;
        private Oyun mayin;
        private System.Windows.Forms.Label lblKullaniciAdi;
        private string mKullanici;
        private int x, y;
        private Button skorAc;


        public AnaPencerem(string kullaniciAdi, int xBoyut, int yBoyut, int xmayinSayisi)
        {
            mKullanici = kullaniciAdi;
            x = xBoyut;
            y = yBoyut;

            Width = 710;
            Height = 710;
            DoubleBuffered = true;
            BackColor = Color.FromArgb(240, 240, 240); 

            
            lblKullaniciAdi = new System.Windows.Forms.Label();
            lblKullaniciAdi.Text = string.IsNullOrEmpty(mKullanici) ? "Misafir Oynuyor!" : mKullanici + " Oynuyor!";
            lblKullaniciAdi.AutoSize = true;
            lblKullaniciAdi.ForeColor = Color.Green;
            
            lblKullaniciAdi.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            
            Controls.Add(lblKullaniciAdi);

           
            UpdateKullaniciAdiLocation();
            this.Resize += (s, e) => UpdateKullaniciAdiLocation();

            
            mOyun = new Button();
            mOyun.Size = new Size(100, 22);
            mOyun.Text = "Yeni Oyun";
            mOyun.Location = new Point(20, 0); 
            mOyun.BackColor = Color.FromArgb(220, 20, 60); 
            mOyun.ForeColor = Color.White;
            mOyun.FlatStyle = FlatStyle.Flat;
            mOyun.FlatAppearance.BorderSize = 0;
            mOyun.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            mOyun.Click += MOyun_Click;
            Controls.Add(mOyun);

            
            skorAc = new Button();
            skorAc.Size = new Size(100, 22);
            skorAc.Text = "Skorboard";
            skorAc.Location = new Point(mOyun.Right + 10, 0);
            skorAc.BackColor = Color.FromArgb(30, 144, 255); 
            skorAc.ForeColor = Color.White;
            skorAc.FlatStyle = FlatStyle.Flat;
            skorAc.FlatAppearance.BorderSize = 0;
            skorAc.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            skorAc.Click += SkorAc_Click;
            Controls.Add(skorAc);

           
            mayin = new Oyun(this, mKullanici,xmayinSayisi);
            mayin.mayinYap(x, y); 
            this.FormClosed += AnaPencerem_FormClosed;
        }

        private void AnaPencerem_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Application.Exit(); 
        }

        private void SkorAc_Click(object? sender, EventArgs e)
        {
            

            
                Skorboard skorboard = new Skorboard();
                skorboard.FormClosed += (s, args) => this.Show(); 
                this.Hide(); 
                skorboard.Show(); 
            
        }

        private void UpdateKullaniciAdiLocation()
        {
            lblKullaniciAdi.Location = new Point(ClientSize.Width - lblKullaniciAdi.Width - 20, 0);
        }

        private void MOyun_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Yeni bir oyun açmak istediğinize emin misiniz?", "Yeni Oyun", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                var tekrar= MessageBox.Show("Yine Aynı boyutlarda mı olsun", "Tekrar Oyna", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(tekrar== DialogResult.Yes)
                {
                    mayin.mayinYap(x,y);
                }
                else
                {
                    Giris giris = new Giris();
                    this.Hide();
                    giris.Show();
                }
               
            }
        }
    }
}
