using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace CommonLibrary
{
    /// <summary>
    /// Пользовательский аттрибут, применяемый к свойствам класса, которые являются настройками метода и вписываются в кодируемый файл
    /// </summary>
    public class ParametrCryptAttribute : Attribute
    { }

    public enum CryptResult
    {
        /// <summary>
        /// Усшнешно
        /// </summary>
        Success,
        /// <summary>
        /// Несовпадение значений параметров записанных в файл с заданными
        /// </summary>
        MismatchValueParameters
    }

    public abstract class CrypterBlocks
    {
        public delegate void ComplitedHandler(CryptResult cryptResult, string password);
        public delegate void ComplitedStringHandler(string result);
        public delegate void ComplitedBytesHandler(byte[] result);

        public event ComplitedStringHandler EncryptStringComplitedEvent;
        public event ComplitedStringHandler DecryptStringComplitedEvent;
        public event ComplitedBytesHandler EncryptBytesComplitedEvent;
        public event ComplitedBytesHandler DecryptBytesComplitedEvent;
        public event ComplitedHandler DecryptComplitedEvent;
        public event ComplitedHandler EncryptComplitedEvent;


        protected delegate CryptResult CryptFunctionDelegate(string inputFile, string outputFile, bool reverse);
        protected delegate void CryptStringFunctionDelegate(ref string resstr);
        protected delegate void CryptBytesFunctionDelegate(byte[] data);

        public int MaxValueProcess { get; protected set; }

        public int CurrentValueProcess { get; protected set; }

        /// <summary>
        /// Пароль - ключ
        /// </summary>
        public string Password { get; protected set; }

        /// <summary>
        /// Количество подблоков
        /// </summary>
        [ParametrCryptAttribute]
        public byte SubBlocks { get; protected set; }


        /// <summary>
        /// Длина блока
        /// </summary>
        [ParametrCryptAttribute]
        public byte BlockLenth { get; protected set; }


        protected byte[] keys;

        public CrypterBlocks(byte BlockLenth, byte subBlocks, string pass)
        {
            this.BlockLenth = BlockLenth;
            this.SubBlocks = subBlocks;
            this.Password = pass;
            GenKeys(pass);
        }
        /// <summary>
        /// Генерация длины блоков псевдослучайным генератором, зависимым от входного ключа-пароля
        /// </summary>
        /// <param name="key"></param>
        protected abstract void GenKeys(string key);


        /// <summary>
        /// Основная функция изменения блока данных по данному методу
        /// </summary>
        /// <param name="isEncrypt">если true - шифрация, false - дешифрация</param>
        /// <param name="buffer">массив данных</param>
        /// <returns></returns>
        protected abstract byte[] CryptBlock(bool isEncrypt, byte[] buffer);

        /// <summary>
        /// Общая функция преобразования входного массива байтов
        /// </summary>
        /// <param name="inputFile">путь входного файла</param>
        /// <param name="outputFile">путь выходного файла</param>
        /// <param name="isEncrypt">если true - шифрация, false - дешифрация</param>
        /// <param name="key">ключ</param>
        protected virtual CryptResult Crypt(string inputFile, string outputFile, bool isEncrypt)
        {
            var inputstream = File.OpenRead(inputFile);
            var sr = new BinaryReader(inputstream);

            FileStream outputstream = null;
            BinaryWriter wr = null;

            this.CurrentValueProcess = 0;
            this.MaxValueProcess = (int)(inputstream.Length);

            if (isEncrypt)
            {
                outputstream = File.OpenWrite(outputFile);
                wr = new BinaryWriter(outputstream);
                var enc = MaHash8v64.GetHashCode(this.Password);
                // var bytesPass = Encoding.Unicode.GetBytes(enc);
                wr.Write(enc);
                var properties = this.GetType().GetProperties().Where(a => a.GetCustomAttributes(true).OfType<ParametrCryptAttribute>().Count() != 0).ToArray();
                foreach (var p in properties)
                    wr.Write((byte)p.GetValue(this, null));
            }
            else
            {
                var hashPass = sr.ReadUInt32();

                var properties = this.GetType().GetProperties().Where(a => a.GetCustomAttributes(true).OfType<ParametrCryptAttribute>().Count() != 0).ToArray();

                var paramsValue = new byte[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                    paramsValue[i] = sr.ReadByte();
                if (hashPass != MaHash8v64.GetHashCode(this.Password))
                    return CryptResult.MismatchValueParameters;
                for (var i = 0; i < properties.Length; i++)
                    if (paramsValue[i] != (byte)properties[i].GetValue(this, null))
                        return CryptResult.MismatchValueParameters;
            }
            if (wr == null)
            {
                outputstream = File.OpenWrite(outputFile);
                wr = new BinaryWriter(outputstream);
            }
            while (true)
            {
                var buffer = sr.ReadBytes(this.BlockLenth);
                if (buffer.Length == BlockLenth)
                    buffer = CryptBlock(isEncrypt, buffer);
                else
                {
                    this.CurrentValueProcess = this.MaxValueProcess - 1;
                    wr.Write(buffer);
                    break;
                }
                wr.Write(buffer);
                this.CurrentValueProcess += 10;
            }
            inputstream.Close();
            outputstream.Close();
            return CryptResult.Success;
        }

        protected CryptStringFunctionDelegate GetCryptStringDelegate(bool isEncrypt)
        {
            return new CryptStringFunctionDelegate(delegate(ref string strr)
            {
                strr = Encrypt(strr);
            });
        }

        protected CryptBytesFunctionDelegate GetCryptBytesDelegate(bool isEncrypt)
        {
            return new CryptBytesFunctionDelegate(delegate(byte[] data)
            {
                Encrypt(data);
            });
        }

        public virtual void EncryptAsync(string inputFile, string outputFile)
        {
            IAsyncResult res = null;
            var syncCurrent = SynchronizationContext.Current;
            var cfDelegate = new CryptFunctionDelegate(this.Crypt);
            res = cfDelegate.BeginInvoke(inputFile, outputFile, true, delegate
            {
                syncCurrent.Send(delegate
                {
                    var cryprres = cfDelegate.EndInvoke(res);
                    if (EncryptComplitedEvent != null)
                        EncryptComplitedEvent(cryprres, Password);
                }, null);
            }, null);
        }

        public virtual void DecryptAsync(string inputFile, string outputFile)
        {
            IAsyncResult res = null;
            var cfDelegate = new CryptFunctionDelegate(this.Crypt);
            var syncCurrent = SynchronizationContext.Current;
            res = cfDelegate.BeginInvoke(inputFile, outputFile, false, delegate
            {

                syncCurrent.Send(delegate
                {
                    var cryprres = cfDelegate.EndInvoke(res);
                    if (DecryptComplitedEvent != null)
                        DecryptComplitedEvent(cryprres, Password);
                }, null);
            }, null);
        }


        public virtual string Encrypt(string str)
        {
            var data = Encoding.Unicode.GetBytes(str);
            var res = CryptBlock(true, data);
            return Encoding.Unicode.GetString(res);
        }

        public virtual void EncryptAsync(string str)
        {
            IAsyncResult res = null;
            var resstr = str + "";

            var deleg = GetCryptStringDelegate(true);

            var syncCurrent = SynchronizationContext.Current;
            res = deleg.BeginInvoke(ref resstr, delegate
            {
                syncCurrent.Send(delegate
                {
                    deleg.EndInvoke(ref resstr, res);
                    if (EncryptStringComplitedEvent != null)
                        EncryptStringComplitedEvent(resstr);
                }, null);
            }, null);
        }

        public virtual string Decrypt(string str)
        {
            var data = Encoding.Unicode.GetBytes(str);
            var res = CryptBlock(true, data);
            return Encoding.Unicode.GetString(res);
        }

        public virtual void DecryptAsync(string str)
        {
            IAsyncResult res = null;
            var resstr = str + "";

            var deleg = GetCryptStringDelegate(false);
            var syncCurrent = SynchronizationContext.Current;
            res = deleg.BeginInvoke(ref resstr, delegate
            {
                syncCurrent.Send(delegate
                {
                    deleg.EndInvoke(ref resstr, res);
                    if (DecryptStringComplitedEvent != null)
                        DecryptStringComplitedEvent(resstr);
                }, null);
            }, null);
        }

        public virtual byte[] Encrypt(byte[] data)
        {
            var clone = (byte[])data.Clone();
            return CryptBlock(true, clone);
        }

        public virtual void EncryptAsync(byte[] data)
        {
            IAsyncResult res = null;

            var deleg = GetCryptBytesDelegate(true);
            var clone = (byte[])data.Clone();
            var syncCurrent = SynchronizationContext.Current;

            res = deleg.BeginInvoke(clone, delegate
            {
                syncCurrent.Send(delegate
                {
                    deleg.EndInvoke(res);
                    if (EncryptBytesComplitedEvent != null)
                        EncryptBytesComplitedEvent(clone);
                }, null);
            }, null);
        }

        public virtual byte[] Decrypt(byte[] data)
        {
            var clone = (byte[])data.Clone();
            return CryptBlock(false, clone);
        }

        public virtual void DecryptAsync(byte[] data)
        {
            IAsyncResult res = null;

            var deleg = GetCryptBytesDelegate(false);
            var clone = (byte[])data.Clone();
            var syncCurrent = SynchronizationContext.Current;
            res = deleg.BeginInvoke(clone, delegate
            {
                syncCurrent.Send(delegate
                {
                    deleg.EndInvoke(res);
                    if (DecryptBytesComplitedEvent != null)
                        DecryptBytesComplitedEvent(clone);
                }, null);
            }, null);
        }


    }
}
