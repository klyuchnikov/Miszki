using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public class ECB : CrypterBlocks
    {
        public ECB(byte BlockLenth, byte subBlocks, string pass)
            : base(BlockLenth, subBlocks, pass)
        {
            inds = new int[this.SubBlocks];
            keysDecrypt = new byte[this.SubBlocks];
            for (int i = 0; i < this.SubBlocks; i++)
                inds[i] = -1;
            var gen = new CongruentialGenerator(MaHash8v64.GetHashCode(this.Password));
            for (int i = 0; i < this.SubBlocks - 1; i++)
            {
                byte newValue;
                do
                {
                    newValue = (byte)gen.Next(0, this.SubBlocks - 1);
                } while (inds[newValue] != -1 || (newValue == 0 && i == 0));
                inds[newValue] = i;
            }
            for (int i = 0; i < this.SubBlocks; i++)
            {
                if (inds[i] == -1)
                    inds[i] = this.SubBlocks - 1;
                keysDecrypt[i] = keys[inds[i]];
            }
        }

        private int[] inds;
        private byte[] keysDecrypt;
        protected override byte[] CryptBlock(bool isEncrypt, byte[] buffer)
        {
            var newblock = new byte[BlockLenth];
            int ind = 0;
            for (int i = 0; i < this.SubBlocks; i++)
            {
                if (isEncrypt)
                    for (int k = 0; k < keys[inds[i]]; k++)
                        newblock[ind++] = buffer[keys.Take(inds[i]).Sum(a => a) + k];
                else
                    for (int k = 0; k < keysDecrypt[inds[i]]; k++)
                        newblock[ind++] = buffer[keysDecrypt.Take(inds[i]).Sum(a => a) + k];

            }
            return newblock;
        }

        protected override void GenKeys(string key)
        {
            var gen = new CongruentialGenerator(MaHash8v64.GetHashCode(key));
            keys = new byte[this.SubBlocks];
            for (var i = 0; i < keys.Length - 1; i++)
                keys[i] = (byte)gen.Next(1, this.BlockLenth - keys.Sum(a => a) - (this.SubBlocks - i - 1));
            keys[keys.Length - 1] = (byte)(this.BlockLenth - keys.Sum(a => a));
        }
    }
}
