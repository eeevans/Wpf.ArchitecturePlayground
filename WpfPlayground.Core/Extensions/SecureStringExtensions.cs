using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>Converts to unsecure string.</summary>
        /// <param name="securePassword">The secure password.</param>
        /// <returns></returns>
        public static string ConvertToUnsecureString(this SecureString securePassword)
        {
            if (securePassword.IsNull())
                return string.Empty;
            IntPtr num = IntPtr.Zero;
            try
            {
                num = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(num);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(num);
            }
        }
    }
}
