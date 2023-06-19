using System.Windows;
using Ma_Hoa_DES.DocFile;
using Ma_Hoa_DES.MaHoa;
using Microsoft.Win32;
using System.IO;

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
    }
}
