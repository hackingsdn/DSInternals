﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSInternals.Common;

namespace DSInternals.DataStore.Test
{
    [TestClass]
    public class PrefixMapTester
    {
        // Sample prefix map after Exchange 2016 schema import
        private static byte[] ExchangeBinaryPrefixMap = "060000006200000023480b002a864886f7140104b658664f310c002a864886f7140104b658668331090c002a864886f7140105b6583e83dc600a002a864886f714010614014b6d0a002a864886f714010614024f750b002a864886f7140106140183".HexToBinary();

        [TestMethod]
        public void PrefixMap_Vector1()
        {
            var map = new PrefixMap(ExchangeBinaryPrefixMap);

            // Should contain 31 builtin refixes + 6 new
            Assert.AreEqual(37, map.Count);

            // Test one of the decoded prefixes
            bool contains1 = map.ContainsPrefix(18467);
            Assert.AreEqual(true, contains1);
            
            string prefix = map[18467];
            Assert.AreEqual("1.2.840.113556.1.4.7000.102", prefix);

            // Test non-existing prefix
            bool contains2 = map.ContainsPrefix(1234);
            Assert.AreEqual(false, contains2);
        }

        [TestMethod]
        public void PrefixMap_TranstaleBuiltin()
        {
            var map = new PrefixMap();

            // givenName
            string oid = map.Translate(42);
            Assert.AreEqual("2.5.4.42", oid);

            // objectSid
            oid = map.Translate(589970);
            Assert.AreEqual("1.2.840.113556.1.4.146", oid);

            // searchFlags
            oid = map.Translate(131406);
            Assert.AreEqual("1.2.840.113556.1.2.334", oid);
            
            // Entry-TTL
            oid = map.Translate(1769475);
            Assert.AreEqual("1.3.6.1.4.1.1466.101.119.3", oid);
        }

        [TestMethod]
        public void PrefixMap_TranstaleUser()
        {
            var map = new PrefixMap(ExchangeBinaryPrefixMap);

            // ms-Exch-Admins
            string oid = map.Translate(827294608);
            Assert.AreEqual("1.2.840.113556.1.4.7000.102.50064", oid);

            // ms-Exch-Folder-Affinity-List
            oid = map.Translate(1210264401);
            Assert.AreEqual("1.2.840.113556.1.4.7000.102.11089", oid);
        }


        [TestMethod]
        public void PrefixMap_NullInput()
        {
            byte[] binaryPrefixMap = null;
            var map = new PrefixMap(binaryPrefixMap);
            // Should only contain 31 builtin prefixes
            Assert.AreEqual(31, map.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PrefixMap_EmptyInput()
        {
            byte[] binaryPrefixMap = new byte[0];
            var map = new PrefixMap(binaryPrefixMap);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PrefixMap_InvalidInput1()
        {
            byte[] binaryPrefixMap = { 1, 2, 3, 4, 5 };
            var map = new PrefixMap(binaryPrefixMap);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PrefixMap_InvalidInput2()
        {
            byte[] binaryPrefixMap = { 1, 2, 3, 4, 5, 6, 7, 8 };
            var map = new PrefixMap(binaryPrefixMap);
        }
    }
}
