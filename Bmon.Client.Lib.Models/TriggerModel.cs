using System;
using System.Collections.Generic;

namespace Bmon.Client.Lib.Models
{
    public class TriggerModel
    {
        public Guid Id;
        public string LocalDir;
        public string LocalFile;
        public TriggerUpload LocalFileTrigger;

        public TriggerModel()
        {
            Id = new Guid();
            LocalDir = string.Empty;
            LocalFile = string.Empty;
            LocalFileTrigger = TriggerUpload.Uninitialized;
        }

        public TriggerModel(string localDir, string localFile, TriggerUpload localTrigger)
        {
            Id = Guid.NewGuid();
            LocalDir = localDir;
            LocalFile = localFile;
            LocalFileTrigger = localTrigger;
        }

        public enum TriggerUpload
        {
            OnFileSystemCommit,
            Uninitialized
        }
    }
}
