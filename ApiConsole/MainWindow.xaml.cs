using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;  // must install Newtonsoft package from Nuget
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;

namespace ApiConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath_;
        private string bookId_;
        private string bookName_;
        private string bookDescription_;
        private string baseUrl_;
        public HttpClient client { get; set; }

        public MainWindow()
        {
            client = new HttpClient();
            baseUrl_= "https://localhost:44333/api/Files";
            InitializeComponent();
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            if (filePath_ == null||bookId_==null||System.IO.Path.GetExtension(filePath_)!=".pdf")
                return;
            Task<HttpResponseMessage> tup = replaceFile();
            clear();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (bookId_ == null)
                return;
            Task<HttpResponseMessage> tup = deleteFile();
            clear();
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            if (bookName_ == null || bookDescription_ == null || filePath_ == null)
                return ;
            Task<HttpResponseMessage> tup=uploadFile();
            clear();

        }
        private async Task<HttpResponseMessage> uploadFile()
        {
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            byte[] data = File.ReadAllBytes(filePath_);
            ByteArrayContent bytes = new ByteArrayContent(data);
            string fileName = System.IO.Path.GetFileName(filePath_);
            multiContent.Add(bytes, "files", fileName);
            multiContent.Add(new StringContent(bookName_), "name");
            multiContent.Add(new StringContent(bookDescription_), "description");
            return await client.PostAsync(baseUrl_, multiContent);
        }
        private async Task<HttpResponseMessage> replaceFile()
        {
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            byte[] data = File.ReadAllBytes(filePath_);
            ByteArrayContent bytes = new ByteArrayContent(data);
            string fileName = System.IO.Path.GetFileName(filePath_);
            multiContent.Add(bytes, "files", fileName);
            multiContent.Add(new StringContent(bookId_), "id");
            return await client.PostAsync(baseUrl_+"/replace", multiContent);
        }

        private async Task<HttpResponseMessage> deleteFile()
        {
            var ret=await client.DeleteAsync(baseUrl_+'/'+bookId_);
            return ret;

        }

        private void BookName_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            bookName_ = bookName.Text.ToString();
            Console.WriteLine(bookName_);
        }

        private void BookDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            bookDescription_ = bookDescription.Text.ToString();
        }

        private void BookId_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                bookId_ = bookId.Text.ToString();
                Console.WriteLine(bookId_);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input");
            }
        }

        private void BookPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            filePath_ = bookPath.Text.ToString();
            Console.WriteLine(filePath_);
        }

        private void FilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            filePath_ = filePath.Text.ToString();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            if(fileDialog.ShowDialog()== System.Windows.Forms.DialogResult.OK)
            {
                filePath_ = fileDialog.FileName;
                bookPath.Text = filePath_;
            }
        }

        private void SelectPath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath_ = fileDialog.FileName;
                filePath.Text = filePath_;
            }

        }
        private void clear()
        {
            bookName_ = null;
            filePath_ = null;
            bookDescription_ = null;
            bookName.Clear();
            bookPath.Clear();
            filePath.Clear();
            bookDescription.Clear();
            bookId.Clear();
        }
    }
}
