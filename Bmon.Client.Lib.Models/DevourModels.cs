using System;
using System.Collections.Generic;

namespace Bmon.Client.Lib.Models
{
    public class LocalFileConfig
    {
        public Guid Id;
        public string LocalDir;
        public string LocalFile;
        public FilePattern LocalFilePattern;
        public List<Guid> UploadTo;

        public LocalFileConfig()
        {
            Id = new Guid();
            LocalDir = string.Empty;
            LocalFile = string.Empty;
            LocalFilePattern = FilePattern.OnlyForInit;
            UploadTo = new List<Guid>();
        }

        public LocalFileConfig(string localDir, string localFile, FilePattern localFilePattern)
        {
            Id = Guid.NewGuid();
            LocalDir = localDir;
            LocalFile = localFile;
            LocalFilePattern = localFilePattern;
            UploadTo = new List<Guid>();
        }
    }

    public enum FilePattern
    {
        Absolute,
        RegEx,
        OnlyForInit
    }

    public enum FileAction
    {
        AbortIfExist,
        OverwriteIfExist,
        OnlyForInit
    }
}
