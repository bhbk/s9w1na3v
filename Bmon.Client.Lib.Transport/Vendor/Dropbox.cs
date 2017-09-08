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
        private DropboxClient session;
        private StringBuilder stdout;

        public StringBuilder Stdout
        {
            get
            {
                return stdout;
            }
        }

        public Dropbox(string token)
        {
            /*
             * need to see if there is a way to get token, in code, using username/password
             * that users enter so that application setup doesn't require normal users to visit
             * https://www.dropbox.com/developers/apps/create to set up app.
             */
            session = new DropboxClient(token);
            stdout = new StringBuilder();
        }

        public async void GetUserInfoAsync()
        {
            try
            {
                var acct = await session.Users.GetCurrentAccountAsync();

                stdout.Append(string.Format("Name: {0} Email: {1}", acct.Name.DisplayName, acct.Email));
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public async Task GetFolderContentsAsync(string remotePath)
        {
            try
            {
                var response = await session.Files.ListFolderAsync(remotePath);

                foreach (var item in response.Entries.Where(i => i.IsFolder))
                    stdout.Append(string.Format("D {0}/", item.Name));

                foreach (var item in response.Entries.Where(i => i.IsFile))
                    stdout.Append(string.Format("F{0,8} {1}", item.AsFile.Size, item.Name));
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
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
                    using (var response = await session.Files.DownloadAsync(remotePath + @"/" + remoteFile))
                    {
                        FileStream fs = File.Create(localPath + @"\" + localFile);
                        (await response.GetContentAsStreamAsync()).CopyTo(fs);

                        stdout.Append(string.Format("Transfer success for {0}", localPath + @"\" + localFile));
                    }
                }
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
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

                    var result = await session.Files.UploadAsync(localPath + "/" + localFile, WriteMode.Overwrite.Instance, body: ms);

                    stdout.Append(string.Format("Transfer success for {0} revision {1}", localPath + @"/" + localFile, result.Rev));
                }
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }
    }
}
