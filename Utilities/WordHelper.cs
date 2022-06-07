using OrganizerWpf.StylizedControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public bool Process(Dictionary<string, string> items, string newFileName, Dictionary<string, string[]>? combinedItems = null, AsyncOperation? operation = null)
        {
            Word.Application? app = null;

            if (operation != null)
            {
                ConfigureOperation(items, operation, combinedItems);
            }

            try
            {
                app = new Word.Application();
                object file = _templateFileInfo.FullName;

                Word.Document document = app.Documents.Open(file);

                for (int i = 0; i < items.Count; i++)
                {
                    FindAndReplaceText(app.ActiveDocument, items.ElementAt(i).Key, items.ElementAt(i).Value);
                    if (operation != null) operation.CompletedStepsCount += 1;
                }

                if (combinedItems != null)
                {
                    foreach (var item in combinedItems)
                    {
                        for (int i = 0; i < item.Value.Length; i++)
                        {
                            FindAndReplaceText(app.ActiveDocument, item.Key, item.Value[i], i != item.Value.Length - 1);
                            if (operation != null) operation.CompletedStepsCount += 1;
                        }
                    }
                }

                SaveDocument(document, newFileName);
                app.ActiveDocument.Close();

                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
            finally
            {
                if (app != null)
                {
                    app.Quit(SaveChanges: false);
                    if (operation != null) operation.CompletedStepsCount = operation.TotalStepsCount;
                }
            }            
        }

        private void FindAndReplaceText(Word.Document document, string text, string replaceWithText, bool addKeyAfterReplacement = false)
        {
            object wrap = Word.WdFindWrap.wdFindContinue;
            object replace = Word.WdReplace.wdReplaceAll;

            object missing = Type.Missing;

            foreach (Word.Range rng in document.StoryRanges)
            {
                rng.Find.Text = text;
                rng.Find.Replacement.Text = replaceWithText;

                if (addKeyAfterReplacement)
                    rng.Find.Replacement.Text += text;

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

        private void ConfigureOperation(Dictionary<string, string> items, AsyncOperation operation, Dictionary<string, string[]>? combinedItems = null)
        {
            operation.TotalStepsCount += items.Count;

            if (combinedItems != null)
            {
                foreach (var item in combinedItems)
                {
                    operation.TotalStepsCount += item.Value.Length;
                }
            }
        }
    }
}