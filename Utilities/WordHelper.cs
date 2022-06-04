using OrganizerWpf.StylizedControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace OrganizerWpf.Utilities
{
    class WordHelper
    {
        private readonly FileInfo _templateFileInfo;

        public WordHelper(string templateFileName)
        {
            if (File.Exists(templateFileName))
            {
                _templateFileInfo = new FileInfo(templateFileName);
            }
            else
            {
                throw new ArgumentException("File not found");
            }
        }

        public bool Process(Dictionary<string, string> items, string newFileName)
        {
            Word.Application? app = null;

            try
            {
                app = new Word.Application();
                object file = _templateFileInfo.FullName;

                Word.Document document = app.Documents.Open(file);

                foreach (var item in items)
                {
                   FindAndReplaceText(app.ActiveDocument, item.Key, item.Value);
                }
                FindAndReplaceText(app.ActiveDocument, "^p ", "^p");

                SaveDocument(document, newFileName);
                app.ActiveDocument.Close();

                return true;
            }
            catch (Exception ex) 
            {
                SCMessageBox.ShowMsgBox(ex.Message);
                return false;
            }
            finally
            {
                if (app != null)
                {
                    app.Quit();
                }
            }            
        }

        private void FindAndReplaceText(Word.Document document, string text, string replaceWithText)
        {
            object wrap = Word.WdFindWrap.wdFindContinue;
            object replace = Word.WdReplace.wdReplaceAll;

            object missing = Type.Missing;

            foreach (Word.Range rng in document.StoryRanges)
            {
                rng.Find.Text = text;
                rng.Find.Replacement.Text = replaceWithText;
               
                rng.Find.Execute(
                    FindText: missing,
                    MatchCase: false,
                    MatchWholeWord: false,
                    MatchWildcards: false,
                    MatchSoundsLike: Type.Missing,
                    MatchAllWordForms: false,
                    Forward: true,
                    Wrap: wrap,
                    Format: false,
                    ReplaceWith: missing,
                    Replace: replace);
            }
        }

        private void SaveDocument(Word.Document document, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                document.SaveAs2(fileName);
            }
            else
            {
                document.SaveAs2(fileName);
            }
        }
    }
}
