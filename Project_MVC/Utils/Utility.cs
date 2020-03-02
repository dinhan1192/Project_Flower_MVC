using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Project_MVC.Utils
{
    public static class Utility
    {
        private const string DATE_FORMAT = "dd/MM/yyyy";

        private const string DATE_TIME_FORMAT = "HH:mm:ss dd/MM/yyyy";

        public static string ValidateEmailAddress(string email)
        {
            string re = string.Empty;
            bool isValid = System.Text.RegularExpressions.Regex.IsMatch(email,
              @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            if (!isValid)
            {
                re = string.Format("Địa chỉ Email {0} của bạn không đúng. <br/>", email);
            }

            return re;
        }

        #region Getting value methods

        public static string GetDateString(DateTime? value, string format = DATE_FORMAT)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString(format);
        }

        private const char CHAR_ESCAPE = (char)13;
        public static void Decrypt(string cypherText, out int? empID, out int[] refID)
        {
            empID = null;
            refID = null;

            if (string.IsNullOrWhiteSpace(cypherText))
                return;

            byte[] arrData = Convert.FromBase64String(cypherText);
            string plainText = System.Text.Encoding.UTF8.GetString(arrData);
            string[] arrToken = plainText.Split(new char[] { '#' });


            if (arrToken.Length == 3)
            {
                int result = 0;
                if (int.TryParse(arrToken[0], out result))
                    empID = result;

                //if (int.TryParse(arrToken[2], out result))
                //    refID = result;

                string strRefID = arrToken[2];
                string[] strRefIDs = strRefID.Split(new char[] { CHAR_ESCAPE }, StringSplitOptions.RemoveEmptyEntries);
                refID = strRefIDs.Select(c => int.Parse(c)).ToArray();
            }
        }

        public static string GetDateTimeString(DateTime? value, string format = DATE_TIME_FORMAT)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString(format);
        }

        public static string GetDateMinuteString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("HH:mm dd/MM/yyyy");
        }

        public static string GetDateTimeString(DateTime? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.Value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string GetStringVND(decimal? value)
        {
            return GetString(value, 0);
        }

        public static string GetString(int? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.ToString();
        }

        public static string GetString(bool? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.ToString();
        }

        public static string GetStringIntFormat(int? value)
        {
            if (value == null)
                return string.Empty;
            else
                return string.Format("{0:N0}", value);
        }

        public static string GetString(decimal? value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.ToString();
        }

        public static string GetString(decimal? value, int numberDigit)
        {
            if (value == null)
                return string.Empty;
            else
                return value.ToString();
        }
        #endregion

        #region Conversion methods
        public static bool? GetNullableBool(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            bool bValue;
            if (bool.TryParse(value, out bValue))
                return bValue;

            return null;
        }

        public static int? GetNullableInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            int intValue;
            if (int.TryParse(value, out intValue))
                return intValue;

            return null;
        }

        public static int GetInt(string value)
        {
            return Convert.ToInt32(value);
        }

        public static decimal? GetNullableDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            decimal decValue;
            if (decimal.TryParse(value, out decValue))
                return decValue;

            return null;
        }

        public static DateTime? GetNullableDate(string value, string format = DATE_FORMAT)
        {
            DateTime dt;
            bool isValid = DateTime.TryParseExact(value, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
            if (!string.IsNullOrEmpty(value) && isValid)
            {
                return dt;
            }
            else
                return null;
        }

        /// <summary>
        /// Ex: number 4: 155000 -> 160000;154999 -> 150000;
        /// </summary>
        public static string GetRoundedString(decimal? value, int number = 4)
        {
            if (value == null)
                return null;

            decimal padding = (decimal)Math.Pow(10, number);
            Decimal d = Decimal.Divide(value.Value, padding);
            d = decimal.Round(d);
            d = d * padding;
            return d.ToString("N0");
        }

        /// <summary>
        /// Ex: number 4: 155000 -> 160000;154999 -> 150000;
        /// </summary>
        public static decimal? GetRoundedDecimal(decimal? value, int number = 4)
        {
            if (value == null)
                return null;

            decimal padding = (decimal)Math.Pow(10, number);
            Decimal d = Decimal.Divide(value.Value, padding);
            d = decimal.Round(d);
            d = d * padding;
            return d;
        }

        public static string DeleteAllSpace(string strInput)
        {
            return string.IsNullOrWhiteSpace(strInput) ? string.Empty : strInput.Replace(" ", string.Empty);
        }
        #endregion

        #region Dictionary
        public static string GetDictionaryValue<T>(Dictionary<T, string> dicInput, T key)
        {
            if (key != null && dicInput.ContainsKey(key))
                return dicInput[key];

            return string.Empty;
        }

        public static T GetDictionaryKey<T>(Dictionary<T, string> dicInput, string value)
        {
            if (!string.IsNullOrEmpty(value))
                return dicInput.FirstOrDefault(d => d.Value.Equals(value)).Key;

            return default(T);
        }
        #endregion

        #region Convert number 2 word
        public static string GetWordNumber(decimal? number)
        {
            if (number == null)
                return string.Empty;

            decimal positiveNumber = Math.Abs(number.Value);
            string strNumber = positiveNumber.ToString("#.00");
            string[] arrNumber = strNumber.Split(new char[] { '.', ',' });

            string word = GetWordNumber_ReadNumber(arrNumber[0]);
            string strScale = GetWordNumber_ReadNumber(arrNumber[1]);

            if (!string.IsNullOrWhiteSpace(strScale))
                word = word + " phẩy " + strScale;
            if (number < 0)
                word = "âm " + word;

            if (word.Length > 0)
                return word[0].ToString().ToUpper() + word.Substring(1);
            else
                return string.Empty;
        }

        public static string NullIfEmptyString(string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                return null;
            else
                return text.Trim();
        }

        private static string GetWordNumber_ReadNumber(string number)
        {
            string word = string.Empty;
            Dictionary<int, string> dicUnitName = new Dictionary<int, string>() { { 0, "tỷ" }, { 1, "nghìn" }, { 2, "triệu" } };
            List<string> lstUnit3 = GetWordNumber_GroupByLength(number, 3);
            string sperateWord = string.Empty;

            for (int index = 0; index < lstUnit3.Count; index++)
            {
                string unit = lstUnit3[index];
                string unitWord = GetWordNumber_ReadThousand(unit);

                int remainIndex = lstUnit3.Count - index - 1;
                int unitIndex = remainIndex % 3;
                string unitUnit = (remainIndex == 0) ? string.Empty : dicUnitName[unitIndex];

                if (!string.IsNullOrWhiteSpace(unitWord))
                {
                    if (!string.IsNullOrWhiteSpace(unitUnit))
                        unitWord = unitWord + " " + unitUnit;

                    word = word + sperateWord + unitWord;
                    sperateWord = " ";
                }

                // truong hop don vi Ty ma khong co so doc dang sau (vd: 23 000 000 000 000)
                if (unitIndex == 0 && (remainIndex / 3) > 0 && string.IsNullOrWhiteSpace(unitWord))
                    word = word + " tỷ";
            }

            word = word.Trim();
            if (!string.IsNullOrWhiteSpace(word) && word.Length > 0 && char.IsLower(word[0]))
                word = char.ToUpper(word[0]).ToString() + word.Substring(1);

            return word;
        }

        private static List<string> GetWordNumber_GroupByLength(string number, int length)
        {
            List<string> lstUnit = new List<string>();
            string unit = string.Empty;
            for (int index = 0; index < number.Length; index++)
            {
                int remainIndex = number.Length - index;
                if (index > 0 && (remainIndex % length == 0))
                {
                    lstUnit.Add(unit);
                    unit = string.Empty;
                }

                unit = unit + number[index].ToString();
            }
            if (!string.IsNullOrWhiteSpace(unit))
                lstUnit.Add(unit);

            return lstUnit;
        }

        private static string GetWordNumber_ReadThousand(string number)
        {
            Dictionary<char, string> dicNumber = new Dictionary<char, string>()
            {
                {'0', "không"}, {'1', "một"}, {'2', "hai"}, {'3', "ba"}, {'4', "bốn"},
                {'5', "năm"}, {'6', "sáu"}, {'7', "bảy"}, {'8', "tám"}, {'9', "chín"}
            };

            switch (number.Length)
            {
                case 1:
                case 2:
                    return GetWordNumber_ReadHundred(number);
                case 3:
                    if (number == "000")
                        return string.Empty;

                    return dicNumber[number[0]] + " trăm " + GetWordNumber_ReadHundred(number.Substring(1));
                default:
                    return string.Empty;
            }
        }

        private static string GetWordNumber_ReadHundred(string number)
        {
            Dictionary<char, string> dicNumber = new Dictionary<char, string>()
            {
                {'0', "lẻ"}, {'1', "một"}, {'2', "hai"}, {'3', "ba"}, {'4', "bốn"},
                {'5', "năm"}, {'6', "sáu"}, {'7', "bảy"}, {'8', "tám"}, {'9', "chín"}
            };

            switch (number.Length)
            {
                case 1:
                    if (number[0] == '0')
                        return string.Empty;

                    return dicNumber[number[0]];
                case 2:
                    if (number == "00")
                        return string.Empty;

                    if (number == "10")
                        return "mười";

                    if (number[1] == '0')
                        return dicNumber[number[0]] + " mươi";

                    if (number[0] == '1')
                        return " mười " + dicNumber[number[1]];

                    if (number[0] == '0')
                        return dicNumber[number[0]] + " " + dicNumber[number[1]];

                    return dicNumber[number[0]] + " mươi " + dicNumber[number[1]];
                default:
                    return string.Empty;
            }
        }
        #endregion

        public static string EncodeImageParam(int? attID, string ecmID, string docCode,
                                                string fileName, string des, int? cdDetailID, int? refType, bool isEdit = false)
        {
            string anyString = Guid.NewGuid().ToString();
            string plainText = string.Format("{0}#{1}#{2}#{3}#{4}#{5}#{6}#{7}#{8}",
                 anyString, attID, ecmID, docCode, fileName, des, cdDetailID, refType, isEdit);

            byte[] arrData = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(arrData);
        }

        public static void DecodeImageParam(string cypherText,
          out int? attID, out string ecmID, out string docCode, out string fileName, out string des,
          out int? cdDetailID, out int? refType, out bool? isEdit)
        {
            attID = null;
            ecmID = null;
            docCode = null;
            fileName = null;
            isEdit = null;
            des = null;
            cdDetailID = null;
            refType = null;
            if (string.IsNullOrWhiteSpace(cypherText))
                return;

            byte[] arrData = Convert.FromBase64String(cypherText);
            string plainText = System.Text.Encoding.UTF8.GetString(arrData);
            string[] arrToken = plainText.Split(new char[] { '#' });

            if (arrToken.Length == 9)
            {
                int result = 0;

                if (int.TryParse(arrToken[1], out result))
                    attID = result;

                ecmID = arrToken[2];

                docCode = arrToken[3];
                fileName = arrToken[4];

                des = arrToken[5];

                if (int.TryParse(arrToken[6], out result))
                    cdDetailID = result;

                if (int.TryParse(arrToken[7], out result))
                    refType = result;

                bool isBool = false;
                if (bool.TryParse(arrToken[8], out isBool))
                    isEdit = isBool;
            }
        }

        public static string EncryptImageParam(int? empID,
           int? attID, string ecmID, int? maID, string docCode, string fileName,
           int? pvdID, string des, bool isEdit = false)
        {
            string anyString = Guid.NewGuid().ToString();
            string plainText = string.Format("{0}#{1}#{2}#{3}#{4}#{5}#{6}#{7}#{8}#{9}",
                empID, anyString, attID, ecmID, maID, docCode, fileName,
                pvdID, des, isEdit);

            byte[] arrData = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(arrData);
        }

        public static void DecryptImageParam(string cypherText, out int? empID,
           out int? attID, out string ecmID, out int? maID, out string docCode, out string fileName,
           out int? pvdID, out string des, out bool? isEdit)
        {
            empID = null;
            attID = null;
            ecmID = null;
            maID = null;
            docCode = null;
            fileName = null;
            pvdID = null;
            isEdit = null;
            des = null;

            if (string.IsNullOrWhiteSpace(cypherText))
                return;

            byte[] arrData = Convert.FromBase64String(cypherText);
            string plainText = System.Text.Encoding.UTF8.GetString(arrData);
            string[] arrToken = plainText.Split(new char[] { '#' });

            if (arrToken.Length == 10)
            {
                int result = 0;
                if (int.TryParse(arrToken[0], out result))
                    empID = result;

                if (int.TryParse(arrToken[2], out result))
                    attID = result;

                ecmID = arrToken[3];

                if (int.TryParse(arrToken[4], out result))
                    maID = result;

                docCode = arrToken[5];
                fileName = arrToken[6];

                if (int.TryParse(arrToken[7], out result))
                    pvdID = result;

                des = arrToken[8];

                bool isBool = false;
                if (bool.TryParse(arrToken[9], out isBool))
                    isEdit = isBool;
            }
        }

        public static string DecompressString(string compressedText)
        {
            if (string.IsNullOrWhiteSpace(compressedText))
                return compressedText;

            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (System.IO.Compression.GZipStream gZipStream = new System.IO.Compression.GZipStream(
                    memoryStream, System.IO.Compression.CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static string CompressString(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            byte[] rawData = Encoding.UTF8.GetBytes(text);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                using (System.IO.Compression.GZipStream gZipStream = new System.IO.Compression.GZipStream(
                    memoryStream, System.IO.Compression.CompressionMode.Compress, true))
                {
                    gZipStream.Write(rawData, 0, rawData.Length);
                }

                memoryStream.Position = 0;
                byte[] compressedData = new byte[memoryStream.Length];
                memoryStream.Read(compressedData, 0, compressedData.Length);

                byte[] gzipData = new byte[compressedData.Length + 4];
                Buffer.BlockCopy(compressedData, 0, gzipData, 4, compressedData.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(rawData.Length), 0, gzipData, 0, 4);

                return Convert.ToBase64String(gzipData);
            }
        }

        #region Encrypt/Decrypt
        private const string Password = "4iuZAGyGyDoDZte7g+IfJQ=="; // CMV@NoneAuthentication

        #region Encrypt
        public static string Encrypt(params object[] arrParam)
        {
            if (arrParam == null || arrParam.Length == 0)
                return string.Empty;

            string plainText = string.Join(CHAR_ESCAPE.ToString(), (from c in arrParam select c.ToString()));
            return Encrypt(plainText);
        }

        /// <summary>
        /// Encrypt a plain text using provided key.
        /// </summary>
        /// <param name="key">Key to Encrypt. Not null or empty.</param>
        /// <param name="plainText">Plain text</param>
        /// <returns>Encrypted text</returns>
        private static string Encrypt(string plainText, string defaultPassword = Password)
        {
            // Check arguments. 
            if (string.IsNullOrWhiteSpace(plainText))
                return string.Empty;

            byte[] bKey = Convert.FromBase64String(defaultPassword);
            var bEncrypted = EncryptStringToBytes(plainText, bKey, bKey);

            var encryptedText = Convert.ToBase64String(bEncrypted);

            return String2Base64String(encryptedText);
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create an Aes object 
            // with the specified key and IV. 
            using (System.Security.Cryptography.Aes aesAlg = System.Security.Cryptography.Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                System.Security.Cryptography.ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (System.IO.MemoryStream msEncrypt = new System.IO.MemoryStream())
                {
                    using (System.Security.Cryptography.CryptoStream csEncrypt = new System.Security.Cryptography.CryptoStream(
                        msEncrypt, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        using (System.IO.StreamWriter swEncrypt = new System.IO.StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }

        private static string String2Base64String(string value)
        {
            byte[] arrData = System.Text.Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(arrData);
        }
        #endregion

        #region Decrypt
        public static List<string> Decrypt(string cypherText)
        {
            if (string.IsNullOrWhiteSpace(cypherText))
                return new List<string>();

            string plainText = Decrypt(cypherText, Password);
            string[] arrToken = plainText.Split(new char[] { CHAR_ESCAPE });

            return arrToken.ToList();
        }

        /// <summary>
        /// Decrypt a text using provided key.
        /// </summary>
        /// <param name="key">Key to Decrypt. Not null or empty.</param>
        /// <param name="plainText">Encrypted text</param>
        /// <returns>Plain text</returns>
        private static string Decrypt(string encryptedText, string defaultPassword = Password)
        {
            // Check arguments.
            if (string.IsNullOrWhiteSpace(encryptedText))
                return string.Empty;

            byte[] bKey = Convert.FromBase64String(defaultPassword);
            byte[] bEncrypted = Convert.FromBase64String(Base64String2String(encryptedText));

            string plainText = DecryptStringFromBytes(bEncrypted, bKey, bKey);
            return plainText;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (System.Security.Cryptography.Aes aesAlg = System.Security.Cryptography.Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                System.Security.Cryptography.ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (System.IO.MemoryStream msDecrypt = new System.IO.MemoryStream(cipherText))
                {
                    using (System.Security.Cryptography.CryptoStream csDecrypt = new System.Security.Cryptography.CryptoStream(
                        msDecrypt, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                    {
                        using (System.IO.StreamReader srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        private static string Base64String2String(string value)
        {
            byte[] arrData = Convert.FromBase64String(value);
            return System.Text.Encoding.UTF8.GetString(arrData);
        }
        #endregion

        #endregion

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }
}