using System;
using System.Drawing;
using System.Windows.Forms;

namespace mayin
{
    public partial class Giris : Form
    {
        private TextBox txtKullaniciAdi;
        private TextBox txtOyunBoyutu;
        private Button btnOyna;
        private TextBox txtMayinSayisi;

        public Giris()
        {
            InitializeComponent();

            
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(710, 710);
            this.BackColor = Color.FromArgb(240, 248, 255); 

            
            Label lblBaslik = new Label();
            lblBaslik.Text = "Mayın Tarlası";
            lblBaslik.Font = new Font("Arial", 24, FontStyle.Bold);
            lblBaslik.ForeColor = Color.FromArgb(70, 130, 180);
            lblBaslik.AutoSize = true;
            lblBaslik.Location = new Point((this.Width - lblBaslik.Width) / 2 - 100, 50);
            Controls.Add(lblBaslik);

            Label lblKullaniciAdi = new Label();
            lblKullaniciAdi.Text = "Kullanıcı Adı:";
            lblKullaniciAdi.Font = new Font("Arial", 14, FontStyle.Bold);
            lblKullaniciAdi.ForeColor = Color.FromArgb(47, 79, 79);
            lblKullaniciAdi.AutoSize = true;
            lblKullaniciAdi.Location = new Point(70, 200);
            Controls.Add(lblKullaniciAdi);

           
            txtKullaniciAdi = new TextBox();
            txtKullaniciAdi.Font = new Font("Arial", 14);
            txtKullaniciAdi.Size = new Size(300, 30);
            txtKullaniciAdi.Location = new Point(300, 200);
            Controls.Add(txtKullaniciAdi);

            
            Label lblOyunBoyutu = new Label();
            lblOyunBoyutu.Text = "Oyun Boyutu (örn: 4-4):";
            lblOyunBoyutu.Font = new Font("Arial", 14, FontStyle.Bold);
            lblOyunBoyutu.ForeColor = Color.FromArgb(47, 79, 79);
            lblOyunBoyutu.AutoSize = true;
            lblOyunBoyutu.Location = new Point(70, 300);
            Controls.Add(lblOyunBoyutu);

            
            txtOyunBoyutu = new TextBox();
            txtOyunBoyutu.Font = new Font("Arial", 14);
            txtOyunBoyutu.Size = new Size(300, 30);
            txtOyunBoyutu.Location = new Point(300, 300);
            Controls.Add(txtOyunBoyutu);

            Label lblMayinSayisi = new Label();
            lblMayinSayisi.Text = "Mayın Sayısı:";
            lblMayinSayisi.Font= new Font("Arial", 14, FontStyle.Bold);
            lblMayinSayisi.ForeColor = Color.FromArgb(47, 79, 79);
            lblMayinSayisi.AutoSize = true;
            lblMayinSayisi.Location = new Point(70, 400);
            Controls.Add(lblMayinSayisi);  
            
            txtMayinSayisi= new TextBox();
            txtMayinSayisi.Font = new Font("Arial", 14);
            txtMayinSayisi.Size = new Size(300, 30);
            txtMayinSayisi.Location = new Point(300, 400);
            Controls.Add(txtMayinSayisi );





            
            btnOyna = new Button();
            btnOyna.Text = "Oyuna Başla";
            btnOyna.Font = new Font("Arial", 16, FontStyle.Bold);
            btnOyna.Size = new Size(300, 50);
            btnOyna.BackColor = Color.FromArgb(60, 179, 113);
            btnOyna.ForeColor = Color.White;
            btnOyna.FlatStyle = FlatStyle.Flat;
            btnOyna.FlatAppearance.BorderSize = 0;
            btnOyna.Location = new Point((this.Width - btnOyna.Width) / 2 - 10, 480);
            btnOyna.Click += BtnOyna_Click;
            Controls.Add(btnOyna);
            this.FormClosed += Giris_FormClosed;
        }

        private void Giris_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Application.Exit(); 
        }

        private void BtnOyna_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text;
            string boyut = txtOyunBoyutu.Text;
            string[] boyutlar = boyut.Split('-');

           
            if (!int.TryParse(txtMayinSayisi.Text, out int mayinS))
            {
                MessageBox.Show("Geçerli bir mayın sayısı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (boyutlar.Length == 2 && int.TryParse(boyutlar[0], out int x) && int.TryParse(boyutlar[1], out int y))
            {
                if (x >= 4 && x <= 30 && y >= 4 && y <= 30)
                {
                    int maxMayinSayisi = x * y;

                    
                    if (mayinS < 10 || mayinS >= maxMayinSayisi)
                    {
                        MessageBox.Show($"Mayın sayısı 10 ile {maxMayinSayisi - 1} arasında olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    AnaPencerem anaPencere = new AnaPencerem(kullaniciAdi, x, y, mayinS);
                    anaPencere.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Lütfen 4 ile 30 arasında bir boyut girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen x-y formatında bir boyut girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
