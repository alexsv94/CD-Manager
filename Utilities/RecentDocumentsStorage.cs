using Newtonsoft.Json;
using OrganizerWpf.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Utilities
{
    [Serializable]
    public sealed class RecentDocumentsStorage
    {
        public static RecentDocumentsStorage Instance { get; private set; } = new();
        public List<DocumentModel> RecentDocuments { get; private set; } = new();

        public static void AddDocument(DocumentModel doc)
        {
            if (Instance.RecentDocuments.Count >= 100)
            {
                Instance.RecentDocuments.Remove(Instance.RecentDocuments.Last());                
            }
            
            if (Instance.RecentDocuments.Count > 0)
            {
                var candidate = Instance.RecentDocuments.FirstOrDefault(x => x.ShortName == doc.ShortName);

                if (candidate != null)
                {
                    candidate = doc;
                }
                else
                {
                    Instance.RecentDocuments.Insert(0, doc);
                }
            }
            else
            {
                Instance.RecentDocuments.Insert(0, doc);
            } 

            Save();
        }

        public static void RemoveDocument(DocumentModel doc)
        {
            if (Instance.RecentDocuments.Count == 0) return;

            var docFromList = Instance.RecentDocuments.FirstOrDefault(x => x.ShortName == doc.ShortName);
            if (docFromList != null) 
                Instance.RecentDocuments.Remove(docFromList);

            Save();
        }

        public static void ChangeDocumentParameters(DocumentModel doc)
        {
            if (Instance.RecentDocuments.Count == 0) return;

            var docFromList = Instance.RecentDocuments.FirstOrDefault(x => x.ShortName == doc.ShortName);
            docFromList ??= doc;

            Save();
        }

        public static void Load()
        {
            if (!File.Exists("RecentDocuments.json"))
            {
                Instance = new();
                return;
            }                

            string jsonString = File.ReadAllText("RecentDocuments.json");
            Instance = JsonConvert.DeserializeObject<RecentDocumentsStorage>(jsonString) ?? new();
        }

        public static void Save()
        {
            if (File.Exists("RecentDocuments.json"))
            {
                File.Delete("RecentDocuments.json");
            }

            File.WriteAllText("RecentDocuments.json", JsonConvert.SerializeObject(Instance, Formatting.Indented));
        }
    }
}
