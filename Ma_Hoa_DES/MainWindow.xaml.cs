using System.Windows;
using Ma_Hoa_DES.DocFile;
using Ma_Hoa_DES.MaHoa;
using Microsoft.Win32;
using System.IO;
using System;
using System.Threading;
using System.Numerics;

namespace Ma_Hoa_DES
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool filehaychuoi = false;
        int mahoahaygiaima = 0;
        string linkbanro = "C:\\Mien cuong\\An toàn thông tin\\MaHoa_DES\\Ma_Hoa_DES\\DES_GIAIMA.txt";
        string linkbanma = "C:\\Mien cuong\\An toàn thông tin\\MaHoa_DES\\Ma_Hoa_DES\\DES_MAHOA.txt";
        DES64bit MaHoaDES64;
        ChuoiNhiPhan kq;


        public MainWindow()
        {
            InitializeComponent();
        }
        public void MaHoa()
        {
            if (mahoahaygiaima == 1)
            {
                Khoa khoa = new Khoa(khoatxt.Text);
                if (!khoa.KiemTraKhoa())
                    MessageBox.Show("Lỗi mã hóa. Khóa chưa đủ độ dài");
                else
                {
                    MaHoaDES64 = new DES64bit();
                    string kqstr = MaHoaDES64.ThucHienDESText(khoa, banrotxt.Text, 1);
                    banmatxt.Text = kqstr;
                }
            }
            else
            {
                Khoa khoa = new Khoa(khoatxt1.Text);
                if (!khoa.KiemTraKhoa())
                    MessageBox.Show("Lỗi mã hóa. Khóa chưa đủ độ dài");
                else
                {
                    MaHoaDES64 = new DES64bit();
                    string kqstr = MaHoaDES64.ThucHienDESText(khoa, banmatxt1.Text, -1);
                    banrotxt1.Text = kqstr;
                }
            }
        }

        private void mahoabtn_Click(object sender, RoutedEventArgs e)
        {
            mahoahaygiaima = 1;
            MaHoa();
        }

        private void fileselectbtn_Click(object sender, RoutedEventArgs e)
        {
            banrotxt.Clear();
            banmatxt.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                if (Path.GetExtension(openFileDialog.FileName) == ".txt")
                {
                    linkbanro = openFileDialog.FileName;
                    linkbanma = openFileDialog.FileName.Replace(".", "_MAHOA.");
                    banrotxt.Text = File.ReadAllText(linkbanro);
                }
                else if (Path.GetExtension(openFileDialog.FileName) == ".docx")
                {
                    linkbanro = openFileDialog.FileName;                  
                    linkbanma = openFileDialog.FileName.Replace(".", "_MAHOA.");
                    banrotxt.Text = DocFileDoc.FileReadToString(linkbanro);
                }
                else
                {
                    MessageBox.Show("Định dạng file sai, phải là .docx hoặc .txt");
                }
            }
        }

        private void giaimabtn_Click(object sender, RoutedEventArgs e)
        {
            mahoahaygiaima = -1;
            MaHoa();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            banmatxt1.Text = banmatxt.Text;
        }

        private void filegiaimaselectbtn_Click(object sender, RoutedEventArgs e)
        {
            banrotxt1.Clear();
            banmatxt1.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                linkbanma = openFileDialog.FileName;
                linkbanro = openFileDialog.FileName.Replace(".", "_GIAIMA.");
                ChuoiNhiPhan chuoi = DocFileTxt.FileReadToBinary(linkbanma);
                banmatxt1.Text = chuoi.GetText();
            }
        }


        private void luubanmabtn_Click(object sender, RoutedEventArgs e)
        {
            kq = ChuoiNhiPhan.ChuyenChuSangChuoiNhiPhan(banmatxt.Text);
            DocFileTxt.WriteBinaryToFile(linkbanma, kq);
        }

        private void luubanrobtn_Click(object sender, RoutedEventArgs e)
        {
            linkbanro.Replace(".", "_GIAIMA.");
            //File.WriteAllText(linkbanro, banrotxt1.Text);
            DocFileDoc.WriteStringToFile(linkbanro, banrotxt1.Text);
        }
        private void btnChiaKhoa_Click(object sender, EventArgs e)
        {
            bool check = true;
            BigInteger checkP;
            if (String.IsNullOrEmpty(txtKChia.Text) || String.IsNullOrEmpty(txtPChia.Text)
                || String.IsNullOrEmpty(txtGTV1.Text) || String.IsNullOrEmpty(txtGTV2.Text)
                || String.IsNullOrEmpty(txtGTA1.Text))
            {
                check = false;
                MessageBox.Show("Vui lòng nhập lòng nhập đầy đủ thông tin để chia khóa!", "Lỗi");
            }

            else if (BigInteger.TryParse(txtPChia.Text, out checkP) == false || !IsPrime(checkP))
            {
                check = false;
                MessageBox.Show("Vui lòng nhập p phải là một số nguyên tố!", "Lỗi");
            }

            if (check)
            {
                string kq = "Các cặp khóa: \n";
                BigInteger Key = BigInteger.Parse(txtKChia.Text);
                BigInteger P = BigInteger.Parse(txtPChia.Text);

                int m = 2, t = 2;

                BigInteger[] v = {0,BigInteger.Parse(txtGTV1.Text),
                            BigInteger.Parse(txtGTV2.Text)};
                BigInteger[] a = { 0, BigInteger.Parse(txtGTA1.Text) };

                for (int i = 1; i <= m; i++)
                {
                    BigInteger l = 0;
                    for (int j = t - 1; j > 0; j--)
                    {
                        //vong for = pow(vi, j)
                        BigInteger h = BigInteger.ModPow(v[i], j, P);

                        l = (l + (a[j] * h)) % P;
                    }
                    BigInteger y = (Key + l) % P;
                    kq += String.Format("(v{0}, f(v{1}) = ({2}, {3})\n", i, i, v[i], y);
                }
                kq += String.Format("Cần 2/2 cặp (vj, f(vj)) để khôi phục khóa\n");
                richTextBoxChia.Text = kq;
            }
        }

        private void btnGhepKhoa_Click(object sender, EventArgs e)
        {
            bool check = true;
            BigInteger checkP;
            if (String.IsNullOrEmpty(txtPGhep.Text) || String.IsNullOrEmpty(txtV1.Text)
                || String.IsNullOrEmpty(txtV2.Text) || String.IsNullOrEmpty(txtFv1.Text)
                || String.IsNullOrEmpty(txtFv2.Text))
            {
                check = false;
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin để ghép khóa!", "Lỗi");
            }

            else if (BigInteger.TryParse(txtPGhep.Text, out checkP) == false || !IsPrime(checkP))
            {
                check = false;
                MessageBox.Show("Vui lòng nhập p phải là một số nguyên tố!", "Lỗi");
            }

            if (check)
            {
                BigInteger p = BigInteger.Parse(txtPGhep.Text);
                BigInteger[] v = {0,BigInteger.Parse(txtV1.Text),
                            BigInteger.Parse(txtV2.Text)};

                BigInteger[] f = {0,BigInteger.Parse(txtFv1.Text),
                            BigInteger.Parse(txtFv2.Text)};
                BigInteger k = 0;

                //txtKhoaKPHexa.Text = f[2].ToString();
                int t = 2;
                for (int i = 1; i <= t; i++)
                {
                    BigInteger m = 1;
                    for (int j = 1; j <= t; j++)
                    {
                        if (j != i)
                        {
                            BigInteger b = v[j] - v[i];
                            BigInteger n = mul_inv(b, p) % p;
                            n = (v[j] * n) % p;
                            m = (m * n) % p;
                        }
                    }
                    //txtKhoaKPHexa.Text = m.ToString();
                    k = (k % p + (f[i] % p * m % p) % p) % p;
                }
                txtKhoaKP.Text = k.ToString();
                //txtKhoaKPHexa.Text = k.ToString("X");
            }
        }

        // Các hàm gộp khóa 
        private static BigInteger mul_inv(BigInteger a, BigInteger b) //a^-1 mod b
        {
            BigInteger b0 = b, t, q;
            BigInteger x0 = 0, x1 = 1;
            if (b == 1) return 1;
            while (a < 0) a += b;
            while (a > 1)
            {
                q = (a / b);
                t = b; b = a % b; a = t;
                t = x0; x0 = x1 - q * x0; x1 = t;
            }
            if (x1 < 0) x1 += b0;
            return x1;
        }
        // Miller rabbin
        public static bool IsPrime(BigInteger n)
        {
            if (n < 2)
                return false;
            if (n == 2 || n == 3 || n == 5 || n == 7)
                return true;
            if (n % 2 == 0)
                return false;

            BigInteger bn = n; // converting to BigInteger here to avoid converting up to 48 times below
            BigInteger n1 = bn - 1;
            int r = 1;
            BigInteger d = n1;
            while (d.IsEven)
            {
                r++;
                d >>= 1;
            }
            if (!Witness(2, r, d, bn, n1)) return false;
            if (!Witness(3, r, d, bn, n1)) return false;
            if (!Witness(5, r, d, bn, n1)) return false;
            if (!Witness(7, r, d, bn, n1)) return false;
            if (!Witness(11, r, d, bn, n1)) return false;
            if (n < 2152302898747) return true;
            if (!Witness(13, r, d, bn, n1)) return false;
            if (n < 3474749660383) return true;
            if (!Witness(17, r, d, bn, n1)) return false;
            if (n < 341550071728321) return true;
            if (!Witness(19, r, d, bn, n1)) return false;
            if (!Witness(23, r, d, bn, n1)) return false;
            if (n < 3825123056546413051) return true;
            return Witness(29, r, d, bn, n1)
                   && Witness(31, r, d, bn, n1)
                   && Witness(37, r, d, bn, n1);
        }

        // a single instance of the Miller-Rabin Witness loop
        private static bool Witness(BigInteger a, int r, BigInteger d, BigInteger n, BigInteger n1)
        {
            var x = BigInteger.ModPow(a, d, n);
            if (x == BigInteger.One || x == n1) return true;

            while (r > 1)
            {
                x = BigInteger.ModPow(x, 2, n);
                if (x == BigInteger.One) return false;
                if (x == n1) return true;
                r--;
            }
            return false;
        }

        private void btnChiaKhoa_Click_1(object sender, RoutedEventArgs e)
        {
            bool check = true;
            BigInteger checkP;
            if (String.IsNullOrEmpty(txtKChia.Text) || String.IsNullOrEmpty(txtPChia.Text)
                || String.IsNullOrEmpty(txtGTV1.Text) || String.IsNullOrEmpty(txtGTV2.Text)
                || String.IsNullOrEmpty(txtGTA1.Text))
            {
                check = false;
                MessageBox.Show("Vui lòng nhập lòng nhập đầy đủ thông tin để chia khóa!", "Lỗi");
            }

            else if (BigInteger.TryParse(txtPChia.Text, out checkP) == false || !IsPrime(checkP))
            {
                check = false;
                MessageBox.Show("Vui lòng nhập p phải là một số nguyên tố!", "Lỗi");
            }

            if (check)
            {
                string kq = "Các cặp khóa: \n";
                BigInteger Key = BigInteger.Parse(txtKChia.Text);
                BigInteger P = BigInteger.Parse(txtPChia.Text);

                int m = 2, t = 2;

                BigInteger[] v = {0,BigInteger.Parse(txtGTV1.Text),
                            BigInteger.Parse(txtGTV2.Text)};
                BigInteger[] a = { 0, BigInteger.Parse(txtGTA1.Text) };

                for (int i = 1; i <= m; i++)
                {
                    BigInteger l = 0;
                    for (int j = t - 1; j > 0; j--)
                    {
                        //vong for = pow(vi, j)
                        BigInteger h = BigInteger.ModPow(v[i], j, P);

                        l = (l + (a[j] * h)) % P;
                    }
                    BigInteger y = (Key + l) % P;
                    kq += String.Format("(v{0}, f(v{1}) = ({2}, {3})\n", i, i, v[i], y);
                }
                kq += String.Format("Cần 2/2 cặp (vj, f(vj)) để khôi phục khóa\n");
                richTextBoxChia.Text = kq;
            }
        }

        private void btnGhepKhoa_Click_1(object sender, RoutedEventArgs e)
        {
            bool check = true;
            BigInteger checkP;
            if (String.IsNullOrEmpty(txtPGhep.Text) || String.IsNullOrEmpty(txtV1.Text)
                || String.IsNullOrEmpty(txtV2.Text) || String.IsNullOrEmpty(txtFv1.Text)
                || String.IsNullOrEmpty(txtFv2.Text))
            {
                check = false;
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin để ghép khóa!", "Lỗi");
            }

            else if (BigInteger.TryParse(txtPGhep.Text, out checkP) == false || !IsPrime(checkP))
            {
                check = false;
                MessageBox.Show("Vui lòng nhập p phải là một số nguyên tố!", "Lỗi");
            }

            if (check)
            {
                BigInteger p = BigInteger.Parse(txtPGhep.Text);
                BigInteger[] v = {0,BigInteger.Parse(txtV1.Text),
                            BigInteger.Parse(txtV2.Text)};

                BigInteger[] f = {0,BigInteger.Parse(txtFv1.Text),
                            BigInteger.Parse(txtFv2.Text)};
                BigInteger k = 0;

                //txtKhoaKPHexa.Text = f[2].ToString();
                int t = 2;
                for (int i = 1; i <= t; i++)
                {
                    BigInteger m = 1;
                    for (int j = 1; j <= t; j++)
                    {
                        if (j != i)
                        {
                            BigInteger b = v[j] - v[i];
                            BigInteger n = mul_inv(b, p) % p;
                            n = (v[j] * n) % p;
                            m = (m * n) % p;
                        }
                    }
                    //txtKhoaKPHexa.Text = m.ToString();
                    k = (k % p + (f[i] % p * m % p) % p) % p;
                }
                txtKhoaKP.Text = k.ToString();
                //txtKhoaKPHexa.Text = k.ToString("X");
            }
        }
    }
}
