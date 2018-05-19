/*

The contents of this file are subject to the Mozilla Public License
Version 1.1 (the "License"); you may not use this file except in
compliance with the License. You may obtain a copy of the License at
http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS"
basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
License for the specific language governing rights and limitations
under the License.

The Original Code is OpenFAST.

The Initial Developer of the Original Code is The LaSalle Technology
Group, LLC.  Portions created by Shariq Muhammad
are Copyright (C) Shariq Muhammad. All Rights Reserved.

Contributor(s): Shariq Muhammad <shariq.muhammad@gmail.com>

*/
using NUnit.Framework;
using OpenFAST;
using UnitTest.Test;

namespace UnitTest
{
    [TestFixture]
    public class BitVectorTest
    {
        [Test]
        public void TestGetTruncatedBytes()
        {
            var vector = new BitVector(new byte[] { 0x00, 0x00 });
            Assert.IsTrue(vector.Overlong);
            TestUtil.AssertByteArrayEquals(new byte[] { 0x80 } ,
                vector.TruncatedBytes);

            vector = new BitVector(new byte[] { 0x00 });
            Assert.IsFalse(vector.Overlong);
            TestUtil.AssertByteArrayEquals(new byte[] { 0x80 },
                vector.TruncatedBytes);

            vector = new BitVector(new byte[] { 0x60, 0x00, 0x04, 0x00 });
            Assert.IsTrue(vector.Overlong);
            TestUtil.AssertByteArrayEquals(new byte[] { 0x60, 0x00, 0x84 },
                vector.TruncatedBytes);
        }

        [Test]
        public void TestGetBytes()
        {
            var vector = new BitVector(7);
            Assert.AreEqual(1, vector.Bytes.Length);
            vector = new BitVector(8);
            Assert.AreEqual(2, vector.Bytes.Length);
        }
        [Test]
        public void TestSetWithOneByte()
        {
            var vector = new BitVector(7);
            vector.set_Renamed(0);
            TestUtil.AssertBitVectorEquals("11000000", vector.Bytes);
            vector.set_Renamed(3);
            TestUtil.AssertBitVectorEquals("11001000", vector.Bytes);
            vector.set_Renamed(6);
            TestUtil.AssertBitVectorEquals("11001001", vector.Bytes);
        }
        [Test]
        public void TestIsSet()
        {
            var vector = new BitVector(7);
            Assert.IsFalse(vector.IsSet(1));
            vector.set_Renamed(1);
            Assert.IsTrue(vector.IsSet(1));
            Assert.IsFalse(vector.IsSet(6));
            vector.set_Renamed(6);
            Assert.IsTrue(vector.IsSet(6));
            Assert.IsFalse(vector.IsSet(7));
            Assert.IsFalse(vector.IsSet(8));
        }
        [Test]
        public void TestSetWithMultipleBytes()
        {
            var vector = new BitVector(15);
            vector.set_Renamed(0);
            TestUtil.AssertBitVectorEquals("01000000 00000000 10000000",
                vector.Bytes);
            vector.set_Renamed(4);
            TestUtil.AssertBitVectorEquals("01000100 00000000 10000000",
                vector.Bytes);
            vector.set_Renamed(9);
            TestUtil.AssertBitVectorEquals("01000100 00010000 10000000",
                vector.Bytes);
            vector.set_Renamed(14);
            TestUtil.AssertBitVectorEquals("01000100 00010000 11000000",
                vector.Bytes);
        }
        [Test]
        public void TestEquals()
        {
            var expected = new BitVector(new byte[] { 0xf0 });
            var actual = new BitVector(7);
            actual.set_Renamed(0);
            actual.set_Renamed(1);
            actual.set_Renamed(2);
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestIndexLastSet()
        {
            var bv = new BitVector(new byte[] { 0x70, 0x00, 0x04 });
            Assert.AreEqual(18, bv.IndexOfLastSet());
        }
    }
}
