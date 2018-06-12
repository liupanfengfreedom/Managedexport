using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;
using System.IO;

namespace Managedexport
{
    public class Class1
    {
/// <summary>
/// //////////////////////////////////
/// </summary>
        struct TestStruct
        {
            public int val;
            public int val2;
        }
        [DllExport("OutTestStruct", CallingConvention = CallingConvention.StdCall)]
        static void OutTestStruct(ref TestStruct testStruct)
        {
            testStruct.val = 7;
            testStruct.val2 = 8;
        }

        /// <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        private static T MarshalToStruct<T>(IntPtr buf)
        {
            return (T)Marshal.PtrToStructure(buf, typeof(T));
        }
        [DllExport("SumVector", CallingConvention = CallingConvention.StdCall)]
        static float SumVector(IntPtr vec)
        {
            MyVector v = MarshalToStruct<MyVector>(vec);
            return v.x + v.y + v.z;
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static IntPtr MarshalToPointer(object data)
        {
            IntPtr buf = Marshal.AllocHGlobal(
            Marshal.SizeOf(data));
            Marshal.StructureToPtr(data,
            buf, false);
            return buf;
        }
        struct MyVector
        {
            public float x, y, z;
        }
        [DllExport("ReturnStruct", CallingConvention = CallingConvention.StdCall)]
        static IntPtr ReturnStruct()
        {
            MyVector v = new MyVector();
            v.x = 0.45f;
            v.y = 0.56f;
            v.z = 0.24f;
            IntPtr lpstruct = MarshalToPointer(v);
            return lpstruct;
        }
/// <summary>
/// /////////////////////////////////////////////////////////////////////////
/// </summary>
/// <param name="left"></param>
/// <param name="right"></param>
/// <returns></returns>
        [DllExport("add", CallingConvention = CallingConvention.Cdecl)]
        public static int TestExport(int left, int right)

        {

            return left + right;

        }
        [DllExport("GetGreetingName", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String GetGreetingName([MarshalAs(UnmanagedType.LPWStr)] String msgName)
        {
            string temp;
            temp = "Happy Managed Coding " + msgName + "!";
         
            return temp;
        }
        [DllExport("Readdllpath", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String Readdllpath([MarshalAs(UnmanagedType.LPWStr)] String fileName)
        {
            string retVal = "";
            StreamReader test = null;
            try
            {
                retVal = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);//the path this dll is located;
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            finally
            {
                if (test != null)
                {
                    test.Close();
                }
            }
            return retVal;
        }
    }
}
