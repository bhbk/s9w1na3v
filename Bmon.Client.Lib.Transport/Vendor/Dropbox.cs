using Bmon.Client.Lib.Models;
using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://blogs.dropbox.com/developers/2015/06/introducing-a-preview-of-the-new-dropbox-net-sdk-for-api-v2/
namespace Bmon.Client.Lib.Transport.Vendor
{
    public class Dropbox
    {
        private DropboxClient Session;
        private StringBuilder StandardOutput;

        public StringBuilder Output
        {
            get
            {
                return StandardOutput;
            }
        }

        public Dropbox(string token)
        {
            /*
             * need to see if there is a way to get token, in code, using username/password
             * that users enter so that application setup doesn't require normal users to visit
             * https://www.dropbox.com/developers/apps/create to set up app.
             */
            Session = new DropboxClient(token);
            StandardOutput = new StringBuilder();
        }

        public async void GetUserInfoAsync()
        {
            try
            {
                var acct = await Session.Users.GetCurrentAccountAsync();

                StandardOutput.Append(string.Format("Name: {0} Email: {1}", acct.Name.DisplayName, acct.Email));
            }
            catch (Exception ex)
            {
                StandardOutput.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public async Task GetFolderContentsAsync(string remotePath)
        {
            try
            {
                var response = await Session.Files.ListFolderAsync(remotePath);

                foreach (var item in response.Entries.Where(i => i.IsFolder))
                    StandardOutput.Append(string.Format("D {0}/", item.Name));

                foreach (var item in response.Entries.Where(i => i.IsFile))
                    StandardOutput.Append(string.Format("F{0,8} {1}", item.AsFile.Size, item.Name));
            }
            catch (Exception ex)
            {
                StandardOutput.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public async Task DownloadFileAsync(string remotePath, string remoteFile, string localPath, string localFile, FileAction action)
        {
            try
            {
                if (File.Exists(localPath + @"\" + localFile) && action == FileAction.OverwriteIfExist
                    || (!File.Exists(localPath + @"\" + localFile)))
                {
                    using (var response = await Session.Files.DownloadAsync(remotePath + @"/" + remoteFile))
                    {
                        FileStream fs = File.Create(localPath + @"\" + localFile);
                        (await response.GetContentAsStreamAsync()).CopyTo(fs);

                        StandardOutput.Append(string.Format("Transfer success for {0}", localPath + @"\" + localFile));
                    }
                }
            }
            catch (Exception ex)
            {
                StandardOutput.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public async Task UploadFileAsync(string localPath, string localFile, string remotePath, string remoteFile, FileAction action)
        {
            try
            {
                using (FileStream fs = File.OpenRead(localPath + @"\" + localFile))
                {
                    MemoryStream ms = new MemoryStream();
                    await fs.CopyToAsync(ms);

                    /*
                     * add check for remote file before doing upload
                     */

                    var result = await Session.Files.UploadAsync(localPath + "/" + localFile, WriteMode.Overwrite.Instance, body: ms);

                    StandardOutput.Append(string.Format("Transfer success for {0} revision {1}", localPath + @"/" + localFile, result.Rev));
                }
            }
            catch (Exception ex)
            {
                StandardOutput.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }
    }
}
