using System;
using System.Collections.Generic;

namespace Bmon.Client.Lib.Models
{
    public class DevourModel
    {
        public Guid Id;
        public string LocalDir;
        public string LocalFile;
        public FilePattern LocalFilePattern;
        public List<Guid> UploadTo;
        public List<Guid> TriggerOn;

        public DevourModel()
        {
            Id = new Guid();
            LocalDir = string.Empty;
            LocalFile = string.Empty;
            LocalFilePattern = FilePattern.Uninitialized;
            UploadTo = new List<Guid>();
            TriggerOn = new List<Guid>();
        }

        public DevourModel(string localDir, string localFile, FilePattern localFilePattern)
        {
            Id = Guid.NewGuid();
            LocalDir = localDir;
            LocalFile = localFile;
            LocalFilePattern = localFilePattern;
            UploadTo = new List<Guid>();
            TriggerOn = new List<Guid>();
        }
    }

    public enum FilePattern
    {
        Absolute,
        RegEx,
        Uninitialized
    }

    public enum FileAction
    {
        AbortIfExist,
        OverwriteIfExist,
        Uninitialized
    }
}
