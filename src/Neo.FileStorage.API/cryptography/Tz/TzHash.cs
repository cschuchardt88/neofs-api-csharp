// Copyright (C) 2015-2025 The Neo Project.
//
// TzHash.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.Cryptography.Tz
{
    public class TzHash : HashAlgorithm
    {
        public const int TzHashLength = 64;
        private GF127[] x;
        public override int HashSize => TzHashLength;

        public TzHash()
        {
            Initialize();
        }

        public override void Initialize()
        {
            x = new GF127[4];
            Reset();
            HashValue = null;
        }

        public void Reset()
        {
            x[0] = new GF127(1, 0);
            x[1] = new GF127(0, 0);
            x[2] = new GF127(0, 0);
            x[3] = new GF127(1, 0);
        }

        public byte[] ToByteArray()
        {
            var buff = new byte[HashSize];
            for (int i = 0; i < 4; i++)
            {
                Array.Copy(x[i].ToByteArray(), 0, buff, i * 16, 16);
            }
            return buff;
        }

        [SecurityCritical]
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            _ = HashData(array[ibStart..(ibStart + cbSize)]);
        }

        [SecurityCritical]
        protected override byte[] HashFinal()
        {
            return HashValue = ToByteArray();
        }

        [SecurityCritical]
        private int HashData(byte[] data)
        {
            var n = data.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = 7; j >= 0; j--)
                {
                    MulBitRight(ref x[0], ref x[1], ref x[2], ref x[3], (data[i] & (1 << j)) != 0);
                }
            }
            return n;
        }

        // MulBitRight() multiply A (if the bit is 0) or B (if the bit is 1) on the right side
        private void MulBitRight(ref GF127 c00, ref GF127 c01, ref GF127 c10, ref GF127 c11, bool bit)
        {
            // plan 1
            GF127 t;
            if (bit)
            {   // MulB
                t = c00;
                c00 = GF127.Mul10(c00) + c01; // c00 = c00 * x + c01
                c01 = GF127.Mul11(t) + c01;   // c01 = c00 * (x+1) + c01

                t = c10;
                c10 = GF127.Mul10(c10) + c11; // c10 = c10 * x + c11
                c11 = GF127.Mul11(t) + c11;   // c11 = c10 * (x+1) + c11
            }
            else
            {   // MulA
                t = c00;
                c00 = GF127.Mul10(c00) + c01; // c00 = c00 * x + c01
                c01 = t;                      // c01 = c00

                t = c10;
                c10 = GF127.Mul10(c10) + c11; // c10 = c10 * x + c11
                c11 = t;                      // c11 = c10;
            }

            //// plan 2
            //var r = new SL2(c00, c01, c10, c11);
            //if (bit)
            //    r.MulB();
            //else
            //    r.MulA();
        }

        // Concat() performs combining of hashes based on homomorphic characteristic.
        public static byte[] Concat(List<byte[]> hs)
        {
            var r = SL2.ID;
            foreach (var h in hs)
            {
                r *= new SL2().FromByteArray(h);
            }
            return r.ToByteArray();
        }

        // Validate() checks if hashes in hs combined are equal to h.
        public static bool Validate(byte[] h, List<byte[]> hs)
        {
            var expected = new SL2().FromByteArray(h);
            var actual = new SL2().FromByteArray(Concat(hs));
            return expected.Equals(actual);
        }

        // SubtractR() returns hash a, such that Concat(a, b) == c
        public static byte[] SubstractR(byte[] b, byte[] c)
        {
            var t1 = new SL2().FromByteArray(b);
            var t2 = new SL2().FromByteArray(c);
            var r = t2 * SL2.Inv(t1);
            return r.ToByteArray();
        }

        // SubtractL() returns hash b, such that Concat(a, b) == c
        public static byte[] SubstractL(byte[] a, byte[] c)
        {
            var t1 = new SL2().FromByteArray(a);
            var t2 = new SL2().FromByteArray(c);
            var r = SL2.Inv(t1) * t2;
            return r.ToByteArray();
        }
    }
}
