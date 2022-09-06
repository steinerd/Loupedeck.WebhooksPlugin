namespace Loupedeck.WebhooksPlugin.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using HarSharp;

    /// <summary>
    /// Encapsulates HAR File Service members
    /// </summary>
    public class HarFileService
    {
        /// <summary>
        /// Gets the full path of the webhook HAR directory
        /// </summary>
        internal static string FULL_PATH => Path.Combine(WebhooksPlugin.UserProfilePath, WebhooksPlugin.DEFAULT_PATH);

        /// <summary>
        /// File Watcher for checking for file changes while plugin is running.
        /// </summary>
        internal FileSystemWatcher _fileWatcher = null;

        /// <summary>
        /// Gets IEnumerable of Webhook <see cref="Entry">Entries</see>
        /// </summary>
        public ConcurrentQueue<Entry> Webhooks { get; private set; }

        /// <summary>
        /// Thread-safe lock
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="paths">Paths for HAR files; optional, defaults to <see cref="FULL_PATH"/>.</param>
        public HarFileService(params string[] paths)
        {
            if (this.Webhooks == null)
            {
                lock (this._lock)
                {
                    this.Webhooks = new ConcurrentQueue<Entry>();
                }                
            }

            this.Init(paths);

            this._fileWatcher = new FileSystemWatcher(FULL_PATH);

            this._fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size;
            this._setFileWatchHandlers();
            this._fileWatcher.Filter = "*.har";
            this._fileWatcher.IncludeSubdirectories = true;
            this._fileWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// File change event handler
        /// </summary>
        /// <param name="sender">Object that requested event</param>
        /// <param name="e">File system event arguments</param>
        public void OnFileChange(object sender, FileSystemEventArgs e)
        {
            this._unsetFileWatchHanders();
            lock (this._lock)
            {
                this.Webhooks = new ConcurrentQueue<Entry>();
                this.LoadFromPath(FULL_PATH).ForEach(file => this.Webhooks.Enqueue(file));
            }            
            this._setFileWatchHandlers();
        }

        /// <summary>
        /// Default deconstructor. Unsets/disposes any unmanaged objects.
        /// </summary>
        ~HarFileService()
        {
            if (this._fileWatcher != null)
            {
                this._unsetFileWatchHanders();
                this._fileWatcher.Dispose();
                this._fileWatcher = null;
            }
        }

        /// <summary>
        /// Sets the file watch event handlers
        /// </summary>
        private void _setFileWatchHandlers()
        {
            this._fileWatcher.Created += this.OnFileChange;
            this._fileWatcher.Changed += this.OnFileChange;
            this._fileWatcher.Deleted += this.OnFileChange;
        }

        /// <summary>
        /// Unsets the file watch event handlers
        /// </summary>
        private void _unsetFileWatchHanders()
        {
            this._fileWatcher.Created -= this.OnFileChange;
            this._fileWatcher.Changed -= this.OnFileChange;
            this._fileWatcher.Deleted -= this.OnFileChange;
        }

        /// <summary>
        /// Initialize routine, seperated in case of decoupled invocation. 
        /// </summary>
        /// <param name="paths">HAR file paths</param>
        private void Init(string[] paths)
        {
            lock (this._lock)
            {
                this.LoadFromPath(FULL_PATH).ForEach(file => this.Webhooks.Enqueue(file));

                // Add aditionally specified HAR files as well
                if (paths.Length >= 0)
                {
                    paths.ToList().ForEach(path =>
                    {
                        this.LoadFromPath(path).ForEach(file => this.Webhooks.Enqueue(file));                        
                    });
                }
            }            
        }

        /// <summary>
        /// Routine for loading HAR files from specific path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<Entry> LoadFromPath(string path)
        {
            var entries = new List<Entry>();
            var harFiles = Directory.GetFiles(path, "*.har");

            foreach (var file in harFiles)
            {
                if (file == null)
                    continue;
                entries.AddRange(HarConvert.DeserializeFromFile(file).Log.Entries);
            }

            return entries;
        }
    }
}
