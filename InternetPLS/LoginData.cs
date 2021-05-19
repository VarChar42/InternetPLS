using System.IO;
using System.Xml.Serialization;

namespace InternetPLS
{
    public class LoginData
    {
        private const string FilePath = "autologin.cfg";

        public string Username { get; set; }

        public string Password { get; set; }


        public void Save()
        {
            var xmlSerializer = new XmlSerializer(typeof(LoginData));
            var streamWriter = new StreamWriter(FilePath);
            xmlSerializer.Serialize(streamWriter, this);
            streamWriter.Close();
        }

        public static LoginData Read()
        {
            var xmlSerializer = new XmlSerializer(typeof(LoginData));
            var fileStream = new FileStream(FilePath, FileMode.Open);
            var data = (LoginData) xmlSerializer.Deserialize(fileStream);
            fileStream.Close();
            return data;
        }


        public LoginData Decrypt()
        {
            var data = new LoginData
            {
                Username = StringCipher.Decrypt(Username, "42"),
                Password = StringCipher.Decrypt(Password, "42")
            };

            return data;
        }

        public static LoginData Encrypt(string username, string pw)
        {
            var data = new LoginData
            {
                Username = StringCipher.Encrypt(username, "42"),
                Password = StringCipher.Encrypt(pw, "42")
            };

            return data;
        }
    }
}