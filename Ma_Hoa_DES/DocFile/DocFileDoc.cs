using Ma_Hoa_DES.MaHoa;
using Xceed.Words.NET;
using System.IO;
using GemBox.Document;

namespace Ma_Hoa_DES.DocFile
{
    class DocFileDoc
    {
        public static string FileReadToString(string filePath)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            DocumentModel document = DocumentModel.Load(filePath);

            string text = document.Content.ToString();

            return text;
        }
        //public string FileReadToString(string filePath)
        //{
        //    using (DocX doc = DocX.Load(filePath))
        //    {
        //        string text = doc.Text;
        //        return text;
        //    }
        //}
        public static void WriteStringToFile(string filePath, string text)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            DocumentModel document = new DocumentModel();
            Section section = new Section(document);
            document.Sections.Add(section);

            Paragraph paragraph = new Paragraph(document);
            section.Blocks.Add(paragraph);

            paragraph.Content.LoadText(text);

            document.Save(filePath);
        }
        //public void WriteStringToFile(string filePath, string text)
        //{
        //    using (DocX doc = DocX.Create(filePath))
        //    {
        //        // Thêm nội dung từ chuỗi text vào tài liệu Word
        //        doc.InsertParagraph().Append(text);

        //        // Lưu tài liệu Word vào tệp tin
        //        doc.Save();
        //    }
        //}
    }
}
