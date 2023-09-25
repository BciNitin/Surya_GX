using System.IO;

namespace MobiVUE.Utility.DataStructures
{
    public class UserFile : BusinessBase
    {
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { SetValue<string>(ref _fileName, value, () => this.FileName); }
        }

        public string FileNameWithoutExt
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_fileName)) return "";
                return Path.GetFileNameWithoutExtension(_fileName);
            }
        }

        private byte[] _fileData;

        public byte[] FileData
        {
            get { return _fileData; }
            set { SetValue<byte[]>(ref _fileData, value, () => this.FileData); }
        }
    }
}