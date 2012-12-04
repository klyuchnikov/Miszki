using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonLibrary
{
    /// <summary>
    /// Сеть Фейстеля - методов построения блочных шифров
    /// </summary>
    public class FeistelNetwork
    {
        /// <summary>
        /// Количество иттераций
        /// </summary>
        public byte Rounds { get; private set; }
        /// <summary>
        /// Длина блока
        /// </summary>
        public byte BlockLenth { get; private set; }

        /// <summary>
        /// Создание экземпляра
        /// </summary>
        /// <param name="BlockLenth">Количество иттераций</param>
        /// <param name="rounds">Длина блока</param>
        public FeistelNetwork(byte BlockLenth, byte rounds)
        {
            this.BlockLenth = BlockLenth;
            this.Rounds = rounds;
        }

        /// <summary>
        /// Функция преобразования данных, зависимая от ключа 
        /// </summary>
        /// <param name="data">Массив данных</param>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        private byte[] F(byte[] data, byte key)
        {
            var clone = (byte[])data.Clone();
            for (var i = 0; i < clone.Length; i++)
                clone[i] = (byte)(data[i] ^ key);
            return clone;
        }

        /// <summary>
        /// Функция ^ (xor) между двумя массивами байтов
        /// </summary>
        /// <param name="a">Массив данных</param>
        /// <param name="b">Массив данных</param>
        /// <returns>a ^ b</returns>
        private byte[] XOR(byte[] a, byte[] b)
        {
            var res = new byte[a.Length];
            for (var i = 0; i < a.Length; i++)
                res[i] = (byte)(a[i] ^ b[i]);
            return res;
        }

        /// <summary>
        /// Общая функция преобразования входного массива байтов 
        /// </summary>
        /// <param name="file">входной массив байтов</param>
        /// <param name="reverse">если false - шифрация, true - дешифрация</param>
        /// <param name="key">ключ</param>
        private void Crypt(ref byte[] file, bool reverse, string key)
        {
            var gen = new CongruentialGenerator(MaHash8v64.GetHashCode(key));
            var keys = new byte[Rounds];
            for (var i = 0; i < keys.Length; i++)
                keys[i] = (byte)gen.Next(byte.MaxValue);

            byte[] l = new byte[BlockLenth / 2];
            byte[] r = new byte[BlockLenth / 2];

            var lenthBlocks = Math.Truncate((double)(file.LongLength / BlockLenth));
            for (byte k = 0; k < Rounds; k++)
            {
                for (var i = 0; i < lenthBlocks; i++)
                {
                    l = file.Skip(i * BlockLenth).Take(BlockLenth / 2).ToArray();
                    r = file.Skip(i * BlockLenth + BlockLenth / 2).Take(BlockLenth / 2).ToArray();
                    if (i < Rounds - 1) // если не последний раунд
                    {
                        byte[] t = l;
                        l = XOR(F(l, keys[k]), r);
                        r = t;
                    }
                    else // последний раунд
                    {
                        r = XOR(F(l, keys[k]), r);
                    }
                    for (int j = 0; j < BlockLenth / 2; j++)
                    {
                        file[i * BlockLenth + j] = l[j];
                        file[i * BlockLenth + j + BlockLenth / 2] = r[j];
                    }
                }
            }
        }

        /// <summary>
        /// Шифрование
        /// </summary>
        /// <param name="file">входной массив байтов</param>
        /// <param name="key"> ключ</param>
        public void Encrypt(ref byte[] file, string key)
        {
            Crypt(ref file, false, key);
        }

        /// <summary>
        /// Дешифрование
        /// </summary>
        /// <param name="file">входной массив байтов</param>
        /// <param name="key">ключ</param>
        public void Decrypt(ref byte[] file, string key)
        {
            Crypt(ref file, true, key);
        }
    }
}
