namespace Loupedeck.WebhooksPlugin.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using HarSharp;

    public class HarFileService
    {

        public List<Entry> Webhooks { get; private set; }

        private static string FULL_PATH { get => Path.Combine(WebhooksPlugin.UserProfilePath, WebhooksPlugin.DEFAULT_PATH); }

        private FileSystemWatcher _fileWatcher = null;

        public HarFileService(params string[] paths)
        {
            if (this.Webhooks == null)
            {
                this.Webhooks = new List<Entry>();
            }

            this.Init(paths);

            this._fileWatcher = new FileSystemWatcher(FULL_PATH);

            this._fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size;
            this._setFileWatchHandlers();
            this._fileWatcher.Filter = "*.har";
            this._fileWatcher.IncludeSubdirectories = true;
            this._fileWatcher.EnableRaisingEvents = true; 
        }
        private void _setFileWatchHandlers()
        {
            this._fileWatcher.Created += this.OnFileChange;
            this._fileWatcher.Changed += this.OnFileChange;
            this._fileWatcher.Deleted += this.OnFileChange;
        }

        private void _unsetFileWatchHanders()
        {
            this._fileWatcher.Created -= this.OnFileChange;
            this._fileWatcher.Changed -= this.OnFileChange;
            this._fileWatcher.Deleted -= this.OnFileChange;
        }
    

        ~HarFileService()
        {
            if (this._fileWatcher != null)
            {
                this._unsetFileWatchHanders();
                this._fileWatcher.Dispose();
                this._fileWatcher = null;
            }                
        }

        public void OnFileChange(object sender, FileSystemEventArgs e)
        {
            this._unsetFileWatchHanders();
            this.Webhooks.Clear();
            System.Threading.Tasks.Task.Delay(150).Wait();
            this.Webhooks.AddRange(this._loadFromPath(FULL_PATH));
            System.Threading.Tasks.Task.Delay(150).Wait();
            this._setFileWatchHandlers();
        }

        private void Init(string[] paths)
        {
            this.Webhooks.AddRange(this._loadFromPath(FULL_PATH));

            if (paths.Length >= 0)
            {
                paths.ToList().ForEach(path =>
                {
                    this.Webhooks.AddRange(this._loadFromPath(path));
                });
            }
        }

        private IList<Entry> _loadFromPath(string path)
        {
            var entries = new List<Entry>();
            var harFiles = System.IO.Directory.GetFiles(path, "*.har");

            foreach (var file in harFiles)
            {
                if (file == null) continue; 
                entries.AddRange(HarConvert.DeserializeFromFile(file).Log.Entries);
            }

            return entries;
        }
    }
}
