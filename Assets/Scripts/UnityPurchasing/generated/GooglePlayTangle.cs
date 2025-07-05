// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("49tD9UH8uGPw6iASS1bj05J/5fma83k9HNyCU4wpwI12GjbOximO+zcQT/oJOAhVoxclJQ0cR0xG0H29GZqUm6sZmpGZGZqamwC5UTX7JJehkgImLU/sW7DepawGNzAQjZK24JngUHbMNONEIUH/evzfmz+xCcJXnsNDVbGRi5ThPAcCZqooWXRn/OekP9lxsGwvOV5T7pMc2O7WvravbKsZmrmrlp2SsR3THWyWmpqanpuY17a2+vp2yBvH3D6zjZaHV90U45yd0v8pHlyVcJqDB2PTshDq/LGLUh5eYDUAJevshBf9rOdWbhJSAONaHb5Jqx1lejfDq9OfKPGnDTBx6Fm/5tyiLoNvuQJXPwkLihRfvndj/OYgJefCaUhsFJmYmpua");
        private static int[] order = new int[] { 7,3,5,3,12,13,8,12,12,12,12,11,13,13,14 };
        private static int key = 155;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
