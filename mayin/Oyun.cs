using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;


namespace mayin
{
    internal class Oyun
    {
        private Button[] mMayin;
        private Form _form;
        private Random random;
        private Image mayinImage;
        private Image bosImage;
        private Image openedImage;
        private Image bayrakImage;
        private bool[] mayinMi;
        private int rows;
        private int cols;
        private Timer timer;
        private Label timerLabel;
        private int sayac;
        private string mKullanici; 
        private int hamleSayisi;
        private Label lblHamleSayisi;
        private int aMayinSayisi;



            

        public Oyun(Form form, string kullaniciAd, int aMayinSayisi)
        {


            _form = form;
            hamleSayisi = 0;
            lblHamleSayisi = new Label()

            {
                Location = new Point(330, 1),
                AutoSize = true,
                Text = "Hamle Sayısı: 0",
                Font = new Font("Arial", 12, FontStyle.Bold)

            };
            _form.Controls.Add(lblHamleSayisi);
            mKullanici = kullaniciAd; 
            random = new Random();
            mayinImage = Image.FromFile("mayinImage.png");
            bosImage = Image.FromFile("emptyImage.png");
            openedImage = Image.FromFile("openedImage.png");
            bayrakImage = Image.FromFile("bayrakImage.png");

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick; 

            
            timerLabel = new Label
            {
                Text = "Süre: 0",
                Location = new Point(230, 1),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            _form.Controls.Add(timerLabel);
            this.aMayinSayisi = aMayinSayisi;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (sender == null) return; 

            sayac++;
            timerLabel.Text = "Süre: " + sayac;
        }


        public void mayinYap(int x, int y)
        {
            ClearPreviousButtons();
            rows = x;
            cols = y;
            int toplam = x * y;
            mMayin = new Button[toplam];
            mayinMi = new bool[toplam];
            int yukariBosluk = 25;
            int aralik = 3;

            const int formGenislik = 900;
            const int formYukseklik = 900;
            int butonBoyutu;

            butonBoyutu = Math.Max(20, Math.Min((formGenislik - yukariBosluk - (aralik * (y - 1))) / y,
                                   (formYukseklik - yukariBosluk - (aralik * (x - 1))) / x));

            List<int> mayinIndeksleri = new List<int>();
            int mayinSayisi = aMayinSayisi;

            while (mayinIndeksleri.Count < mayinSayisi)
            {
                int randomIndex = random.Next(0, toplam);
                if (!mayinIndeksleri.Contains(randomIndex))
                {
                    mayinIndeksleri.Add(randomIndex);
                    mayinMi[randomIndex] = true; 
                }
            }

            int k = 0;

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    mMayin[k] = new Button();
                    mMayin[k].Name = "mayinButton" + k;
                    mMayin[k].BackgroundImage = bosImage; 
                    mMayin[k].BackgroundImageLayout = ImageLayout.Stretch;
                    mMayin[k].Size = new Size(butonBoyutu, butonBoyutu);
                    mMayin[k].Location = new Point(10 + (j * (butonBoyutu + aralik)), yukariBosluk + (i * (butonBoyutu + aralik)));
                    mMayin[k].Tag = k; 

                    
                    mMayin[k].Click += Button_Click;
                    mMayin[k].MouseUp += Button_MouseUp;

                    _form.Controls.Add(mMayin[k]);
                    k++;
                }
            }

            _form.ClientSize = new Size(formGenislik, formYukseklik + 40);
            Label yapimciLabel = new Label
            {
                Text = "Yapımcı: Numan Arif Deniz",
                AutoSize = true,
                Location = new Point(formGenislik - 200, formYukseklik + 10),
                ForeColor = Color.Gray,
                Font = new Font("Arial", 10, FontStyle.Italic)
            };
            _form.Controls.Add(yapimciLabel);

           
            sayac = 0;
            timer.Start(); 
        }

        public void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int index = (int)button.Tag;

             if (button.BackgroundImage == bayrakImage)
            {
                return;
            }

            hamleSayisi++; 
            lblHamleSayisi.Text = "Hamle Sayısı: " + hamleSayisi; 

            if (mayinMi[index])
            {
                button.BackgroundImage = mayinImage;
                timer.Stop(); 
                MessageBox.Show("Mayına bastınız! Oyun bitti.");

                RevealAllMines();
            }
            else
            {
                int surroundingMines = CountSurroundingMines(index);

               
                if (surroundingMines == 0)
                {
                    RevealSurroundingCells(index);
                }
                else
                {
                    button.Text = surroundingMines > 0 ? surroundingMines.ToString() : "";
                    button.ForeColor = Color.Black;
                    button.BackgroundImage = openedImage;
                    button.Font = new Font(button.Font.FontFamily, 11, FontStyle.Bold);
                    button.TextAlign = ContentAlignment.MiddleCenter;
                    button.Enabled = false;
                }

               
                if (CheckForWin())
                {
                    timer.Stop(); 
                    MessageBox.Show("Tebrikler! Oyunu kazandınız.");

                    RevealAllMines(); 

                   
                    int skor = HesaplaSkor();
                   

                    
                    Skorboard skorboard = new Skorboard();
                    skorboard.SkorEkle(skor, mKullanici); 
                    skorboard.Show(); 
                    
                }
            }
        }


        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (e.Button == MouseButtons.Right)
            {
                if (button.BackgroundImage == bayrakImage)
                {
                    button.BackgroundImage = bosImage; 
                }
                else
                {
                    button.BackgroundImage = bayrakImage; 
                }
                button.BackgroundImageLayout = ImageLayout.Stretch;

                
                
               

            }
        }


        public bool CheckForWin()
        {
            if (mMayin == null)
            {
                return false; 
            }

            for (int i = 0; i < mMayin.Length; i++)
            {
                if (!mayinMi[i] && mMayin[i].Enabled)
                {
                    return false;
                }
            }
            return true;
        }


        private void RevealSurroundingCells(int index)
        {
            Queue<int> indicesToCheck = new Queue<int>();
            indicesToCheck.Enqueue(index);

            while (indicesToCheck.Count > 0)
            {
                int currentIndex = indicesToCheck.Dequeue();
                Button currentButton = mMayin[currentIndex];

                
                if (!currentButton.Enabled || currentButton.BackgroundImage == bayrakImage)
                {
                    continue;
                }

               
                int surroundingMines = CountSurroundingMines(currentIndex);
                currentButton.BackgroundImage = openedImage;
                currentButton.Font = new Font(currentButton.Font.FontFamily, 11, FontStyle.Bold);
                currentButton.TextAlign = ContentAlignment.MiddleCenter;
                currentButton.Enabled = false; 

                if (surroundingMines > 0)
                {
                    currentButton.Text = surroundingMines.ToString();
                    currentButton.ForeColor = Color.Black;
                }
                else
                {
                    
                    int row = currentIndex / cols;
                    int col = currentIndex % cols;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                           
                            if (i == 0 && j == 0) continue;

                            int neighborRow = row + i;
                            int neighborCol = col + j;

                           
                            if (neighborRow >= 0 && neighborRow < rows && neighborCol >= 0 && neighborCol < cols)
                            {
                                int neighborIndex = neighborRow * cols + neighborCol;
                                indicesToCheck.Enqueue(neighborIndex); 
                            }
                        }
                    }
                }
            }
        }

        private int CountSurroundingMines(int index)
        {
            int count = 0;

            int row = index / cols;
            int col = index % cols;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int neighborRow = row + i;
                    int neighborCol = col + j;

                    if (neighborRow >= 0 && neighborRow < rows && neighborCol >= 0 && neighborCol < cols)
                    {
                        int neighborIndex = neighborRow * cols + neighborCol;

                        if (mayinMi[neighborIndex])
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private void RevealAllMines()
        {
            for (int i = 0; i < mayinMi.Length; i++)
            {
                if (mayinMi[i])
                {
                    
                    if (mMayin[i].BackgroundImage != bayrakImage)
                    {
                        mMayin[i].BackgroundImage = mayinImage;
                    }
                }
                mMayin[i].Enabled = false; 
            }
        }

        private void ClearPreviousButtons()
        {
            for (int i = _form.Controls.Count - 1; i >= 0; i--)
            {
                if (_form.Controls[i] is Button && _form.Controls[i].Name.StartsWith("mayinButton"))
                {
                    _form.Controls.RemoveAt(i);
                }
            }
        }
        public int HesaplaSkor()
        {
            int dogruBayrakSayisi = 0;

            
            for (int i = 0; i < mMayin.Length; i++)
            {
                if (mayinMi[i] && mMayin[i].BackgroundImage == bayrakImage)
                {
                    dogruBayrakSayisi++;
                }
            }
            

           
            return (int)((dogruBayrakSayisi / (double)sayac) * 1000);
        }   



    }
}
