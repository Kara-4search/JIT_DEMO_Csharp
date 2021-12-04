using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JIT_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Target targetInstance = new Target();

            targetInstance.test();

            Injection.install(1);
            Injection.install(2);
            Injection.install(3);
            Injection.install(4);

            targetInstance.test();

            Console.Read();
        }
    }

    public class Target
    {
        public void test()
        {
            targetMethod1();
            Console.WriteLine(targetMethod2());
            targetMethod3("Test");
            targetMethod4();
        }

        private void targetMethod1()
        {
            Console.WriteLine("Target.targetMethod1()");

        }

        private string targetMethod2()
        {
            Console.WriteLine("Target.targetMethod2()");
            return "Not injected 2";
        }

        public void targetMethod3(string text)
        {
            Console.WriteLine("Target.targetMethod3(" + text + ")");
        }

        private void targetMethod4()
        {
            Console.WriteLine("Target.targetMethod4()");
        }
    }

    public class Injection
    {
        public static void install(int funcNum)
        {
            MethodInfo methodToReplace = typeof(Target).GetMethod("targetMethod" + funcNum, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            MethodInfo methodToInject = typeof(Injection).GetMethod("injectionMethod" + funcNum, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
            RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);

            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    int* inj = (int*)methodToInject.MethodHandle.Value.ToPointer() + 2;
                    int* tar = (int*)methodToReplace.MethodHandle.Value.ToPointer() + 2;


#if DEBUG
                    Console.WriteLine("\nVersion x86 Debug\n");

                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;

                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    Console.WriteLine("\nVersion x86 Release\n");
                    *tar = *inj;
#endif
                }
                else
                {

                    long* inj = (long*)methodToInject.MethodHandle.Value.ToPointer() + 1;
                    long* tar = (long*)methodToReplace.MethodHandle.Value.ToPointer() + 1;
                    

                    Console.WriteLine("*Pointer Address");
                    Console.WriteLine($"\t[*] Relative Address: 0x{(IntPtr)inj:X16}");
                    Console.WriteLine($"\t[*] Relative Address: 0x{(IntPtr)tar:X16}\n");


                    IntPtr pMethod = methodToInject.MethodHandle.GetFunctionPointer();
                    IntPtr pMethod2 = methodToReplace.MethodHandle.GetFunctionPointer();

                    Console.WriteLine("pPointer Address");
                    Console.WriteLine($"\t[*] Relative Address: 0x{pMethod:X16}");
                    Console.WriteLine($"\t[*] Relative Address: 0x{pMethod2:X16}\n");
                    Console.Write($"{Marshal.ReadByte(pMethod, -1):X2} # ");
                    Console.Write($"{Marshal.ReadByte(pMethod, 0):X2}");
                    Console.Write($"{Marshal.ReadByte(pMethod, 1):X2}");
                    Console.Write($"{Marshal.ReadByte(pMethod, 2):X2}");
                    Console.Write($"{Marshal.ReadByte(pMethod, 3):X2}");
                    Console.Write($"{Marshal.ReadByte(pMethod, 4):X2}");
                    Console.Write($"{Marshal.ReadByte(pMethod, 5):X2}");
                    Console.Write($"{Marshal.ReadByte(pMethod, 6):X2}");
                    Console.Write($"{Marshal.ReadByte(pMethod, 7):X2}");
                    Console.Write($" # {Marshal.ReadByte(pMethod, 8):X2}");
                    System.Threading.Thread.Sleep(10000);
#if DEBUG
                    Console.WriteLine("\nVersion x64 Debug\n");
                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;


                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    Console.WriteLine("\nVersion x64 Release\n");
                    *tar = *inj;
#endif
                }
            }
        }

        private void injectionMethod1()
        {
            Console.WriteLine("Injection.injectionMethod1");
        }

        private string injectionMethod2()
        {
            Console.WriteLine("Injection.injectionMethod2");
            return "Injected 2";
        }

        private void injectionMethod3(string text)
        {
            Console.WriteLine("Injection.injectionMethod3 " + text);
        }

        private void injectionMethod4()
        {
            System.Diagnostics.Process.Start("calc");
        }
    }

}