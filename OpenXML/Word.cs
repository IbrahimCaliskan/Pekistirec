using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Packaging;
using System.IO;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using System.Text;

namespace OpenXML
{
    public class Word : MyInterfaces.DokumanInterfaces.IDokumanReadAndEditTool
    {
        public bool Kontrol(string FilePath)
        {
            bool s = false;
            if (File.Exists(FilePath))
                try
                {
                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(FilePath, true))
                    {
                        string docText = null;
                        using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                        {
                            docText = sr.ReadToEnd();
                        }

                        if (String.IsNullOrEmpty(docText))
                        {
                            s = false;
                        }
                        else
                        {
                            s = true;
                        }

                    }
                }
                catch (Exception)
                {
                    s = false;
                }

            return s;

        }

        public List<string> ParametreAra(string FilePath, string baslangicText, string bitisText, out string docText)
        {
            List<string> parametreList = new List<string>();

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(FilePath, true))
            {
                string documentText = "";
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    documentText = sr.ReadToEnd();
                }
                int birOncekiBaslangicIndex = 0;
                do
                {
                    int baslangicIndex = documentText.IndexOf(baslangicText, birOncekiBaslangicIndex) + baslangicText.Length;
                    if (baslangicIndex < 0)
                    {
                        break;
                    }

                    int bitisIndex = documentText.IndexOf(bitisText, baslangicIndex);
                    if (bitisIndex < 0)
                    {
                        break;
                    }

                    birOncekiBaslangicIndex = bitisIndex + bitisText.Length;

                    string parametre = documentText.Substring(baslangicIndex, bitisIndex - baslangicIndex);
                    if (parametre.Length < 51)
                    {
                        parametreList.Add(baslangicText + parametre + bitisText);
                    }

                } while (true);


                StringBuilder sb = new StringBuilder();
                OpenXmlElement element = wordDoc.MainDocumentPart.Document.Body;
                if (element == null)
                {
                    docText = string.Empty;
                }
                sb.Append(GetPlainText(element));
                docText = sb.ToString();
            }

            return parametreList.Distinct().ToList();
        }

        /// <summary> 
        ///  Read Plain Text in all XmlElements of word document 
        /// </summary> 
        /// <param name="element">XmlElement in document</param> 
        /// <returns>Plain Text in XmlElement</returns> 
        private string GetPlainText(OpenXmlElement element)
        {
            StringBuilder PlainTextInWord = new StringBuilder();
            foreach (OpenXmlElement section in element.Elements())
            {
                switch (section.LocalName)
                {
                    // Text 
                    case "t":
                        PlainTextInWord.Append(section.InnerText);
                        break;


                    case "cr":                          // Carriage return 
                    case "br":                          // Page break 
                        PlainTextInWord.Append(Environment.NewLine);
                        break;


                    // Tab 
                    case "tab":
                        PlainTextInWord.Append("\t");
                        break;


                    // Paragraph 
                    case "p":
                        PlainTextInWord.Append(GetPlainText(section));
                        PlainTextInWord.AppendLine(Environment.NewLine);
                        break;


                    default:
                        PlainTextInWord.Append(GetPlainText(section));
                        break;
                }
            }


            return PlainTextInWord.ToString();
        }

        public void FindAndReplace(string FilePath, Dictionary<string, string> findReplaceList)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(FilePath, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (var item in findReplaceList)
                {
                    Regex regexText = new Regex(item.Key);
                    docText = regexText.Replace(docText, item.Value);
                }

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }
    }
}