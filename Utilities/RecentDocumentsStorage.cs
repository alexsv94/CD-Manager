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
            Instance.RecentDocuments.Insert(0, doc);
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
