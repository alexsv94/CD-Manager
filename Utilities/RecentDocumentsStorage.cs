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

        private List<DocumentModel> _recentDocuments { get; set; } = new();

        public static void AddDocument(DocumentModel doc)
        {
            if (Instance._recentDocuments.Count >= 100)
            {
                Instance._recentDocuments.RemoveAt(Instance._recentDocuments.Count - 1);
            }
            Instance._recentDocuments.Insert(0, doc);
            Save();
        }

        public static List<DocumentModel> GetDocuments() => Instance._recentDocuments;

        public static void Load()
        {
            if (!File.Exists("RecentDocuments.json"))
            {
                Instance = new();
                return;
            }                

            string jsonString = File.ReadAllText("RecentDocuments.json");
            Instance = JsonConvert.DeserializeObject<RecentDocumentsStorage>(jsonString);
        }

        public static void Save()
        {
            if (File.Exists("RecentDocuments.json"))
            {
                File.Delete("RecentDocuments.json");
            }

            File.WriteAllText("RecentDocuments.json", JsonConvert.SerializeObject(Instance));
        }
    }
}
